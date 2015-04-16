namespace RahulRai.Websites.Utilities.AzureStorage.BlobStorage
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Common.Entities;
    using Common.Exceptions;
    using Common.Helpers;
    using Common.RegularTypes;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;

    #endregion

    #region

    #endregion

    /// <summary>
    ///     The azure blob repository.
    /// </summary>
    public class AzureBlobRepository
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AzureBlobRepository" /> class.
        /// </summary>
        /// <param name="context">
        ///     The context.
        /// </param>
        public AzureBlobRepository(string storageAccountConnectionString)
        {
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrWhiteSpace(storageAccountConnectionString), "storageAccountConnectionString");
            BlobClient = CloudStorageAccount.Parse(storageAccountConnectionString).CreateCloudBlobClient();
            BlobClient.DefaultRequestOptions.RetryPolicy =
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

        #region Properties

        /// <summary>
        ///     Gets or sets the repository client.
        /// </summary>
        private CloudBlobClient BlobClient { get; set; }

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
                var container = BlobClient.GetContainerReference(folderName);
                var blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.Properties.ContentType = MimeMapping.GetMimeMapping(fileName);
                blockBlob.UploadFromStream(fileStream);
                return FileOperationStatus.FileCreatedOrUpdated;
            }
            catch (StorageException exception)
            {
                Trace.TraceError(
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
                var container = BlobClient.GetContainerReference(folderName);
                var blobDirectory = container.GetDirectoryReference(directoryName);
                var blockBlob = blobDirectory.GetBlockBlobReference(fileName);
                blockBlob.Properties.ContentType = MimeMapping.GetMimeMapping(fileName);

                blockBlob.UploadFromStream(fileStream);
            }
            catch (StorageException exception)
            {
                Trace.TraceError(
                    Routines.FormatStringInvariantCulture(
                        "File operation failed with the following error {0}",
                        exception.Message),
                    null);
                return FileOperationStatus.Error;
            }

            return FileOperationStatus.FileCreatedOrUpdated;
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
        public FileOperationStatus CopyFileWithinStorageAccount(
            string sourceFolder,
            string fileName,
            string targetFolder,
            string targetFileName)
        {
            try
            {
                var sourceContainer = BlobClient.GetContainerReference(sourceFolder);
                var targetContainer = BlobClient.GetContainerReference(targetFolder);
                var sourceBlob = sourceContainer.GetBlockBlobReference(fileName);
                var targetBlob = targetContainer.GetBlockBlobReference(targetFileName);
                targetBlob.StartCopyFromBlob(sourceBlob);
                return FileOperationStatus.InitiatedCopyOperation;
            }
            catch (StorageException exception)
            {
                Trace.TraceError(
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
                Trace.TraceError(
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
                Trace.TraceError(
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
                var container = BlobClient.GetContainerReference(folderName);
                var directory = container.GetDirectoryReference(directoryName);
                foreach (
                    var blockBlob in
                        directory.ListBlobs(true)
                            .Select(file => BlobClient.GetBlobReferenceFromServer(file.Uri)))
                {
                    blockBlob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
                }
            }
            catch (StorageException exception)
            {
                Trace.TraceError(
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
                var container = BlobClient.GetContainerReference(folderName);
                var blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.DeleteIfExists();
            }
            catch (StorageException exception)
            {
                Trace.TraceError(
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
                var blockBlob = BlobClient.GetBlobReferenceFromServer(fullyQualifiedFileName);
                blockBlob.DeleteIfExists();
            }
            catch (StorageException exception)
            {
                Trace.TraceError(
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
                var container = BlobClient.GetContainerReference(folderName);
                container.DeleteIfExists();
                return FileOperationStatus.FolderDeleted;
            }
            catch (StorageException exception)
            {
                Trace.TraceError(
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
            var container = BlobClient.GetContainerReference(folderName);
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
            var blockBlob = BlobClient.GetBlobReferenceFromServer(fullyQualifiedFileName);
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
            var container = BlobClient.GetContainerReference(folderName);
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
            var container = BlobClient.GetContainerReference(folderName);
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
            var container = BlobClient.GetContainerReference(folderName);
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
            var containers = BlobClient.ListContainers(null, ContainerListingDetails.All);
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
            var container = BlobClient.GetContainerReference(folderName);
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

            var container = BlobClient.GetContainerReference(folderName);
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

            var blockBlob = BlobClient.GetBlobReferenceFromServer(fullyQualifiedFileName);
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
            var container = BlobClient.GetContainerReference(folderName.ToLowerInvariant());
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