// ***********************************************************************
// Assembly         : RahulRai.Websites.Jobs.MailWorker
// Author           : rahulrai
// Created          : 08-20-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-22-2015
// ***********************************************************************
// <copyright file="SendNewPostMails.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Jobs.MailWorker
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Azure.WebJobs;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.Storage.Table.Queryable;

    using RahulRai.Websites.Utilities.Common.Helpers;
    using RahulRai.Websites.Utilities.Common.Mailer;
    using RahulRai.Websites.Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class SendNewPostMails.
    /// </summary>
    public class SendNewPostMails
    {
        #region Constants

        /// <summary>
        /// The blog table
        /// </summary>
        public const string BlogTable = "blogs";

        /// <summary>
        /// The new post queue
        /// </summary>
        public const string NewPostQueue = "newblogpost";

        /// <summary>
        /// The subscriber table
        /// </summary>
        public const string SubscriberTable = "newslettersubscriber";

        #endregion

        #region Static Fields

        /// <summary>
        /// The mailer address
        /// </summary>
        private static readonly string MailerAddress = ConfigurationManager.AppSettings[ApplicationConstants.MailerAddress];

        /// <summary>
        /// The mailer name
        /// </summary>
        private static readonly string MailerName = ConfigurationManager.AppSettings[ApplicationConstants.MailerName];

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

        /// <summary>
        /// The template string
        /// </summary>
        private static readonly string TemplateString = File.ReadAllText("NewPost.html");

        /// <summary>
        /// The mail system
        /// </summary>
        private static IMailSystem mailSystem;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Processes the queue message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="blogTable">The blog table.</param>
        /// <param name="subscriberTable">The subscriber table.</param>
        public static void ProcessNewPostQueueMessage(
            [QueueTrigger(NewPostQueue)] string message,
            [Table(BlogTable)] CloudTable blogTable,
            [Table(SubscriberTable)] CloudTable subscriberTable)
        {
            try
            {
                Console.Out.WriteLine("New post message captured {0}", message);
                var sendgridUserName = ConfigurationManager.AppSettings[ApplicationConstants.SendgridUserName];
                var sendgridPassword = ConfigurationManager.AppSettings[ApplicationConstants.SendgridPassword];
                mailSystem = new SendgridMailClient(sendgridUserName, sendgridPassword);
                var operation = TableOperation.Retrieve(ApplicationConstants.BlogKey, message);
                var result = blogTable.Execute(operation, TableRequestOptions).Result as DynamicTableEntity;
                if (null == result)
                {
                    Console.Error.WriteLine("Could not find record corresponding to RK {0}", message);
                    return;
                }

                var title = result.Properties["Title"].StringValue;
                var postedDate = result.Properties["PostedDate"].DateTime;
                var bodySnippet = Routines.GeneratePreview(result.Properties["AutoIndexedElement_0_Body"].StringValue);
                var formattedUri = result.Properties["FormattedUri"].StringValue;

                //// Run paged queries to get subscribers.
                Console.Out.WriteLine("Going to get list of subscribers");
                var query = (from record in subscriberTable.CreateQuery<DynamicTableEntity>()
                             where
                                 record.PartitionKey == ApplicationConstants.SubscriberListKey
                                     && record.Properties["LastEmailIdentifier"].StringValue != result.RowKey
                                     && record.Properties["LastEmailIdentifier"].StringValue != string.Format("Faulted {0}", DateTime.UtcNow.Date)
                                     && record.Properties["IsVerified"].BooleanValue == true
                             select record).Take(10).AsTableQuery();
                TableContinuationToken token = null;
                do
                {
                    var segment = subscriberTable.ExecuteQuerySegmented(query, token, TableRequestOptions);
                    var batchUpdate = new TableBatchOperation();
                    var userDetails = new List<Tuple<string, string, string>>();
                    if (null == segment || !segment.Any())
                    {
                        Console.Out.WriteLine("No users found. Aborting current call.");
                        return;
                    }

                    Console.Out.WriteLine("Going to send mails to users");
                    if (SendNewPostMailsToUsers(userDetails, title, postedDate, formattedUri, bodySnippet))
                    {
                        foreach (var record in segment)
                        {
                            record.Properties["LastEmailIdentifier"].StringValue = result.RowKey;
                            batchUpdate.Add(TableOperation.InsertOrReplace(record));
                            userDetails.Add(
                                new Tuple<string, string, string>(
                                    record.Properties["FirstName"].StringValue,
                                    record.RowKey,
                                    record.Properties["VerificationString"].StringValue));
                        }
                    }
                    else
                    {
                        foreach (var record in segment)
                        {
                            record.Properties["LastEmailIdentifier"].StringValue = string.Format("Faulted {0}", DateTime.UtcNow.Date);
                            batchUpdate.Add(TableOperation.InsertOrReplace(record));
                            userDetails.Add(
                                new Tuple<string, string, string>(
                                    record.Properties["FirstName"].StringValue,
                                    record.RowKey,
                                    record.Properties["VerificationString"].StringValue));
                        }
                    }

                    subscriberTable.ExecuteBatch(batchUpdate, TableRequestOptions);
                    token = segment.ContinuationToken;
                }
                while (token != null);
                Console.Out.WriteLine("Mails sent");
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error at Time:{0} Message:{1}", DateTime.UtcNow, exception);
                throw;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends the new post mails to users.
        /// </summary>
        /// <param name="userDetails">The user details.</param>
        /// <param name="title">The title.</param>
        /// <param name="postedDate">The posted date.</param>
        /// <param name="formattedUri">The formatted URI.</param>
        /// <param name="bodySnippet">The body snippet.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private static bool SendNewPostMailsToUsers(
            IEnumerable<Tuple<string, string, string>> userDetails,
            string title,
            DateTime? postedDate,
            string formattedUri,
            string bodySnippet)
        {
            try
            {
                var subject = string.Format("New blog on rahulrai.in: {0}", title);
                Parallel.ForEach(
                    userDetails,
                    userDetail =>
                    {
                        var mailBody =
                            TemplateString.Replace("[NAME]", userDetail.Item1)
                                .Replace("[BLOGTITLE]", title)
                                .Replace("[BODYSNIP]", bodySnippet)
                                .Replace("[POSTLINK]", formattedUri)
                                .Replace("[CODESTRING]", userDetail.Item3)
                                .Replace("[POSTDATE]", (postedDate ?? DateTime.MinValue).ToLocalTime().ToString("f"));
                        mailSystem.SendEmail(userDetail.Item2, MailerAddress, MailerName, subject, mailBody);
                    });
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error at Time:{0} Message:{1}", DateTime.UtcNow, exception);
                return false;
            }

            return true;
        }

        #endregion
    }
}