// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.AzureStorage
// Author           : rahulrai
// Created          : 04-15-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="BlobStorageService.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.AzureStorage.BlobStorage
{
    #region

    using System;
    using System.IO;
    using System.Web;
    using Common.Entities;
    using Common.Exceptions;
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
    public class BlobStorageService
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BlobStorageService" /> class.
        /// </summary>
        /// <param name="storageAccountConnectionString">The storage account connection string.</param>
        public BlobStorageService(string storageAccountConnectionString)
        {
            this.BlobClient = CloudStorageAccount.Parse(storageAccountConnectionString).CreateCloudBlobClient();
            this.BlobClient.DefaultRequestOptions.RetryPolicy =
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
        /// <value>The BLOB client.</value>
        private CloudBlobClient BlobClient { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The add file to folder.
        /// </summary>
        /// <param name="containerName">The folder name.</param>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="blobName">The file name.</param>
        /// <returns>The <see cref="FileOperationStatus" />.</returns>
        /// <exception cref="BlogSystemException">Failed to add blob to container</exception>
        /// <exception cref="RahulRai.Websites.Utilities.Common.Exceptions.BlogSystemException">Failed to add blob to container</exception>
        public Uri AddBlobToContainer(string containerName, Stream fileStream, string blobName)
        {
            var container = this.BlobClient.GetContainerReference(containerName);
            var blockBlob = container.GetBlockBlobReference(blobName);
            blockBlob.Properties.ContentType = MimeMapping.GetMimeMapping(blobName);
            blockBlob.UploadFromStream(fileStream);
            return blockBlob.Uri;
        }

        /// <summary>
        ///     The create container.
        /// </summary>
        /// <param name="containerName">The folder name.</param>
        /// <param name="visibilityType">The visibility type.</param>
        /// <returns>The <see cref="FileOperationStatus" />.</returns>
        public FileOperationStatus CreateContainer(string containerName, VisibilityType visibilityType)
        {
            this.CreateContainerWithPermissions(containerName, visibilityType);
            return FileOperationStatus.FolderCreated;
        }

        /// <summary>
        ///     The delete file.
        /// </summary>
        /// <param name="containerName">The folder name.</param>
        /// <param name="blobName">The file name.</param>
        /// <returns>The <see cref="FileOperationStatus" />.</returns>
        public FileOperationStatus DeleteBlob(string containerName, string blobName)
        {
            var container = this.BlobClient.GetContainerReference(containerName);
            var blockBlob = container.GetBlockBlobReference(blobName);
            blockBlob.DeleteIfExists();
            return FileOperationStatus.FileDeleted;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The create container with permissions.
        /// </summary>
        /// <param name="folderName">The folder name.</param>
        /// <param name="visibilityType">The visibility type.</param>
        /// <returns>The <see cref="CloudBlobContainer" />.</returns>
        private CloudBlobContainer CreateContainerWithPermissions(string folderName, VisibilityType visibilityType)
        {
            var containerPermission = new BlobContainerPermissions();
            var container = this.BlobClient.GetContainerReference(folderName.ToLowerInvariant());
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