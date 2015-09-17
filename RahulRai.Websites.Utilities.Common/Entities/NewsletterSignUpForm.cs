// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 08-19-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-19-2015
// ***********************************************************************
// <copyright file="NewsletterSignUpForm.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using RahulRai.Websites.Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class NewsletterSignUpForm.
    /// </summary>
    public class NewsletterSignUpForm
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [DisplayName("Your Email Address (never displayed)")]
        [Required(ErrorMessage = "I need your Email Address so that I can send you mail!")]
        [RegularExpression(KnownTypes.ValidEmailIdRegex, ErrorMessage = "Your email address is invalid.")]
        [StringLength(100, ErrorMessage = "Email address needs to be less than 100 characters in length.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Your email address is invalid.")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [DisplayName("First Name")]
        [Required(ErrorMessage = "I need your First Name to know who I am writing to!")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "First Name needs to be between 3 and 50 characters in length.")]
        [RegularExpression("([a-zA-Z]+)", ErrorMessage = "Only alphabets are allowed in First Name field.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is verified.
        /// </summary>
        /// <value><c>true</c> if this instance is verified; otherwise, <c>false</c>.</value>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Gets or sets the verification string.
        /// </summary>
        /// <value>The verification string.</value>
        public string VerificationString { get; set; }

        /// <summary>
        /// Gets or sets the unsubscribe string.
        /// </summary>
        /// <value>The unsubscribe string.</value>
        public string UnsubscribeString { get; set; }

        /// <summary>
        /// Gets or sets the last email identifier.
        /// </summary>
        /// <value>The last email identifier.</value>
        public string LastEmailIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate { get; set; }

        #endregion
    }
}