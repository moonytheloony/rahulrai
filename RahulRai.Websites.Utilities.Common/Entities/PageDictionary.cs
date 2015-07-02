// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 07-03-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-03-2015
// ***********************************************************************
// <copyright file="PageDictionary.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure.Storage.Table;

    #endregion

    /// <summary>
    ///     Class PageDictionary.
    /// </summary>
    public class PageDictionary
    {
        /// <summary>
        ///     The pages
        /// </summary>
        private readonly List<Page> pages;

        /// <summary>
        ///     The current page
        /// </summary>
        private int currentPage;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PageDictionary" /> class.
        /// </summary>
        public PageDictionary()
        {
            this.pages = new List<Page> { new Page { PageNumber = 0, ContinuationToken = null } };
            this.currentPage = 0;
        }

        /// <summary>
        /// Gets the maximum page number.
        /// </summary>
        /// <value>The maximum page number.</value>
        public int MaximumPageNumber
        {
            get
            {
                return this.pages.Max(page => page.PageNumber);
            }
        }

        /// <summary>
        ///     Determines whether this instance [can move back].
        /// </summary>
        /// <returns><c>true</c> if this instance [can move back]; otherwise, <c>false</c>.</returns>
        public bool CanMoveBack()
        {
            return this.pages.Any(page => page.PageNumber < this.currentPage);
        }

        /// <summary>
        ///     Determines whether this instance [can move forward].
        /// </summary>
        /// <returns><c>true</c> if this instance [can move forward]; otherwise, <c>false</c>.</returns>
        public bool CanMoveForward()
        {
            return this.pages.Any(page => page.PageNumber > this.currentPage);
        }

        /// <summary>
        ///     Adds the page.
        /// </summary>
        /// <param name="continuationToken">The continuation token.</param>
        public void AddPage(TableContinuationToken continuationToken)
        {
            var maxPageNumber = this.pages.Max(page => page.PageNumber);
            this.pages.Add(new Page { PageNumber = ++maxPageNumber, ContinuationToken = continuationToken });
        }

        /// <summary>
        ///     Gets the page continuation token.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>TableContinuationToken.</returns>
        public TableContinuationToken GetPageContinuationToken(int pageNumber)
        {
            this.currentPage = pageNumber;
            var resultPage = this.pages.FirstOrDefault(page => page.PageNumber == this.currentPage);
            if (resultPage != null)
            {
                return resultPage.ContinuationToken;
            }

            //// Keep page number and token in sync
            this.currentPage = 0;
            return null;
        }
    }
}