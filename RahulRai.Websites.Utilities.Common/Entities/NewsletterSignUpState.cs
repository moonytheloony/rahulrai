// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 08-19-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-19-2015
// ***********************************************************************
// <copyright file="NewsletterSignUpState.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    /// <summary>
    /// Enum NewsletterSignUpState
    /// </summary>
    public enum NewsletterSignUpState
    {
        /// <summary>
        /// The success
        /// </summary>
        Success = 0,

        /// <summary>
        /// The user exists
        /// </summary>
        UserExists = 1,

        /// <summary>
        /// The failure
        /// </summary>
        Failure = 2,

        /// <summary>
        /// The unsubscribed
        /// </summary>
        Unsubscribed = 3,

        /// <summary>
        /// The no input
        /// </summary>
        NoInput = 4,

        /// <summary>
        /// The user does not exist
        /// </summary>
        UserDoesNotExist = 5
    }
}