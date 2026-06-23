using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SpanTools.Tasks
{
    public sealed class ResolvePackageVersionTask : Task
    {
        [Required]
        public string ProjectAssetsFile { get; set; } = string.Empty;

        [Required]
        public string TargetFramework { get; set; } = string.Empty;

        [Required]
        public string TargetFrameworkMoniker { get; set; } = string.Empty;

        [Required]
        public string PackageId { get; set; } = string.Empty;

        [Output]
        public string PackageVersion { get; set; } = string.Empty;

        public override bool Execute()
        {
            Log.LogMessage(
                MessageImportance.High,
                $"Looking for package '{PackageId}','{TargetFramework}','{TargetFrameworkMoniker}'");

            PackageVersion =
                ProjectAssetsParser.FindPackageVersion(
                    ProjectAssetsFile,
                    TargetFramework,
                    TargetFrameworkMoniker,
                    PackageId)
                ?? string.Empty;

            Log.LogMessage(
                MessageImportance.High,
                $"Package '{PackageId}','{TargetFramework}','{TargetFrameworkMoniker}' resulted in version '{PackageVersion}'");

            return true;
        }
    }
}
