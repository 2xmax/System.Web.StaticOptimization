namespace System.Web.StaticOptimization.HtmlPreprocessing.Html.Processors
{
    internal interface ILineProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">input line</param>
        /// <returns>replacement line</returns>
        string ProcessLine(string input);
    }
}