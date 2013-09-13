namespace System.Web.StaticOptimization.Mvc
{
    /// <summary>
    /// Holder for Bundle collection
    /// </summary>
    public static class BundleTable
    {
        private static bool _enableOptimizations = true;
        private static BundleCollection _bundles;

        public static BundleCollection Bundles
        {
            get
            {
                return _bundles;
            }
        }

        /// <summary>
        /// Whether need to use minified bundles or full debug version
        /// Hint: use #if DEBUG preprocessor directive
        /// </summary>
        public static bool EnableOptimizations
        {
            get { return _enableOptimizations; }
            private set { _enableOptimizations = value; }
        }

        public static void Init(BundleCollection bundles)
        {
            if (bundles == null)
            {
                throw new ArgumentNullException("bundles");
            }
            _bundles = bundles;
            EnableOptimizations = false;
        }

        public static void Init(string configFile)
        {
            Init(WebGreaceConfigBundleLoader.Load(configFile));
            EnableOptimizations = false;
        }
    }
}