namespace System.Web.StaticOptimization.HtmlPreprocessing
{
    [Serializable]
    public class HtmlPreprocessorException : Exception
    {
        public HtmlPreprocessorException(string message) : base(message)
        {
        }

        public HtmlPreprocessorException(string message, Exception cause) : base(message, cause)
        {
        }
    }
}