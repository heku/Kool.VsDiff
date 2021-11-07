using static Kool.VsDiff.VsDiffPackage;

namespace Kool.VsDiff.Models
{
    internal static class DiffToolFactory
    {
        private static IDiffTool CachedDiffTool;

        public static IDiffTool CreateDiffTool() => CachedDiffTool ??= Options.UseCustomDiffTool ? new CustomDiffTool() : new VsDiffTool();

        public static void ClearCache() => CachedDiffTool = null;
    }
}