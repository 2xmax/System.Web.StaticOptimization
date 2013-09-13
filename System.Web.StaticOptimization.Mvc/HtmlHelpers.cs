using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace System.Web.StaticOptimization.Mvc
{
    /// <summary>
    /// Reusable html helpers for rendering script, link tags
    /// </summary>
    public static class HtmlHelpers
    {
        public static IHtmlString Script(this HtmlHelper helper, string path)
        {
            var paths = GetScripts(path).Select(GetVirtualPath).ToList();
#if DEBUG
            if (!paths.Any())
            {
                throw new BundleConfigurationException("No registered bundle found " + path);
            }
#endif
            var sb = new StringBuilder();
            foreach (var p in paths)
            {
                sb.AppendFormat("<script src=\"{0}\"></script>", p);
                sb.AppendLine();
            }
            return new MvcHtmlString(sb.ToString());
        }

        public static string Style(this HtmlHelper helper, string path)
        {
            var paths = GetStyles(path).Select(GetVirtualPath).ToList();
#if DEBUG
            if (!paths.Any())
            {
                throw new BundleConfigurationException("No registered bundle found" + path);
            }
#endif
            var sb = new StringBuilder();
            foreach (var p in paths)
            {
                sb.AppendFormat(" <link href=\"{0}\" rel=\"stylesheet\"/>", p);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static IEnumerable<string> GetScripts(string bundlePath)
        {
#if DEBUG
            if (BundleTable.Bundles == null)
            {
                throw new BundleConfigurationException("You need to init the collection first");
            }
#endif
            var lowered = bundlePath.ToLowerInvariant();
            var bundles = BundleTable.Bundles.Scripts.Where(p => p.Root.ToLowerInvariant() == lowered);
            return BundleTable.EnableOptimizations ? new[] { bundlePath } : bundles.SelectMany(p => p.Childs);
        }

        private static IEnumerable<string> GetStyles(string bundlePath)
        {
#if DEBUG
            if (BundleTable.Bundles == null)
            {
                throw new BundleConfigurationException("You need to init the collection first");
            }
#endif
            var lowered = bundlePath.ToLowerInvariant();
            var bundles = BundleTable.Bundles.Styles.Where(p => p.Root.ToLowerInvariant() == lowered);
            return BundleTable.EnableOptimizations ? new[] { bundlePath } : bundles.SelectMany(p => p.Childs);
        }

        private static string GetVirtualPath(string path)
        {
            return VirtualPathUtility.ToAbsolute(path);
        }
    }
}