using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.StaticOptimization.Bundles;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using ZetaHtmlCompressor;

namespace System.Web.StaticOptimization
{
    public class HtmlMinifierTask : Task
    {
        [Required]
        public string InputDir { get; set; }

        public string Filemask { get; set; }

        public List<string> ExcludeList { get; set; }

        public override bool Execute()
        {
            if (string.IsNullOrEmpty(Filemask))
            {
                Filemask = "*.html";
            }
            ExcludeList = new List<string>
            {
                ".template.html"
            };
            var visitor = new DirectoryVisitor();
            visitor.Visit(InputDir, Filemask, new List<string>(), Minify);
            return true;
        }

        private void Minify(string file)
        {
            if (ExcludeList.Any(file.Contains))
            {
                return;
            }

            string input = File.ReadAllText(file);
            var min = new HtmlContentCompressor();
            string cmp = min.Compress(input);
            File.WriteAllText(file, cmp);
        }
    }
}