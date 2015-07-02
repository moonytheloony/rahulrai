// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-02-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-03-2015
// ***********************************************************************
// <copyright file="UserPageDictionary.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.GlobalAccess
{
    #region

    using System.Web;
    using Utilities.Common.Entities;
    using Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class UserPageDictionary.
    /// </summary>
    public static class UserPageDictionary
    {
        /// <summary>
        /// Gets or sets the page dictionary.
        /// </summary>
        /// <value>The page dictionary.</value>
        public static PageDictionary PageDictionary
        {
            get
            {
                if (HttpContext.Current.Session[ApplicationConstants.PageKey] != null)
                {
                    return HttpContext.Current.Session[ApplicationConstants.PageKey] as PageDictionary;
                }

                var pageDictionary = new PageDictionary();
                HttpContext.Current.Session[ApplicationConstants.PageKey] = pageDictionary;
                return HttpContext.Current.Session[ApplicationConstants.PageKey] as PageDictionary;
            }

            set
            {
                HttpContext.Current.Session[ApplicationConstants.PageKey] = value;
            }
        }
    }
}