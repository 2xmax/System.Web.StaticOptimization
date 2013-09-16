using System.Text;

namespace System.Web.StaticOptimization.HtmlPreprocessing.Html.Processors
{
    internal sealed class ScriptTagLineProcessor : TagLineProcessorBase
    {
        private const string Tag = "script";

        public ScriptTagLineProcessor()
            : base(Tag)
        {
        }

        protected override string GetContent(string command)
        {
            var scripts = BundleContentResolver.GetScripts(command);
            var sb = new StringBuilder();
            foreach (string script in scripts)
            {
                string val = script.StartsWith("~") ? script.Substring(1) : script;
                sb.AppendFormat("<script src=\"{0}\"></script>", val);
            }
            return sb.ToString();
        }
    }
}