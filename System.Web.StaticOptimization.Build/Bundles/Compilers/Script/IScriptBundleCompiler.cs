using System.Collections.Generic;

namespace System.Web.StaticOptimization.Bundles.Compilers.Script
{
    internal interface IScriptBundleCompiler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files">absolute paths to the files</param>
        /// <returns>compiled bundle content</returns>
        string Compile(IEnumerable<string> files);
    }
}