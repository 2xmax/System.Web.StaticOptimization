using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System.Web.StaticOptimization.HtmlPreprocessing
{
    internal class TemplateContentResolver
    {
        private readonly string _rootPath;

        private readonly IDictionary<string, IEnumerable<string>> _scripts;

        private readonly IDictionary<string, IEnumerable<string>> _styles;

        private TemplateContentResolver(string bindleConfig, string rootPath, bool enableOptmizations)
        {
            _rootPath = rootPath;
            var bundleInstance = WebGreaceConfigBundleLoader.Load(ResolvePath(rootPath, bindleConfig));
            _scripts = bundleInstance.Scripts.ToDictionary(k => k.Root, k => enableOptmizations ? new[] { k.Root } : k.Childs);
            _styles = bundleInstance.Styles.ToDictionary(k => k.Root, k => enableOptmizations ? new[] { k.Root } : k.Childs);
        }

        public static TemplateContentResolver Instance { get; private set; }

        public static void Init(string bindleConfig, string rootPath, bool enableOptmizations)
        {
            Instance = new TemplateContentResolver(bindleConfig, rootPath, enableOptmizations);
        }

        public IEnumerable<string> GetScripts(string bundleName)
        {
            if (!_scripts.ContainsKey(bundleName))
            {
                throw new BundleConfigurationException(bundleName + " bundle does not exists");
            }

            return _scripts[bundleName];
        }

        public IEnumerable<string> GetStyles(string bundleName)
        {
            if (!_styles.ContainsKey(bundleName))
            {
                throw new BundleConfigurationException(bundleName + " bundle does not exists");
            }

            return _styles[bundleName];
        }

        public string GetTemplate(string templatename)
        {
            return File.ReadAllText(ResolvePath(_rootPath, templatename));
        }

        private static string ResolvePath(string rootPath, string path)
        {
            if (new FileInfo(path).Exists)
            {
                return path;
            }

            string f = path.StartsWith(@"~/") ? path.Substring(2) : path;
            string comb = Path.Combine(rootPath, f.Replace("/", "\\"));
            return comb;
        }
    }
}