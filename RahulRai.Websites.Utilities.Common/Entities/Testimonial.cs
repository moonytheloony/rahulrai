// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 06-25-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-25-2015
// ***********************************************************************
// <copyright file="Testimonial.cs" company="Rahul Rai">
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
    using RegularTypes;

    #endregion

    /// <summary>
    ///     Class Testimonial.
    /// </summary>
    [Serializable]
    public class Testimonial
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public long TestimonialId { get; set; }

        /// <summary>
        ///     Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        [DisplayName("First Name")]
        [Required(ErrorMessage = "I need your First Name to create a badge for you!")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "First Name needs to be between 3 and 50 characters in length.")]
        [RegularExpression("([a-zA-Z]+)", ErrorMessage = "Only alphabets are allowed in First Name field.")]
        public string AuthorFirstName { get; set; }

        /// <summary>
        ///     Gets or sets the last name of the author.
        /// </summary>
        /// <value>The last name of the author.</value>
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "I need your Last Name to create a badge for you!")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "Last Name needs to be between 3 and 50 characters in length.")]
        [RegularExpression("([a-zA-Z]+)", ErrorMessage = "Only alphabets are allowed in Last Name field.")]
        public string AuthorLastName { get; set; }

        /// <summary>
        ///     Gets or sets the author email.
        /// </summary>
        /// <value>The author email.</value>
        [DisplayName("Your Email Address (not displayed)")]
        [Required(ErrorMessage = "I need your Email Address to stay in touch!")]
        [RegularExpression(KnownTypes.ValidEmailIdRegex, ErrorMessage = "Your email address is invalid.")]
        [StringLength(100, ErrorMessage = "Email address needs to be less than 100 characters in length.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Your email address is invalid.")]
        public string AuthorEmail { get; set; }

        /// <summary>
        ///     Gets or sets the author organization.
        /// </summary>
        /// <value>The author organization.</value>
        [DisplayName("You Work at")]
        [Required(ErrorMessage = "You haven't specified where you work at.")]
        [StringLength(100, ErrorMessage = "Your Organization Name needs to be less than 100 charaters in length.")]
        public string AuthorOrganization { get; set; }

        /// <summary>
        ///     Gets or sets the author designation.
        /// </summary>
        /// <value>
        ///     The author designation.
        /// </value>
        [DisplayName("Your Designation (leave empty if you don't want it to be displayed)")]
        [StringLength(100, ErrorMessage = "Your Designation needs to be less than 100 charaters in length.")]
        public string AuthorDesignation { get; set; }

        /// <summary>
        ///     Gets or sets the author comments.
        /// </summary>
        /// <value>The author comments.</value>
        [DisplayName("Testimonial (50-500 characters)")]
        [Required(ErrorMessage = "Please don't leave the Testimonial field empty :(")]
        [StringLength(500, MinimumLength = 50,
            ErrorMessage = "The Testimonial needs to be between 50 and 500 characters in length.")]
        [DataType(DataType.MultilineText)]
        public string AuthorComments { get; set; }

        /// <summary>
        ///     Gets or sets the author token.
        /// </summary>
        /// <value>The author token.</value>
        [DisplayName("Your Access Token (case insensitive)")]
        [StringLength(10, ErrorMessage = "Supported token length is 4 to 10 characters.", MinimumLength = 4)]
        [RegularExpression("([a-zA-Z]+)", ErrorMessage = "Only alphabets are allowed in Token field.")]
        [Required(ErrorMessage = "Please enter the token so that I can validate your request.")]
        public string AuthorToken { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is approved.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is approved; otherwise, <c>false</c>.
        /// </value>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is featured.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is featured; otherwise, <c>false</c>.
        /// </value>
        public bool IsFeatured { get; set; }
    }
}