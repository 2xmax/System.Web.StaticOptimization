using System.IO;
using System.Linq;
using System.Web.StaticOptimization.Bundles.Compilers.Script;
using System.Web.StaticOptimization.Bundles.Compilers.Style;
using System.Web.StaticOptimization.MsBuildUtils;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace System.Web.StaticOptimization
{
    /// <summary>
    /// Generates bundle biles by WebGreace config 
    /// http://webgrease.codeplex.com/wikipage?title=WebGrease%20configuration%20file
    /// </summary>
    public class BundleGeneratorTask : Task
    {
        /// <summary>
        /// Path to WebGreace config 
        /// </summary>
        [Required]
        public string BundleConfig { get; set; }

        /// <summary>
        /// Prefix for absolute paths calculation for bundles
        /// </summary>
        public string RootDir { get; set; }

        public override bool Execute()
        {
            if (string.IsNullOrEmpty(RootDir))
            {
                RootDir = BuildEngine.GetProjectDir();
            }
            var bundles = WebGreaceConfigBundleLoader.Load(GetAbsPath(RootDir, BundleConfig));

            IScriptBundleCompiler scriptCompiler = new MicrosoftAjaxScriptBundleCompiler();
            foreach (var bundle in bundles.Scripts)
            {
                var filesToMin = bundle.Childs.Select(p => GetAbsPath(RootDir, p));
                string compiles = scriptCompiler.Compile(filesToMin);
                string fileToWtite = GetAbsPath(RootDir, bundle.Root);
                File.WriteAllText(fileToWtite, compiles);
            }

            IStyleBundleCompiler styleCompiler = new MicrosoftAjaxCssBundleCompiler();

            foreach (var bundle in bundles.Styles)
            {
                var filesToMin = bundle.Childs.Select(p => GetAbsPath(RootDir, p));
                string compiles = styleCompiler.Compile(filesToMin);
                string fileToWtite = GetAbsPath(RootDir, bundle.Root);
                File.WriteAllText(fileToWtite, compiles);
            }
            return true;
        }

        private static string GetAbsPath(string rootDir, string relativePath)
        {
            if (File.Exists(relativePath))
            {
                return relativePath;
            }
            string rel = relativePath;
            if (rel.StartsWith("~/"))
            {
                rel = rel.Substring(2);
            }
            rel = rel.Replace(@"/", @"\");
            string ret = Path.Combine(rootDir, rel);
            return ret;
        }
    }
}