namespace System.Web.StaticOptimization.HtmlPreprocessing.Html.Processors
{
    internal sealed class TemplateTagLineProcessor : TagLineProcessorBase
    {
        private const string Key = "template";

        public TemplateTagLineProcessor()
            : base(Key)
        {
        }

        protected override string GetContent(string command)
        {
            return BundleContentResolver.GetTemplate(command);
        }
    }
}