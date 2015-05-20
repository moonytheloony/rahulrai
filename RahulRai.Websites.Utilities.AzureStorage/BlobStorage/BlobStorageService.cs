namespace RahulRai.Websites.Utilities.AzureStorage.BlobStorage
{
    #region

    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Web;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Common.Exceptions;
    using Common.RegularTypes;
    using Common.Entities;
    using Common.Helpers;

    #endregion

    #region

    #endregion

    /// <summary>
    ///     The azure blob repository.
    /// </summary>
    public class BlobStorageService
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BlobStorageService" /> class.
        /// </summary>
        /// <param name="context">
        ///     The context.
        /// </param>
        public BlobStorageService(string storageAccountConnectionString)
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
        ///     Prevents a default instance of the <see cref="BlobStorageService" /> class from being created.
        /// </summary>
        private BlobStorageService()
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
        /// <param name="containerName">
        ///     The folder name.
        /// </param>
        /// <param name="fileStream">
        ///     The file stream.
        /// </param>
        /// <param name="blobName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus AddBlobToContainer(string containerName, Stream fileStream, string blobName)
        {
            try
            {
                var container = BlobClient.GetContainerReference(containerName);
                var blockBlob = container.GetBlockBlobReference(blobName);
                blockBlob.Properties.ContentType = MimeMapping.GetMimeMapping(blobName);
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
        ///     The create container.
        /// </summary>
        /// <param name="containerName">
        ///     The folder name.
        /// </param>
        /// <param name="visibilityType">
        ///     The visibility type.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus CreateContainer(string containerName, VisibilityType visibilityType)
        {
            try
            {
                CreateContainerWithPermissions(containerName, visibilityType);
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
        ///     The delete file.
        /// </summary>
        /// <param name="containerName">
        ///     The folder name.
        /// </param>
        /// <param name="blobName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        public FileOperationStatus DeleteBlob(string containerName, string blobName)
        {
            try
            {
                var container = BlobClient.GetContainerReference(containerName);
                var blockBlob = container.GetBlockBlobReference(blobName);
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
        ///     The download file.
        /// </summary>
        /// <param name="containerName">
        ///     The folder name.
        /// </param>
        /// <param name="blobName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="Stream" />.
        /// </returns>
        public DataFile DownloadBlob(string containerName, string blobName)
        {
            var container = BlobClient.GetContainerReference(containerName);
            var blockBlob = container.GetBlockBlobReference(blobName);
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