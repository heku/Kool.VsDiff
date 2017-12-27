using System.Reflection;
using System.Resources;

namespace Kool.VsDiff.Models
{
    internal static class Resources
    {
        private const string PACKAGE_RESX_FILE_NAME = "VSPackage";
        private static readonly ResourceManager Resx = new ResourceManager(PACKAGE_RESX_FILE_NAME, Assembly.GetExecutingAssembly());

        public static string OptionsPage_UseCustomDiffTool { get; } = Resx.GetString(nameof(OptionsPage_UseCustomDiffTool));

        public static string OptionsPage_DiffToolPath { get; } = Resx.GetString(nameof(OptionsPage_DiffToolPath));

        public static string OptionsPage_Arguments { get; } = Resx.GetString(nameof(OptionsPage_Arguments));

        public static string OptionsPage_EnableDiffClipboardWithCode { get; } = Resx.GetString(nameof(OptionsPage_EnableDiffClipboardWithCode));

        public static string OptionsPage_EnableDiffClipboardWithFile { get; } = Resx.GetString(nameof(OptionsPage_EnableDiffClipboardWithFile));

        public static string OptionsPage_EnableDiagnosticsMode { get; } = Resx.GetString(nameof(OptionsPage_EnableDiagnosticsMode));

        public static string OptionsPage_TestButtonContent { get; } = Resx.GetString(nameof(OptionsPage_TestButtonContent));

        public static string OptionsPage_ErrorMessageTitle { get; } = Resx.GetString(nameof(OptionsPage_ErrorMessageTitle));
    }
}