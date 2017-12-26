using Kool.VsDiff.Models;
using System.IO;
using System.Windows;
using static Kool.VsDiff.Models.VS;

namespace Kool.VsDiff.Commands
{
    internal sealed class DiffClipboardWithFileCommand : BaseCommand
    {
        public static DiffClipboardWithFileCommand Instance { get; private set; }

        public static void Initialize(VsDiffPackage package)
        {
            Instance = new DiffClipboardWithFileCommand(package);
            package.CommandService.AddCommand(Instance);
        }

        private string _clipboardText;
        private string _selectedFile;

        private DiffClipboardWithFileCommand(VsDiffPackage package) : base(package, Ids.CMD_SET, Ids.DIFF_CLIPBOARD_WITH_FILE_CMD_ID)
        {
        }

        protected override void OnBeforeQueryStatus()
        {
            Visible = Package.Options.DiffClipboardWithFileEnabled
                && TryGetClipboardText()
                && SolutionExplorer.TryGetSingleSelectedFile(out _selectedFile);
        }

        private bool TryGetClipboardText()
        {
            _clipboardText = Clipboard.GetText();
            return _clipboardText?.Length > 0;
        }

        protected override void OnExecute()
        {
            var extension = Path.GetExtension(_selectedFile);
            var clipboardFile = TempFileHelper.CreateTempFile("Clipboard", extension, _clipboardText);
            DiffToolFactory.CreateDiffTool().Diff(clipboardFile, _selectedFile);
        }
    }
}