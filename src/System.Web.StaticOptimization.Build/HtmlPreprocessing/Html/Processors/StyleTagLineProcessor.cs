using System.Text;

namespace System.Web.StaticOptimization.HtmlPreprocessing.Html.Processors
{
    internal sealed class StyleTagLineProcessor : TagLineProcessorBase
    {
        private const string Tag = "style";

        public StyleTagLineProcessor()
            : base(Tag)
        {
        }

        protected override string GetContent(string command)
        {
            var scripts = TemplateContentResolver.Instance.GetStyles(command);
            var sb = new StringBuilder();
            foreach (string script in scripts)
            {
                string val = script.StartsWith("~") ? script.Substring(1) : script;
                sb.AppendFormat("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\">", val);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}