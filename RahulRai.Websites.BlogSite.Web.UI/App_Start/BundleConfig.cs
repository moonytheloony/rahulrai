namespace RahulRai.Websites.BlogSite.Web.UI.App_Start
{
    #region

    using System.Web.Optimization;

    #endregion

    public class BundleConfig
    {
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