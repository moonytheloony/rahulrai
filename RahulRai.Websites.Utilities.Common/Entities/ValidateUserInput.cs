// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 08-12-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-12-2015
// ***********************************************************************
// <copyright file="ValidateUserInput.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    /// <summary>
    /// Class ValidateUserInput.
    /// </summary>
    public class ValidateUserInput
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the input text.
        /// </summary>
        /// <value>The input text.</value>
        public string InputText { get; set; }

        /// <summary>
        /// Gets or sets the validation string.
        /// </summary>
        /// <value>The validation string.</value>
        public string ValidationString { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the input identifier.
        /// </summary>
        /// <value>The input identifier.</value>
        public string InputIdentifier { get; set; }

        #endregion
    }
}