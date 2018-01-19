using Kool.VsDiff.Models;
using System.IO;
using static Kool.VsDiff.Models.VS;

namespace Kool.VsDiff.Commands
{
    internal sealed class DiffClipboardWithFileCommand : BaseCommand
    {
        public static DiffClipboardWithFileCommand Instance { get; private set; }

        public static void Initialize(VsDiffPackage package)
        {
            Instance = new DiffClipboardWithFileCommand(package);
            Instance.Turn(package.Options.DiffClipboardWithCodeEnabled);
        }

        private string _selectedFile;
        private string _clipboardText;

        private DiffClipboardWithFileCommand(VsDiffPackage package)
            : base(package, Ids.CMD_SET, Ids.DIFF_CLIPBOARD_WITH_FILE_CMD_ID)
        {
        }

        protected override void OnBeforeQueryStatus()
        {
            Visible = ClipboardHelper.TryGetClipboardText(out _clipboardText)
                && SolutionExplorer.TryGetSingleSelectedFile(out _selectedFile);
        }

        protected override void OnExecute()
        {
            var extension = Path.GetExtension(_selectedFile);
            var clipboardFile = TempFileHelper.CreateTempFile("Clipboard" + extension, _clipboardText);

            DiffToolFactory.CreateDiffTool().Diff(clipboardFile, _selectedFile);

            if (!Package.Options.UseCustomDiffTool)
            {
                TempFileHelper.RemoveTempFile(clipboardFile);
            }
        }
    }
}