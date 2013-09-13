using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System.Web.StaticOptimization.Bundles
{
    internal sealed class DirectoryVisitor
    {
        public void Visit(string dir, string pattern, List<string> ignoreFolders, Action<string> action)
        {
            foreach (string file in Directory.GetFiles(dir, pattern))
            {
                action(file);
            }

            foreach (string subDir in Directory.GetDirectories(dir))
            {
                if (!ignoreFolders.Any(p => subDir.Contains(p)))
                {
                    Visit(subDir, pattern, ignoreFolders, action);
                }
            }
        }
    }
}