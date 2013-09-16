namespace System.Web.StaticOptimization.HtmlPreprocessing
{
    internal sealed class Configuration
    {
        private static Configuration _instance;

        private Configuration(bool isRelease, string configPath, string rootPath)
        {
            IsRelease = isRelease;
            ConfigPath = configPath;
            RootPath = rootPath;
        }

        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new HtmlPreprocessorException("The preprocessor is not yet initialized.");
                }
                return _instance;
            }
            private set { _instance = value; }
        }

        public bool IsRelease { get; private set; }

        public string ConfigPath { get; private set; }

        public string RootPath { get; private set; }

        public static void Init(bool isRelease, string configPath, string rootPath)
        {
            Instance = new Configuration(isRelease, configPath, rootPath);
        }
    }
}