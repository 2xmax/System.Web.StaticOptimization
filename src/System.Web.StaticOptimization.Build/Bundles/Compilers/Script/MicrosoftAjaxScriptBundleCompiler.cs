using System.Collections.Generic;
using System.IO;
using Microsoft.Ajax.Utilities;

namespace System.Web.StaticOptimization.Bundles.Compilers.Script
{
    internal sealed class MicrosoftAjaxScriptBundleCompiler : IScriptBundleCompiler
    {
        public string Compile(IEnumerable<string> files)
        {
            var blocks = new List<Block>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (string file in files)
            {
                var parser = new JSParser(File.ReadAllText(file)) { FileContext = file };
                var block = parser.Parse(new CodeSettings
                {
                    EvalTreatment = EvalTreatment.MakeImmediateSafe,
                    PreserveImportantComments = false
                });
                if (block != null)
                {
                    blocks.Add(block);
                }
            }

            Block fst = blocks[0];
            for (int i = 1; i < blocks.Count; i++)
            {
                fst.Append(blocks[i]);
            }

            string sequenceCode = fst.ToCode();
            var minifier = new Minifier();
            string compiled = minifier.MinifyJavaScript(
                sequenceCode,
                new CodeSettings
                {
                    EvalTreatment = EvalTreatment.MakeImmediateSafe,
                    PreserveImportantComments = false
                });
            return compiled;
        }
    }
}