using System.Collections.Generic;

namespace System.Web.StaticOptimization
{
    /// <summary>
    /// Represents set of bundles divided on multiple logical groups
    /// </summary>
    public class BundleCollection
    {
        public BundleCollection(IEnumerable<Bundle> scripts, IEnumerable<Bundle> styles)
        {
            Scripts = new List<Bundle>(scripts);
            Styles = new List<Bundle>(styles);
        }

        public IEnumerable<Bundle> Scripts { get; private set; }

        public IEnumerable<Bundle> Styles { get; private set; }
    }
}