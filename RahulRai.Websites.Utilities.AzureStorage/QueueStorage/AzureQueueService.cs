// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.AzureStorage
// Author           : rahulrai
// Created          : 08-20-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-20-2015
// ***********************************************************************
// <copyright file="AzureQueueService.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.AzureStorage.QueueStorage
{
    #region

    using System;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;

    using RahulRai.Websites.Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class AzureQueueService.
    /// </summary>
    public class AzureQueueService
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueueService"/> class.
        /// </summary>
        /// <param name="storageAccountConnectionString">The storage account connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        public AzureQueueService(string storageAccountConnectionString, string queueName)
        {
            this.QueueClient = CloudStorageAccount.Parse(storageAccountConnectionString).CreateCloudQueueClient();
            this.QueueClient.DefaultRequestOptions.RetryPolicy =
                new ExponentialRetry(
                    TimeSpan.FromSeconds(CustomRetryPolicy.RetryBackOffSeconds),
                    CustomRetryPolicy.MaxRetries);
            this.CloudQueue = this.QueueClient.GetQueueReference(queueName);
            this.CloudQueue.CreateIfNotExists();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cloud queue.
        /// </summary>
        /// <value>The cloud queue.</value>
        private CloudQueue CloudQueue { get; set; }

        /// <summary>
        /// Gets or sets the queue client.
        /// </summary>
        /// <value>The queue client.</value>
        private CloudQueueClient QueueClient { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the message to queue.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddMessageToQueue(string message)
        {
            this.CloudQueue.AddMessage(new CloudQueueMessage(message));
        }

        #endregion
    }
}