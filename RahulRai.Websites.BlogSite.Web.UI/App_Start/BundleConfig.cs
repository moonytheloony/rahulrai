// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 06-01-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="BundleConfig.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI
{
    #region

    using System.Web.Optimization;

    #endregion

    /// <summary>
    ///     Class BundleConfig.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        ///     Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/SyntaxHighlighter").Include(
                "~/Scripts/SyntaxHighlighter/shCore*",
                "~/Scripts/SyntaxHighlighter/shBrushCSharp*",
                "~/Scripts/SyntaxHighlighter/shBrushCss*",
                "~/Scripts/SyntaxHighlighter/shBrushJScript*",
                "~/Scripts/SyntaxHighlighter/shBrushXml*",
                "~/Scripts/SyntaxHighlighter/shBrushSql*"));

            bundles.Add(new StyleBundle("~/styles/SyntaxHighlighter").Include(
                "~/Content/SyntaxHighlighter/shCore.css",
                "~/Content/SyntaxHighlighter/shThemeDefault.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}