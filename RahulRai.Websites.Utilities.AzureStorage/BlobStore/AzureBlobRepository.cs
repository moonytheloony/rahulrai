#region



#endregion

namespace RahulRai.Websites.Utilities.AzureStorage.BlobStore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading;
    using Repositories;

    #region

    

    #endregion

    /// <summary>
    ///     The azure blob repository.
    /// </summary>
    public class AzureBlobRepository : IFileRepository
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AzureBlobRepository" /> class.
        /// </summary>
        /// <param name="context">
        ///     The context.
        /// </param>
        public AzureBlobRepository(IContext context)
        {
            Contract.Requires<InputValidationFailedException>(null != context, "context");
            Context = context;
            RepositoryClient = CloudStorageAccount.Parse(context.FileServerAccessString).CreateCloudBlobClient();
            RepositoryClient.DefaultRequestOptions.RetryPolicy =
                new ExponentialRetry(
                    TimeSpan.FromSeconds(CustomRetryPolicy.RetryBackOffSeconds),
                    CustomRetryPolicy.MaxRetries);
        }

        /// <summary>
        ///     Prevents a default instance of the <see cref="AzureBlobRepository" /> class from being created.
        /// </summary>
        private AzureBlobRepository()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the context.
        /// </summary>
        public IContext Context { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the repository client.
        /// </summary>
        private CloudBlobClient RepositoryClient { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The add file to folder.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileStream">
        ///     The file stream.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus AddFileToFolder(string folderName, Stream fileStream, string fileName)
        {
            try
            {
                var container = RepositoryClient.GetContainerReference(folderName);
                var blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.Properties.ContentType = MimeMapping.GetMimeMapping(fileName);
                blockBlob.UploadFromStream(fileStream);
                return FileOperationStatus.FileCreatedOrUpdated;
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }
        }

        /// <summary>
        ///     The add file to sub directory.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="directoryName">
        ///     The directory name.
        /// </param>
        /// <param name="fileStream">
        ///     The file stream.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus AddFileToSubdirectory(
            string folderName,
            string directoryName,
            Stream fileStream,
            string fileName)
        {
            try
            {
                var container = RepositoryClient.GetContainerReference(folderName);
                var blobDirectory = container.GetDirectoryReference(directoryName);
                var blockBlob = blobDirectory.GetBlockBlobReference(fileName);
                blockBlob.Properties.ContentType = MimeMapping.GetMimeMapping(fileName);

                blockBlob.UploadFromStream(fileStream);
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }

            return FileOperationStatus.FileCreatedOrUpdated;
        }

        /// <summary>
        ///     The copy file across server.
        /// </summary>
        /// <param name="sourceFolder">
        ///     The source folder.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="targetContext">
        ///     The target context.
        /// </param>
        /// <param name="targetFolder">
        ///     The target folder.
        /// </param>
        /// <param name="targetFileName">
        ///     The target File Name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus CopyFileAcrossServer(
            string sourceFolder,
            string fileName,
            IContext targetContext,
            string targetFolder,
            string targetFileName)
        {
            try
            {
                var sourceContainer = RepositoryClient.GetContainerReference(sourceFolder);
                var cloudContext = targetContext as CloudContext;
                if (cloudContext != null)
                {
                    var targetRepository = cloudContext.FileRepository as AzureBlobRepository;
                    if (targetRepository != null)
                    {
                        var targetContainer = targetRepository.RepositoryClient.GetContainerReference(targetFolder);
                        var sourceBlob = sourceContainer.GetBlockBlobReference(fileName);
                        var targetBlob = targetContainer.GetBlockBlobReference(targetFileName);
                        targetBlob.StartCopyFromBlob(sourceBlob);
                        return FileOperationStatus.InitiatedCopyOperation;
                    }
                }
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }

            return FileOperationStatus.Error;
        }

        /// <summary>
        ///     The copy file across server synchronous.
        /// </summary>
        /// <param name="sourceFolder">
        ///     The source folder.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="targetContext">
        ///     The target context.
        /// </param>
        /// <param name="targetFolder">
        ///     The target folder.
        /// </param>
        /// <param name="targetFileName">
        ///     The target File Name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus CopyFileAcrossServerSynchronous(
            string sourceFolder,
            string fileName,
            IContext targetContext,
            string targetFolder,
            string targetFileName)
        {
            try
            {
                var synchronusCounter = 0;
                var sourceContainer = RepositoryClient.GetContainerReference(sourceFolder);
                var cloudContext = targetContext as CloudContext;
                if (cloudContext != null)
                {
                    var targetRepository = cloudContext.FileRepository as AzureBlobRepository;
                    if (targetRepository != null)
                    {
                        var targetContainer = targetRepository.RepositoryClient.GetContainerReference(targetFolder);
                        var sourceBlob = sourceContainer.GetBlockBlobReference(fileName);
                        var targetBlob = targetContainer.GetBlockBlobReference(targetFileName);
                        targetBlob.StartCopyFromBlob(sourceBlob);
                        while (10 > synchronusCounter++)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(5));
                            switch (targetBlob.CopyState.Status)
                            {
                                case CopyStatus.Aborted:
                                case CopyStatus.Failed:
                                case CopyStatus.Invalid:
                                    return FileOperationStatus.Error;
                                case CopyStatus.Success:
                                    return FileOperationStatus.CopyCompleted;
                            }
                        }

                        return FileOperationStatus.Timeout;
                    }
                }
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }

            return FileOperationStatus.Error;
        }

        /// <summary>
        ///     The copy file within server.
        /// </summary>
        /// <param name="sourceFolder">
        ///     The source folder.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="targetFolder">
        ///     The target folder.
        /// </param>
        /// <param name="targetFileName">
        ///     The target File Name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus CopyFileWithinServer(
            string sourceFolder,
            string fileName,
            string targetFolder,
            string targetFileName)
        {
            try
            {
                var sourceContainer = RepositoryClient.GetContainerReference(sourceFolder);
                var targetContainer = RepositoryClient.GetContainerReference(targetFolder);
                var sourceBlob = sourceContainer.GetBlockBlobReference(fileName);
                var targetBlob = targetContainer.GetBlockBlobReference(targetFileName);
                targetBlob.StartCopyFromBlob(sourceBlob);
                return FileOperationStatus.InitiatedCopyOperation;
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }
        }

        /// <summary>
        ///     The copy file within server synchronous.
        /// </summary>
        /// <param name="sourceFolder">
        ///     The source folder.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="targetFolder">
        ///     The target folder.
        /// </param>
        /// <param name="targetFileName">
        ///     The target File Name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus CopyFileWithinServerSynchronous(
            string sourceFolder,
            string fileName,
            string targetFolder,
            string targetFileName)
        {
            try
            {
                var synchronusCounter = 0;
                var sourceContainer = RepositoryClient.GetContainerReference(sourceFolder);
                var targetContainer = RepositoryClient.GetContainerReference(targetFolder);
                var sourceBlob = sourceContainer.GetBlockBlobReference(fileName);
                var targetBlob = targetContainer.GetBlockBlobReference(targetFileName);
                targetBlob.StartCopyFromBlob(sourceBlob);
                while (10 > synchronusCounter++)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    switch (targetBlob.CopyState.Status)
                    {
                        case CopyStatus.Aborted:
                        case CopyStatus.Failed:
                        case CopyStatus.Invalid:
                            return FileOperationStatus.Error;
                        case CopyStatus.Success:
                            return FileOperationStatus.CopyCompleted;
                    }
                }

                return FileOperationStatus.Timeout;
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }
        }

        /// <summary>
        ///     The create folder.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="visibilityType">
        ///     The visibility type.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus CreateFolder(string folderName, VisibilityType visibilityType)
        {
            try
            {
                CreateContainerWithPermissions(folderName, visibilityType);
                return FileOperationStatus.FolderCreated;
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }
        }

        /// <summary>
        ///     The create folder.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="visibilityType">
        ///     The visibility type.
        /// </param>
        /// <param name="folderMetadata">
        ///     The folder metadata.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus CreateFolder(
            string folderName,
            VisibilityType visibilityType,
            IDictionary<string, string> folderMetadata)
        {
            try
            {
                var container = CreateContainerWithPermissions(folderName, visibilityType);
                foreach (var metaData in folderMetadata)
                {
                    container.Metadata.Add(metaData.Key, metaData.Value);
                }

                container.SetMetadata();
                return FileOperationStatus.FolderCreated;
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }
        }

        /// <summary>
        ///     The delete directory.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="directoryName">
        ///     The directory name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus DeleteDirectory(string folderName, string directoryName)
        {
            try
            {
                var container = RepositoryClient.GetContainerReference(folderName);
                var directory = container.GetDirectoryReference(directoryName);
                foreach (
                    var blockBlob in
                        directory.ListBlobs(true)
                            .Select(file => RepositoryClient.GetBlobReferenceFromServer(file.Uri)))
                {
                    blockBlob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
                }
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }

            return FileOperationStatus.FileDeleted;
        }

        /// <summary>
        ///     The delete file.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus DeleteFile(string folderName, string fileName)
        {
            try
            {
                var container = RepositoryClient.GetContainerReference(folderName);
                var blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.DeleteIfExists();
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }

            return FileOperationStatus.FileDeleted;
        }

        /// <summary>
        ///     The delete file.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus DeleteFile(Uri fullyQualifiedFileName)
        {
            try
            {
                var blockBlob = RepositoryClient.GetBlobReferenceFromServer(fullyQualifiedFileName);
                blockBlob.DeleteIfExists();
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }

            return FileOperationStatus.FileDeleted;
        }

        /// <summary>
        ///     The delete folder.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus DeleteFolder(string folderName)
        {
            try
            {
                var container = RepositoryClient.GetContainerReference(folderName);
                container.DeleteIfExists();
                return FileOperationStatus.FolderDeleted;
            }
            catch (StorageException exception)
            {
                AuditAndTraceWriter.WriteToErrorLog(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }
        }

        /// <summary>
        ///     The download file.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="Stream" />.
        /// </returns>
        public DataFile DownloadFile(string folderName, string fileName)
        {
            var container = RepositoryClient.GetContainerReference(folderName);
            var blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.FetchAttributes();
            var fileContent = new byte[blockBlob.Properties.Length];
            blockBlob.DownloadToByteArray(fileContent, 0);
            return new DataFile
            {
                FileContent = fileContent,
                FileEncodingType = blockBlob.Properties.ContentType,
                FileName = blockBlob.Name
            };
        }

        /// <summary>
        ///     The download file.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <returns>
        ///     The <see cref="Stream" />.
        /// </returns>
        public DataFile DownloadFile(Uri fullyQualifiedFileName)
        {
            var blockBlob = RepositoryClient.GetBlobReferenceFromServer(fullyQualifiedFileName);
            var fileContent = new byte[blockBlob.Properties.Length];
            blockBlob.DownloadToByteArray(fileContent, 0);
            return new DataFile
            {
                FileContent = fileContent,
                FileEncodingType = blockBlob.Properties.ContentType,
                FileName = blockBlob.Name
            };
        }

        /// <summary>
        ///     The download file.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="targetFilePath">
        ///     The target file path.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus DownloadFile(string folderName, string fileName, string targetFilePath)
        {
            var container = RepositoryClient.GetContainerReference(folderName);
            var blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.DownloadToFile(targetFilePath, FileMode.CreateNew);
            return FileOperationStatus.FileCreatedOrUpdated;
        }

        /// <summary>
        ///     The enlist documents.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public IEnumerable<Document> EnlistDocuments(string folderName)
        {
            var container = RepositoryClient.GetContainerReference(folderName);
            var blockBlobs = container.ListBlobs(null, true, BlobListingDetails.All);
            return
                blockBlobs.Select(
                    blockBlob =>
                        new Document
                        {
                            Uri = blockBlob.Uri,
                            Name =
                                blockBlob.Uri.IsFile
                                    ? Path.GetFileName(blockBlob.Uri.LocalPath)
                                    : blockBlob.Uri.ToString()
                        });
        }

        /// <summary>
        ///     The enlist files in subdirectory.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="directoryName">
        ///     The directory name.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public IEnumerable<Document> EnlistFilesInSubdirectory(string folderName, string directoryName)
        {
            var container = RepositoryClient.GetContainerReference(folderName);
            var blobDirectory = container.GetDirectoryReference(directoryName);
            return
                blobDirectory.ListBlobs()
                    .Select(
                        blockBlob =>
                            new Document
                            {
                                Uri = blockBlob.Uri,
                                Name =
                                    blockBlob.Uri.IsFile
                                        ? Path.GetFileName(blockBlob.Uri.LocalPath)
                                        : blockBlob.Uri.ToString()
                            });
        }

        /// <summary>
        ///     The enlist folders.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public IEnumerable<Folder> EnlistFolders()
        {
            var containers = RepositoryClient.ListContainers(null, ContainerListingDetails.All);
            return containers.Select(container => new Folder {Name = container.Name, Metadata = container.Metadata});
        }

        /// <summary>
        ///     The enlist sub directories.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public IEnumerable<string> EnlistSubdirectories(string folderName)
        {
            var container = RepositoryClient.GetContainerReference(folderName);
            return from blobItem in container.ListBlobs()
                where blobItem is CloudBlobDirectory
                let directoryItem = blobItem as CloudBlobDirectory
                where directoryItem != null
                select directoryItem.Prefix;
        }

        /// <summary>
        ///     The get time bound file read access.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="fileReadAccessPeriod">
        ///     The file read access period.
        /// </param>
        /// <returns>
        ///     The <see cref="Uri" />.
        /// </returns>
        public Uri GetTimeBoundFileReadAccess(string folderName, string fileName, TimeSpan fileReadAccessPeriod)
        {
            var policy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = null,
                SharedAccessExpiryTime = DateTime.Now + fileReadAccessPeriod
            };

            var container = RepositoryClient.GetContainerReference(folderName);
            var blockBlob = container.GetBlockBlobReference(fileName);
            var sas = blockBlob.GetSharedAccessSignature(policy);
            return new Uri(Routines.FormatStringCurrentCulture(blockBlob.Uri.AbsoluteUri, sas));
        }

        /// <summary>
        ///     The get time bound file read access.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <param name="fileReadAccessPeriod">
        ///     The file read access period.
        /// </param>
        /// <returns>
        ///     The <see cref="Uri" />.
        /// </returns>
        public Uri GetTimeBoundFileReadAccess(Uri fullyQualifiedFileName, TimeSpan fileReadAccessPeriod)
        {
            var policy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = null,
                SharedAccessExpiryTime = DateTime.Now + fileReadAccessPeriod
            };

            var blockBlob = RepositoryClient.GetBlobReferenceFromServer(fullyQualifiedFileName);
            var sas = blockBlob.GetSharedAccessSignature(policy);
            return new Uri(Routines.FormatStringInvariantCulture("{0}{1}", blockBlob.Uri.AbsoluteUri, sas));
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The create container with permissions.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="visibilityType">
        ///     The visibility type.
        /// </param>
        /// <returns>
        ///     The <see cref="CloudBlobContainer" />.
        /// </returns>
        private CloudBlobContainer CreateContainerWithPermissions(string folderName, VisibilityType visibilityType)
        {
            var containerPermission = new BlobContainerPermissions();
            var container = RepositoryClient.GetContainerReference(folderName.ToLowerInvariant());
            container.CreateIfNotExists();
            switch (visibilityType)
            {
                case VisibilityType.FilesVisibleToAll:
                    containerPermission.PublicAccess = BlobContainerPublicAccessType.Blob;
                    break;
                case VisibilityType.FolderVisibleToAll:
                    containerPermission.PublicAccess = BlobContainerPublicAccessType.Container;
                    break;
                case VisibilityType.VisibleToOwnerOnly:
                    containerPermission.PublicAccess = BlobContainerPublicAccessType.Off;
                    break;
                default:
                    containerPermission.PublicAccess = BlobContainerPublicAccessType.Off;
                    break;
            }

            container.SetPermissions(containerPermission);
            return container;
        }

        #endregion
    }
}