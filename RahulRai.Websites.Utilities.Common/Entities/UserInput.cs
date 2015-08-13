// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 08-11-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-13-2015
// ***********************************************************************
// <copyright file="UserInput.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Enum UserInputType
    /// </summary>
    public enum UserInputType
    {
        /// <summary>
        /// The vote
        /// </summary>
        VoteSingleOption,

        /// <summary>
        /// The vote multiple option
        /// </summary>
        VoteMultipleOptions,

        /// <summary>
        /// The text
        /// </summary>
        Text
    }

    /// <summary>
    /// Class UserInput.
    /// </summary>
    public class UserInput
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        /// <value>The name of the document.</value>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [accept attachment].
        /// </summary>
        /// <value><c>true</c> if [accept attachment]; otherwise, <c>false</c>.</value>
        public bool AcceptAttachment { get; set; }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>The input.</value>
        public List<ValidateUserInput> Input { get; set; }

        /// <summary>
        /// Gets or sets the survey title.
        /// </summary>
        /// <value>The survey title.</value>
        public string SurveyTitle { get; set; }

        /// <summary>
        /// Gets or sets the survey guideline.
        /// </summary>
        /// <value>The survey guideline.</value>
        public string SurveyGuideline { get; set; }

        /// <summary>
        /// Gets or sets the type of the user input.
        /// </summary>
        /// <value>The type of the user input.</value>
        public UserInputType UserInputType { get; set; }

        #endregion
    }
}