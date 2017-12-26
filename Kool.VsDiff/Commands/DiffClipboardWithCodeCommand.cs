using EnvDTE;
using Kool.VsDiff.Models;
using System.IO;
using System.Windows;

namespace Kool.VsDiff.Commands
{
    internal sealed class DiffClipboardWithCodeCommand : BaseCommand
    {
        public static DiffClipboardWithCodeCommand Instance { get; private set; }

        public static void Initialize(VsDiffPackage package)
        {
            Instance = new DiffClipboardWithCodeCommand(package);
            package.CommandService.AddCommand(Instance);
        }

        private string _selectedText;
        private string _clipboardText;

        private DiffClipboardWithCodeCommand(VsDiffPackage package) : base(package, Ids.CMD_SET, Ids.DIFF_CLIPBOARD_WITH_CODE_CMD_ID)
        {
        }

        public Document ActiveDocument => Package.IDE.ActiveDocument;

        protected override void OnBeforeQueryStatus()
        {
            Visible = Package.Options.DiffClipboardWithCodeEnabled
                && TryGetClipboardText()
                && TryGetSelectedText();
        }

        private bool TryGetClipboardText()
        {
            _clipboardText = Clipboard.GetText();
            return _clipboardText?.Length > 0;
        }

        private bool TryGetSelectedText()
        {
            _selectedText = (ActiveDocument.Selection as TextSelection)?.Text;
            return _selectedText?.Length > 0;
        }

        protected override void OnExecute()
        {
            var extension = Path.GetExtension(ActiveDocument.Name);

            var selection = TempFileHelper.CreateTempFile("Selection", extension, _selectedText);
            var clipboard = TempFileHelper.CreateTempFile("Clipboard", extension, _clipboardText);

            DiffToolFactory.CreateDiffTool().Diff(clipboard, selection);
        }
    }
}