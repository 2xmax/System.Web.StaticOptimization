﻿namespace System.Web.StaticOptimization.Bundles
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