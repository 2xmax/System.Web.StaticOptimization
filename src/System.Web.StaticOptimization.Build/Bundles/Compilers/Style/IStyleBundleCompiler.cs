using System.Collections.Generic;

namespace System.Web.StaticOptimization.Bundles.Compilers.Style
{
    internal interface IStyleBundleCompiler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files">absolute paths to the files</param>
        /// <returns>compiled bundle content</returns>
        string Compile(IEnumerable<string> files);
    }
}