// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 08-19-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-19-2015
// ***********************************************************************
// <copyright file="TableNewsletterEntity.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System;

    using Microsoft.WindowsAzure.Storage.Table;

    using RahulRai.Websites.Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class TableNewsletterEntity.
    /// </summary>
    public class TableNewsletterEntity : TableEntity
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TableNewsletterEntity"/> class.
        /// </summary>
        /// <param name="signUpForm">The sign up form.</param>
        public TableNewsletterEntity(NewsletterSignUpForm signUpForm)
        {
            this.FirstName = signUpForm.FirstName;
            this.Email = signUpForm.Email;
            this.IsVerified = signUpForm.IsVerified;
            this.CreatedDate = signUpForm.CreatedDate;
            this.EmailCount = signUpForm.EmailCount;
            this.VerificationString = signUpForm.VerificationString;
            this.RowKey = signUpForm.Email;
            this.PartitionKey = ApplicationConstants.SubscriberListKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableNewsletterEntity"/> class.
        /// </summary>
        public TableNewsletterEntity()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the entity tag.
        /// </summary>
        /// <value>The entity tag.</value>
        public string EntityTag { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the email count.
        /// </summary>
        /// <value>The email count.</value>
        public int EmailCount { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
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

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the newsletter sign up form.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>NewsletterSignUpForm.</returns>
        public NewsletterSignUpForm GetNewsletterSignUpForm(TableNewsletterEntity entity)
        {
            if (null == entity)
            {
                return null;
            }

            return new NewsletterSignUpForm
                {
                    Email = this.Email,
                    FirstName = this.FirstName,
                    IsVerified = this.IsVerified,
                    EmailCount = this.EmailCount,
                    VerificationString = this.VerificationString,
                    CreatedDate = this.CreatedDate
                };
        }

        #endregion
    }
}