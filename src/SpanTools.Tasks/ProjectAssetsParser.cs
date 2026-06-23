using System;
using System.IO;

namespace SpanTools.Tasks
{
    internal static class ProjectAssetsParser
    {
        private enum ParserState
        {
            LookingForTargets,
            LookingForTargetFramework,
            LookingForPackage
        }

        public static string? FindPackageVersion(
            string assetsFile,
            string targetFramework,
            string targetFrameworkMoniker,
            string packageId)
        {
            using var stream =
                File.OpenRead(assetsFile);

            using var reader =
                new JsonPropertyReader(stream);

            ParserState state =
                ParserState.LookingForTargets;

            int targetsDepth = -1;
            int tfmDepth = -1;

            string? propertyName;

            string packageIdPrefix = string.Concat(packageId, "/");

            while ((propertyName = reader.ReadPropertyName()) != null)
            {
                int propertyDepth = reader.CurrentDepth;

                switch (state)
                {
                    case ParserState.LookingForTargets:

                        if (propertyName == "targets")
                        {
                            int depth = propertyDepth;

                            reader.MoveToPropertyValue();

                            if (!reader.ExpectStartObject())
                                return null;

                            targetsDepth = depth;
                            state = ParserState.LookingForTargetFramework;
                        }

                        break;

                    case ParserState.LookingForTargetFramework:

                        if (propertyDepth <= targetsDepth)
                        {
                            state = ParserState.LookingForTargets;
                            break;
                        }

                        if (propertyDepth == targetsDepth + 1)
                        {
                            if (StringComparer.OrdinalIgnoreCase.Equals(propertyName, targetFramework) ||
                                StringComparer.OrdinalIgnoreCase.Equals(propertyName, targetFrameworkMoniker))
                            {
                                int depth = propertyDepth;

                                reader.MoveToPropertyValue();

                                if (!reader.ExpectStartObject())
                                    return null;

                                tfmDepth = depth;
                                state = ParserState.LookingForPackage;
                            }
                        }

                        break;

                    case ParserState.LookingForPackage:

                        if (propertyDepth <= tfmDepth)
                        {
                            state = ParserState.LookingForTargetFramework;
                            break;
                        }

                        if (propertyDepth == tfmDepth + 1)
                        {
                            if (propertyName.StartsWith(packageIdPrefix, StringComparison.OrdinalIgnoreCase))
                            {
                                return propertyName.Substring(packageIdPrefix.Length);
                            }
                        }

                        break;
                }
            }

            return null;
        }
    }
}
