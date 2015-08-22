// ***********************************************************************
// Assembly         : RahulRai.Websites.Jobs.MailWorker
// Author           : rahulrai
// Created          : 08-20-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-20-2015
// ***********************************************************************
// <copyright file="Program.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Jobs.MailWorker
{
    #region

    using System.Configuration;

    using Microsoft.Azure.WebJobs;

    using RahulRai.Websites.Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class Program.
    /// </summary>
    internal class Program
    {
        #region Methods

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            var configuration = new JobHostConfiguration
                {
                    StorageConnectionString = ConfigurationManager.AppSettings[ApplicationConstants.StorageAccountConnectionString],
                    DashboardConnectionString = ConfigurationManager.AppSettings[ApplicationConstants.StorageAccountConnectionString]
                };
            var host = new JobHost(configuration);
            host.RunAndBlock();
        }

        #endregion
    }
}