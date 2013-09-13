using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.StaticOptimization.HtmlPreprocessing;
using System.Web.StaticOptimization.HtmlPreprocessing.Html;
using System.Web.StaticOptimization.MsBuildUtils;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace System.Web.StaticOptimization
{
    /// <summary>
    /// Modifies the input html files by specific directives.
    /// Supported ones:
    /// 1. <!--script:filename.js--> - js bundle replacement. See <code>ScriptTagPreprocessor</code> for details.
    /// 2. <!--style:filename.css--> - the same as script, but for css. See <code>StyleTagPreprocessor</code>.
    /// 3. <!--template:filename.html--> inserts file content. See <code>TemplateTagPreprocessor</code>.
    /// </summary>
    public class HtmlPreprocessingTask : Task
    {
        private bool _isRelease = true;

        /// <summary>
        /// Whether generate release or debug bundles
        /// </summary>
        public bool IsRelease
        {
            get { return _isRelease; }
            set { _isRelease = value; }
        }

        /// <summary>
        /// Direct filename or pattern (like \{0}.template.html)
        /// </summary>
        [Required]
        public string InputFile { get; set; }

        public ITaskItem[] OutputFileNameReplaceRules { get; set; }

        /// <summary>
        /// Path to WebGreace config 
        /// http://webgrease.codeplex.com/wikipage?title=WebGrease%20configuration%20file
        /// </summary>
        [Required]
        public string BundleConfig { get; set; }

        /// <summary>
        /// Prefix for absolute paths calculation for bundles and template
        /// </summary>
        public string RootDir { get; set; }

        public override bool Execute()
        {
            if (string.IsNullOrEmpty(RootDir))
            {
                RootDir = BuildEngine.GetProjectDir();
            }
            HtmlPreprocessing.Configuration.Init(IsRelease, BundleConfig, RootDir);
            var html = new HtmlPreprocessor();

            var replaceRules = OutputFileNameReplaceRules.Select(p =>
                new Tuple<string, string>(p.ItemSpec, p.GetMetadata("ReplaceBy"))).ToList();

            if (!replaceRules.Any())
            {
                replaceRules = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>(".template.html", ".html")
                };
            }

            var subs = InputOuputFilesResolver.FindMatches(RootDir, InputFile, replaceRules);
            foreach (var pair in subs)
            {
                html.Process(new FileInfo(pair.Input), new FileInfo(pair.Output));
            }

            return true;
        }
    }
}