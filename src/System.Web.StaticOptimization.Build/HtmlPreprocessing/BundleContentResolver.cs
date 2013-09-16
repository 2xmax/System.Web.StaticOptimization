using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System.Web.StaticOptimization.HtmlPreprocessing
{
    internal static class BundleContentResolver
    {
        private static BundleCollection _bundleInstance;

        private static BundleCollection Bundles
        {
            get
            {
                return _bundleInstance ??
                       (_bundleInstance = WebGreaceConfigBundleLoader.Load(Configuration.Instance.ConfigPath));
            }
        }

        public static IEnumerable<string> GetScripts(string bundleName)
        {
            if (Configuration.Instance.IsRelease)
            {
                return Bundles.Scripts.Where(p => p.Root == bundleName).Select(p => p.Root).ToList();
            }

            return Bundles.Scripts.Where(p => p.Root == bundleName).SelectMany(p => p.Childs).ToList();
        }

        public static IEnumerable<string> GetStyles(string bundleName)
        {
            return Configuration.Instance.IsRelease ? 
                Bundles.Styles.Where(p => p.Root == bundleName).Select(p => p.Root).ToList() : 
                Bundles.Styles.Where(p => p.Root == bundleName).SelectMany(p => p.Childs).ToList();
        }

        public static string GetTemplate(string templatename)
        {
            return File.ReadAllText(ResolvePath(templatename));
        }

        private static string ResolvePath(string path)
        {
            if (new FileInfo(path).Exists)
            {
                return path;
            }

            string f = path.StartsWith(@"~/") ? path.Substring(2) : path;
            string comb = Path.Combine(Configuration.Instance.RootPath, f.Replace("/", "\\"));
            return comb;
        }
    }
}