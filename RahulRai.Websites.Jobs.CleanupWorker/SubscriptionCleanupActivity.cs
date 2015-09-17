// ***********************************************************************
// Assembly         : RahulRai.Websites.Jobs.CleanupWorker
// Author           : rahulrai
// Created          : 08-22-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-31-2015
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
    using System.Configuration;
    using System.IO;
    using System.Linq;

    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.Storage.Table.Queryable;

    using RahulRai.Websites.Utilities.Common.Mailer;
    using RahulRai.Websites.Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    ///     Class SubscriptionCleanupActivity.
    /// </summary>
    public class SubscriptionCleanupActivity
    {
        #region Static Fields

        /// <summary>
        ///     The mailer address
        /// </summary>
        private static readonly string MailerAddress =
            ConfigurationManager.AppSettings[ApplicationConstants.MailerAddress];

        /// <summary>
        ///     The mailer name
        /// </summary>
        private static readonly string MailerName = ConfigurationManager.AppSettings[ApplicationConstants.MailerName];

        /// <summary>
        ///     The table request options
        /// </summary>
        private static readonly TableRequestOptions TableRequestOptions = new TableRequestOptions
            {
                RetryPolicy =
                    new ExponentialRetry(
                        TimeSpan.FromSeconds(CustomRetryPolicy.RetryBackOffSeconds),
                        CustomRetryPolicy.MaxRetries)
            };

        /// <summary>
        ///     The template string
        /// </summary>
        private static readonly string TemplateString = File.ReadAllText("NewUser.html");

        /// <summary>
        ///     The mail system.
        /// </summary>
        private static IMailSystem mailSystem;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Cleanups the old subscribers.
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
                    }
                    else
                    {
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
                }
                while (token != null);

                //// Send a friendly reminder everyday for the last 5 days before deleting a subscription.
                var sendgridUserName = ConfigurationManager.AppSettings[ApplicationConstants.SendgridUserName];
                var sendgridPassword = ConfigurationManager.AppSettings[ApplicationConstants.SendgridPassword];
                mailSystem = new SendgridMailClient(sendgridUserName, sendgridPassword);
                var reminderQuery = (from record in subscriberTable.CreateQuery<DynamicTableEntity>()
                                     where
                                         record.PartitionKey == ApplicationConstants.SubscriberListKey
                                             && record.Properties["CreatedDate"].DateTime
                                                 <= (DateTime.UtcNow - TimeSpan.FromDays(3))
                                             && record.Properties["IsVerified"].BooleanValue == false
                                     select record).Take(10).AsTableQuery();
                TableContinuationToken reminderContinuationToken = null;
                do
                {
                    var segment = subscriberTable.ExecuteQuerySegmented(
                        reminderQuery,
                        reminderContinuationToken,
                        TableRequestOptions);
                    if (null == segment || !segment.Any())
                    {
                        Console.Out.WriteLine("No user found for reminder. Aborting current call.");
                    }
                    else
                    {
                        //// Log users we are going to remind.
                        foreach (var user in segment)
                        {
                            Console.Out.WriteLine("Reminding {0}", user.Properties["Email"].StringValue);
                            var reminderStatus =
                                SendReminderMail(
                                    new Tuple<string, string, string, DateTime>(
                                        user.Properties["FirstName"].StringValue,
                                        user.Properties["VerificationString"].StringValue,
                                        user.Properties["Email"].StringValue,
                                // ReSharper disable once PossibleInvalidOperationException
                                        user.Properties["CreatedDate"].DateTime.Value));
                            Console.Out.WriteLine(
                                "Reminded {0} with state {1}",
                                user.Properties["Email"].StringValue,
                                reminderStatus);
                        }

                        Console.Out.WriteLine("All users reminded");
                        reminderContinuationToken = segment.ContinuationToken;
                    }
                }
                while (reminderContinuationToken != null);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error at Time:{0} Exception:{1}", DateTime.UtcNow, exception);
                throw;
            }
        }

        /// <summary>
        /// Changes user validation and unsubscribe codes.
        /// </summary>
        /// <param name="subscriberTable">The subscriber table.</param>
        public static void RefreshUserValidationStrings(CloudTable subscriberTable)
        {
            try
            {
                var query = (from record in subscriberTable.CreateQuery<DynamicTableEntity>()
                             where
                             record.PartitionKey == ApplicationConstants.SubscriberListKey
                             && record.Properties["IsVerified"].BooleanValue == true
                             select record).Take(100).AsTableQuery();
                TableContinuationToken token = null;
                do
                {
                    var segment = subscriberTable.ExecuteQuerySegmented(query, token, TableRequestOptions);
                    var batchUpdate = new TableBatchOperation();
                    if (null == segment || !segment.Any())
                    {
                        Console.Out.WriteLine("No users found. Aborting current call.");
                    }
                    else
                    {
                        //// Log users we are going to update.
                        foreach (var user in segment)
                        {
                            Console.Out.WriteLine("Updating {0}", user.Properties["Email"].StringValue);
                            user.Properties["VerificationString"].StringValue = Guid.NewGuid().ToString();
                            user.Properties["UnsubscribeString"].StringValue = Guid.NewGuid().ToString();
                            batchUpdate.Add(TableOperation.Replace(user));
                        }

                        Console.Out.WriteLine("Going to update user secrets");
                        subscriberTable.ExecuteBatch(batchUpdate, TableRequestOptions);
                        token = segment.ContinuationToken;
                    }
                }
                while (token != null);
                Console.Out.WriteLine("Secrets of all users updated.");
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error at Time:{0} Exception:{1}", DateTime.UtcNow, exception);
                throw;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Send reminder mail.
        /// </summary>
        /// <param name="userDetail">The user details.</param>
        /// <returns>State of operation.</returns>
        private static bool SendReminderMail(Tuple<string, string, string, DateTime> userDetail)
        {
            try
            {
                var subject = string.Format("[Reminder] Hi {0}! Please Activate Your Subscription!‏", userDetail.Item1);
                var mailBody =
                    TemplateString.Replace("[NAME]", userDetail.Item1)
                        .Replace("[CODESTRING]", userDetail.Item2)
                        .Replace(
                            "[DATEOFEXPIRY]",
                            (userDetail.Item4 + TimeSpan.FromDays(7)).ToLocalTime().ToString("f"));
                mailSystem.SendEmail(userDetail.Item3, MailerAddress, MailerName, subject, mailBody);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error at Time:{0} Exception:{1}", DateTime.UtcNow, exception);
                return false;
            }

            return true;
        }

        #endregion
    }
}