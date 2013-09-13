using System.Text.RegularExpressions;

namespace System.Web.StaticOptimization.HtmlPreprocessing.Html.Processors
{
    internal abstract class TagLineProcessorBase : ILineProcessor
    {
        private readonly Regex _expression;
        private readonly string _tag;

        protected TagLineProcessorBase(string tag)
        {
            _tag = tag;
            _expression = new Regex("<!--" + tag + ":([^>]*)-->");
        }

        public string ProcessLine(string input)
        {
            return _expression.Replace(input, GetMatchContent);
        }

        protected abstract string GetContent(string command);

        private string GetMatchContent(Match match)
        {
            string str = match.Groups[0].ToString().Trim();
            string origin = "<!--" + _tag + ":";
            if (str.StartsWith(origin))
            {
                str = str.Substring(origin.Length);
            }

            const string End = "-->";
            if (str.EndsWith(End))
            {
                str = str.Replace(End, string.Empty);
            }

            return GetContent(str);
        }
    }
}