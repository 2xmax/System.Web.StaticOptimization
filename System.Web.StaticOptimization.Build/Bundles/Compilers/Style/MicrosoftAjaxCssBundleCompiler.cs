using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Ajax.Utilities;

namespace System.Web.StaticOptimization.Bundles.Compilers.Style
{
    internal sealed class MicrosoftAjaxCssBundleCompiler : IStyleBundleCompiler
    {
        public string Compile(IEnumerable<string> files)
        {
            var sb = new StringBuilder();
            foreach (string file in files)
            {
                string compressed = new CssParser().Parse(File.ReadAllText(file));
                sb.Append(compressed);
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}