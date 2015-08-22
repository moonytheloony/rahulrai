// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 08-18-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-18-2015
// ***********************************************************************
// <copyright file="IMailSystem.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Mailer
{
    /// <summary>
    /// Interface IMailSystem
    /// </summary>
    public interface IMailSystem
    {
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="receiverAddress">The receiver address.</param>
        /// <param name="senderAddress">The sender address.</param>
        /// <param name="senderName">Name of the sender.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        void SendEmail(string receiverAddress, string senderAddress, string senderName, string subject, string body);
    }
}