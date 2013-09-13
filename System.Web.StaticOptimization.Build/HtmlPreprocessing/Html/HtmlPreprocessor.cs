using System.Collections.Generic;
using System.IO;
using System.Web.StaticOptimization.HtmlPreprocessing.Html.Processors;

namespace System.Web.StaticOptimization.HtmlPreprocessing.Html
{
    internal sealed class HtmlPreprocessor
    {
        private readonly IList<ILineProcessor> _processors;

        public HtmlPreprocessor()
        {
            _processors = new List<ILineProcessor>
            {
                new ScriptTagLineProcessor(),
                new StyleTagLineProcessor(),
                new TemplateTagLineProcessor()
            };
        }

        public void Process(FileInfo input, FileInfo output)
        {
            using (var inputReader = new StreamReader(File.OpenRead(input.FullName)))
            {
                using (var outputWriter = new StreamWriter(output.FullName))
                {
                    string tempLineValue;
                    while (null != (tempLineValue = inputReader.ReadLine()))
                    {
                        // ReSharper disable once LoopCanBeConvertedToQuery
                        foreach (var lineProcessor in _processors)
                        {
                            tempLineValue = lineProcessor.ProcessLine(tempLineValue);
                        }
                        outputWriter.WriteLine(tempLineValue);
                    }
                }
            }
        }
    }
}