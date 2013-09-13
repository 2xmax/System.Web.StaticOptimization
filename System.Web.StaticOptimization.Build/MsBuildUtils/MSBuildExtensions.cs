using System.Linq;
using System.Reflection;
using Microsoft.Build.Exceptions;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

namespace System.Web.StaticOptimization.MsBuildUtils
{
    public static class BuildEngineExtensions
    {
        private const BindingFlags SearchBindingFlags =
            BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public;

        public static string GetProjectDir(this IBuildEngine buildEngine)
        {
            return GetEnvironmentVariable(buildEngine, "MSBuildProjectDirectory");
        }

        private static string GetEnvironmentVariable(this IBuildEngine buildEngine, string key)
        {
            var projectInstance = GetProjectInstance(buildEngine);

            var items = projectInstance.Items
                .Where(x => string.Equals(x.ItemType, key, StringComparison.OrdinalIgnoreCase)).ToList();
            if (items.Count > 0)
            {
                return items.Select(x => x.EvaluatedInclude).First();
            }

            var properties = projectInstance.Properties
                .Where(x => string.Equals(x.Name, key, StringComparison.OrdinalIgnoreCase)).ToList();
            if (properties.Count > 0)
            {
                return properties.Select(x => x.EvaluatedValue).First();
            }

            return null;
        }

        private static ProjectInstance GetProjectInstance(IBuildEngine buildEngine)
        {
            var buildEngineType = buildEngine.GetType();
            var targetBuilderCallbackField = buildEngineType.GetField("targetBuilderCallback", SearchBindingFlags);
            if (targetBuilderCallbackField == null)
            {
                throw new InvalidProjectFileException("Could not extract targetBuilderCallback from " + buildEngineType.FullName);
            }
            var targetBuilderCallback = targetBuilderCallbackField.GetValue(buildEngine);
            var targetCallbackType = targetBuilderCallback.GetType();
            var projectInstanceField = targetCallbackType.GetField("projectInstance", SearchBindingFlags);
            if (projectInstanceField == null)
            {
                throw new InvalidProjectFileException("Could not extract projectInstance from " + targetCallbackType.FullName);
            }
            return (ProjectInstance)projectInstanceField.GetValue(targetBuilderCallback);
        }
    }
}