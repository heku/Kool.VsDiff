using EnvDTE;
using Kool.VsDiff.Models;
using System.IO;

namespace Kool.VsDiff.Commands
{
    internal sealed class DiffClipboardWithCodeCommand : BaseCommand
    {
        public static DiffClipboardWithCodeCommand Instance { get; private set; }

        public static void Initialize(VsDiffPackage package)
        {
            Instance = new DiffClipboardWithCodeCommand(package);
            Instance.Turn(package.Options.DiffClipboardWithCodeEnabled);
        }

        private string _selectionText;
        private string _clipboardText;

        private DiffClipboardWithCodeCommand(VsDiffPackage package)
            : base(package, Ids.CMD_SET, Ids.DIFF_CLIPBOARD_WITH_CODE_CMD_ID)
        {
        }

        private Document ActiveDocument => Package.IDE.ActiveDocument;

        protected override void OnBeforeQueryStatus()
        {
            Visible = ClipboardHelper.TryGetClipboardText(out _clipboardText)
                && TryGetSelectionText();
        }

        private bool TryGetSelectionText()
        {
            _selectionText = (ActiveDocument.Selection as TextSelection)?.Text;
            return _selectionText?.Length > 0;
        }

        protected override void OnExecute()
        {
            var extension = Path.GetExtension(ActiveDocument.Name);

            var selectionFile = TempFileHelper.CreateTempFile("Selection" + extension, _selectionText);
            var clipboardFile = TempFileHelper.CreateTempFile("Clipboard" + extension, _clipboardText);

            DiffToolFactory.CreateDiffTool().Diff(clipboardFile, selectionFile);

            TempFileHelper.RemoveTempFile(selectionFile);
            TempFileHelper.RemoveTempFile(clipboardFile);
        }
    }
}