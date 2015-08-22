// ***********************************************************************
// Assembly         : RahulRai.Websites.Jobs.CleanupWorker
// Author           : rahulrai
// Created          : 08-22-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-22-2015
// ***********************************************************************
// <copyright file="SubscriptionCleanupActivity.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Jobs.CleanupWorker
{
    #region

    using System;
    using System.Linq;

    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.Storage.Table.Queryable;

    using RahulRai.Websites.Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class SubscriptionCleanupActivity.
    /// </summary>
    public class SubscriptionCleanupActivity
    {
        #region Static Fields

        /// <summary>
        /// The table request options
        /// </summary>
        private static readonly TableRequestOptions TableRequestOptions = new TableRequestOptions
            {
                RetryPolicy =
                    new ExponentialRetry(
                        TimeSpan.FromSeconds(CustomRetryPolicy.RetryBackOffSeconds),
                        CustomRetryPolicy.MaxRetries)
            };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Cleanups the old subscribers.
        /// </summary>
        /// <param name="subscriberTable">The subscriber table.</param>
        public static void CleanupOldSubscribers(CloudTable subscriberTable)
        {
            try
            {
                var query = (from record in subscriberTable.CreateQuery<DynamicTableEntity>()
                             where
                                 record.PartitionKey == ApplicationConstants.SubscriberListKey
                                     && record.Properties["CreatedDate"].DateTime
                                         < (DateTime.UtcNow - TimeSpan.FromDays(7))
                                     && record.Properties["IsVerified"].BooleanValue == false
                             select record).Take(100).AsTableQuery();
                TableContinuationToken token = null;
                do
                {
                    var segment = subscriberTable.ExecuteQuerySegmented(query, token, TableRequestOptions);
                    var batchDelete = new TableBatchOperation();
                    if (null == segment || !segment.Any())
                    {
                        Console.Out.WriteLine("No users found. Aborting current call.");
                        return;
                    }

                    //// Log users we are going to delete.
                    foreach (var user in segment)
                    {
                        Console.Out.WriteLine("Deleting {0}", user.Properties["Email"].StringValue);
                        batchDelete.Add(TableOperation.Delete(user));
                    }

                    Console.Out.WriteLine("Going to delete inactive users");
                    subscriberTable.ExecuteBatch(batchDelete, TableRequestOptions);
                    token = segment.ContinuationToken;
                }
                while (token != null);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error at Time:{0} Exception:{1}", DateTime.UtcNow, exception);
                throw;
            }
        }

        #endregion
    }
}