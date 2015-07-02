// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-02-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-02-2015
// ***********************************************************************
// <copyright file="UserContinuationStack.cs" company="Rahul Rai">
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
    ///     Class UserContinuationStack.
    /// </summary>
    public static class UserContinuationStack
    {
        /// <summary>
        /// Gets or sets the continuation stack.
        /// </summary>
        /// <value>The continuation stack.</value>
        public static ContinuationStack ContinuationStack
        {
            get
            {
                if (HttpContext.Current.Session[ApplicationConstants.StackKey] != null)
                {
                    return HttpContext.Current.Session[ApplicationConstants.StackKey] as ContinuationStack;
                }

                var stack = new ContinuationStack();
                HttpContext.Current.Session[ApplicationConstants.StackKey] = stack;
                return HttpContext.Current.Session[ApplicationConstants.StackKey] as ContinuationStack;
            }

            set
            {
                HttpContext.Current.Session[ApplicationConstants.StackKey] = value;
            }
        }
    }
}