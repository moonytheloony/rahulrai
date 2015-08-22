// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 08-20-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-21-2015
// ***********************************************************************
// <copyright file="SendgridMailClient.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Mailer
{
    #region

    using System.Net;
    using System.Net.Mail;
    using System.Web;

    using SendGrid;

    #endregion

    /// <summary>
    /// Class SendgridMailClient.
    /// </summary>
    public class SendgridMailClient : IMailSystem
    {
        #region Fields

        /// <summary>
        /// The credentials
        /// </summary>
        private readonly NetworkCredential credentials;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SendgridMailClient"/> class.
        /// </summary>
        /// <param name="sendgridAccountName">Name of the sendgrid account.</param>
        /// <param name="sendgridAccountKey">The sendgrid account key.</param>
        public SendgridMailClient(string sendgridAccountName, string sendgridAccountKey)
        {
            this.credentials = new NetworkCredential(sendgridAccountName, sendgridAccountKey);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="receiverAddress">The receiver address.</param>
        /// <param name="senderAddress">The sender address.</param>
        /// <param name="senderName">Name of the sender.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public void SendEmail(
            string receiverAddress,
            string senderAddress,
            string senderName,
            string subject,
            string body)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(receiverAddress);
            myMessage.From = new MailAddress(senderAddress, senderName);
            myMessage.Subject = subject;
            myMessage.Text = body;
            myMessage.Html = body;
            var transportWeb = new Web(this.credentials);
            var task = transportWeb.DeliverAsync(myMessage);
            task.Wait();
        }

        #endregion
    }
}