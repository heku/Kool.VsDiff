namespace Kool.VsDiff.Models
{
    internal static class DiffToolFactory
    {
        private static VsDiffPackage Package;
        private static IDiffTool CachedDiffTool;

        public static void Initialize(VsDiffPackage package) => Package = package;

        public static void ClearCache() => CachedDiffTool = null;

        public static IDiffTool CreateDiffTool()
        {
            if (CachedDiffTool == null)
            {
                CachedDiffTool = Package.Options.UseCustomDiffTool
                    ? new CustomDiffTool(Package.Options.CustomDiffToolPath, Package.Options.CustomDiffToolArgs)
                    : (IDiffTool)new VsDiffTool(Package);
            }

            return CachedDiffTool;
        }
    }
}