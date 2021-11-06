using static Kool.VsDiff.VsDiffPackage;

namespace Kool.VsDiff.Models
{
    internal static class DiffToolFactory
    {
        private static IDiffTool CachedDiffTool;

        public static IDiffTool CreateDiffTool()
        {
            if (CachedDiffTool == null)
            {
                CachedDiffTool = Options.UseCustomDiffTool
                    ? new CustomDiffTool(Options.CustomDiffToolPath, Options.CustomDiffToolArgs)
                    : new VsDiffTool(Instance);
            }

            return CachedDiffTool;
        }

        public static void ClearCache() => CachedDiffTool = null;
    }
}