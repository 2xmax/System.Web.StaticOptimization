using System.Collections.Generic;

namespace System.Web.StaticOptimization
{
    /// <summary>
    /// Represents group of files that can be combined into root file
    /// (e.g. set of js or css files)
    /// </summary>
    public class Bundle
    {
        public Bundle(string root, IEnumerable<string> childs)
        {
            if (string.IsNullOrWhiteSpace(root))
            {
                throw new ArgumentNullException("root");
            }
            Root = root;
            Childs = new List<string>(childs);
        }

        public string Root { get; private set; }

        public IEnumerable<string> Childs { get; private set; }
    }
}