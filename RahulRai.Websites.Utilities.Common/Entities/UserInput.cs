// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 08-11-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-11-2015
// ***********************************************************************
// <copyright file="UserInput.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System;

    #endregion

    /// <summary>
    /// Class UserInput.
    /// </summary>
    public class UserInput
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the json URI.
        /// </summary>
        /// <value>The json URI.</value>
        public Uri JsonUri { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        #endregion
    }
}