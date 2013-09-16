using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.Web.StaticOptimization.HtmlPreprocessing
{
    internal sealed class InputOuputFilesResolver
    {
        public static IEnumerable<InputOutputFilePair> FindMatches(
            string rootDir,
            string inputPattern,
            List<Tuple<string, string>> replaceRules)
        {
            string pattern = Path.GetFileName(inputPattern);
            if (pattern == null)
            {
                throw new ArgumentException("Can't recognize  filename in pattern", "inputPattern");
            }
            string relDir = inputPattern.Substring(0, inputPattern.Length - pattern.Length);

            string absPath;
            string[] files;
            if (relDir.Contains(rootDir))
            {
                absPath = relDir;
                files = Directory.GetFiles(absPath, pattern, SearchOption.TopDirectoryOnly);
            }
            else
            {
                absPath = Path.GetFullPath(Path.Combine(rootDir, relDir));
                files = Directory.GetFiles(absPath, pattern, SearchOption.AllDirectories);
            }

            return files.Select(p => new InputOutputFilePair(p, ResolveOutput(p, replaceRules)));
        }

        private static string ResolveOutput(string input, IEnumerable<Tuple<string, string>> substitutions)
        {
            string file = Path.GetFileName(input);
            string dir = Path.GetDirectoryName(input);
            file = substitutions.Aggregate(file, (current, subs) => Regex.Replace(current, subs.Item1, subs.Item2));
            // ReSharper disable once AssignNullToNotNullAttribute
            return Path.Combine(dir, file);
        }
    }

    internal sealed class InputOutputFilePair
    {
        public InputOutputFilePair(string input, string output)
        {
            Input = input;
            Output = output;
        }

        public string Input { get; private set; }

        public string Output { get; private set; }

        public override string ToString()
        {
            return Path.GetFileName(Input) + " " + Path.GetFileName(Output);
        }
    }
}