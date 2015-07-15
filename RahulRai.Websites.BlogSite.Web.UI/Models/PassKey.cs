// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-07-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="PassKey.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.Models
{
    #region

    using System.ComponentModel.DataAnnotations;

    #endregion

    /// <summary>
    /// Class PassKey.
    /// </summary>
    public class PassKey
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [Required(ErrorMessage = "Please Enter Code.")]
        [StringLength(10, ErrorMessage = "Supported key length is 4 to 10 characters.", MinimumLength = 4)]
        [RegularExpression("^([a-zA-Z]+)$", ErrorMessage = "Only alphabets are allowed.")]
        [Display(Name = "Your Pass Key")]
        public string Key { get; set; }
    }
}