namespace System.Web.StaticOptimization
{
    [Serializable]
    public class BundleConfigurationException : Exception
    {
        public BundleConfigurationException(string message) : base(message)
        {
        }

        public BundleConfigurationException(string message, Exception cause) : base(message, cause)
        {
        }
    }
}