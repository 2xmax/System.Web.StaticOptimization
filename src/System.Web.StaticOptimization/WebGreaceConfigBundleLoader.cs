using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace System.Web.StaticOptimization
{
    /// <summary>
    ///     Documentation about the config file is available at
    ///     http://webgrease.codeplex.com/wikipage?title=WebGrease%20configuration%20file
    /// </summary>
    public static class WebGreaceConfigBundleLoader
    {
        public static BundleCollection Load(string configFile)
        {
            XDocument doc = XDocument.Load(configFile);
            return new BundleCollection(ParseFileSet(doc, "JsFileSet"), ParseFileSet(doc, "CssFileSet"));
        }

        private static IEnumerable<Bundle> ParseFileSet(XDocument doc, string elementName)
        {
            var bundleFileSets = doc.Descendants()
                .Where(p => string.Equals(p.Name.LocalName, elementName, StringComparison.OrdinalIgnoreCase))
                .ToList();
            var ret = new List<Bundle>();
            foreach (XElement bundleFileSet in bundleFileSets)
            {
                XAttribute name = bundleFileSet
                    .Attributes()
                    .FirstOrDefault(
                        p => string.Equals(p.Name.LocalName, "name", StringComparison.OrdinalIgnoreCase));

                if (name != null)
                {
                    var childs = bundleFileSet
                        .Descendants()
                        .Where(
                            p => string.Equals(p.Name.LocalName, "Input", StringComparison.OrdinalIgnoreCase));

                    var bundleData = new Bundle(name.Value, childs.Select(p => p.Value).ToList());
                    ret.Add(bundleData);
                }
                else
                {
                    throw new BundleConfigurationException("Input[name] attribute is required");
                }
            }

            return ret;
        }
    }
}