// ***********************************************************************
// Assembly         : RahulRai.Websites.Jobs.MailWorker
// Author           : rahulrai
// Created          : 08-20-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-22-2015
// ***********************************************************************
// <copyright file="SendNewSubscriberMails.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Jobs.MailWorker
{
    #region

    using System;
    using System.Configuration;
    using System.IO;

    using Microsoft.Azure.WebJobs;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Table;

    using RahulRai.Websites.Utilities.Common.Mailer;
    using RahulRai.Websites.Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class SendNewSubscriberMails.
    /// </summary>
    public class SendNewSubscriberMails
    {
        #region Constants

        /// <summary>
        /// The new subscriber queue
        /// </summary>
        public const string NewSubscriberQueue = "newsubscriber";

        /// <summary>
        /// The subscriber table
        /// </summary>
        private const string SubscriberTable = "newslettersubscriber";

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
        private static readonly string TemplateString = File.ReadAllText("NewUser.html");

        /// <summary>
        /// Unsubscribe template string
        /// </summary>
        private static readonly string UnsubscribeTemplateString = File.ReadAllText("Unsubscribe.html");

        /// <summary>
        /// The mail system
        /// </summary>
        private static IMailSystem mailSystem;

        /// <summary>
        /// Locking object
        /// </summary>
        private static readonly object LockingObject = new object();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Processes the subscriber queue message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="subscriberTable">The subscriber table.</param>
        public static void ProcessSubscriberQueueMessage(
            [QueueTrigger(NewSubscriberQueue)] string message,
            [Table(SubscriberTable)] CloudTable subscriberTable)
        {
            try
            {
                lock (LockingObject)
                {
                    var isUnsubsribeMessage = false;
                    Console.Out.WriteLine("New subscriber message captured {0}", message);
                    var sendgridUserName = ConfigurationManager.AppSettings[ApplicationConstants.SendgridUserName];
                    var sendgridPassword = ConfigurationManager.AppSettings[ApplicationConstants.SendgridPassword];
                    mailSystem = new SendgridMailClient(sendgridUserName, sendgridPassword);
                    if (message.StartsWith("unsubscribe:", StringComparison.OrdinalIgnoreCase))
                    {
                        message = message.Split(':')[1];
                        isUnsubsribeMessage = true;
                    }

                    var operation = TableOperation.Retrieve(ApplicationConstants.SubscriberListKey, message);
                    var result = subscriberTable.Execute(operation, TableRequestOptions).Result as DynamicTableEntity;
                    if (null == result)
                    {
                        Console.Error.WriteLine("There was no record found for {0}", message);
                        return;
                    }

                    Console.Out.WriteLine("Going to send mail to {0}", message);
                    if (isUnsubsribeMessage)
                    {
                        // ReSharper disable once PossibleInvalidOperationException
                        SendUnsubscribeMailToUser(
                            new Tuple<string, string, string, DateTime>(
                                result.Properties["FirstName"].StringValue,
                                result.Properties["UnsubscribeString"].StringValue,
                                message,
                                result.Properties["CreatedDate"].DateTime.Value));
                    }
                    else
                    {
                        // ReSharper disable once PossibleInvalidOperationException
                        SendNewSubscriberMailToUser(
                            new Tuple<string, string, string, DateTime>(
                                result.Properties["FirstName"].StringValue,
                                result.Properties["VerificationString"].StringValue,
                                message,
                                result.Properties["CreatedDate"].DateTime.Value));
                    }

                    Console.Out.WriteLine("Mail sent to {0}", message);
                }
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error at Time:{0} Exception:{1}", DateTime.UtcNow, exception);
                throw;
            }
        }

        /// <summary>
        /// Send unsubscribe mail to user.
        /// </summary>
        /// <param name="userDetail">User details.</param>
        private static void SendUnsubscribeMailToUser(Tuple<string, string, string, DateTime> userDetail)
        {
            var subject = string.Format("Hi {0}! Would you like to unsubscribe?‏", userDetail.Item1);
            var mailBody = UnsubscribeTemplateString.Replace("[NAME]", userDetail.Item1).Replace("[CODESTRING]", userDetail.Item2);
            mailSystem.SendEmail(userDetail.Item3, MailerAddress, MailerName, subject, mailBody);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends the new subscriber mail to user.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        private static void SendNewSubscriberMailToUser(Tuple<string, string, string, DateTime> userDetail)
        {
            var subject = string.Format("Hi {0}! Welcome to rahulrai.in‏", userDetail.Item1);
            var mailBody =
                TemplateString.Replace("[NAME]", userDetail.Item1)
                    .Replace("[CODESTRING]", userDetail.Item2)
                    .Replace("[DATEOFEXPIRY]", (userDetail.Item4 + TimeSpan.FromDays(7)).ToLocalTime().ToString("f"));
            mailSystem.SendEmail(userDetail.Item3, MailerAddress, MailerName, subject, mailBody);
        }

        #endregion
    }
}