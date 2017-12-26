namespace Kool.VsDiff.Models
{
    internal static class DiffToolFactory
    {
        private static VsDiffPackage Package;
        private static IDiffTool DefaultDiffTool;

        public static void Initialize(VsDiffPackage package)
        {
            Package = package;
        }

        public static IDiffTool CreateDiffTool()
        {
            var options = Package.Options;
            if (options.UseCustomDiffTool)
            {
                return new CustomDiffTool(options.CustomDiffToolPath, options.CustomDiffToolArgs);
            }
            else
            {
                return DefaultDiffTool ?? (DefaultDiffTool = new VsDiffTool(Package));
            }
        }
    }
}