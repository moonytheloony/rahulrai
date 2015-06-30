// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 04-16-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="CustomRetryPolicy.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.RegularTypes
{
    /// <summary>
    ///     The retry policy.
    /// </summary>
    public static class CustomRetryPolicy
    {
        #region Constants

        /// <summary>
        ///     The max retries.
        /// </summary>
        public const int MaxRetries = 10;

        /// <summary>
        ///     The retry back off.
        /// </summary>
        public const int RetryBackOffSeconds = 2;

        #endregion
    }
}