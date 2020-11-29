using Kool.VsDiff.Models;
using System;
using System.Diagnostics;
using System.IO;

namespace Kool.VsDiff.Commands
{
    internal sealed class DiffClipboardWithDocumentCommand : BaseCommand
    {
        public static DiffClipboardWithDocumentCommand Instance { get; private set; }

        public static void Initialize(VsDiffPackage package)
        {
            Instance = new DiffClipboardWithDocumentCommand(package);
            Instance.Turn(package.Options.DiffClipboardWithDocumentEnabled);
        }

        private string _documentFile;
        private string _clipboardText;

        private DiffClipboardWithDocumentCommand(VsDiffPackage package)
            : base(package, Ids.CMD_SET, Ids.DIFF_CLIPBOARD_WITH_DOCUMENT_CMD_ID)
        {
        }

        protected override void OnBeforeQueryStatus()
        {
            Visible = ClipboardHelper.TryGetClipboardText(out _clipboardText)
                    && TryGetDocumentActiveDocumentFile(out _documentFile);
        }

        protected override void OnExecute()
        {
            var extension = Path.GetExtension(_documentFile);
            var clipboardFile = TempFileHelper.CreateTempFile("Clipboard" + extension, _clipboardText);

            DiffToolFactory.CreateDiffTool().Diff(clipboardFile, _documentFile, (f, _) => TempFileHelper.RemoveTempFile(f));
        }

        private bool TryGetDocumentActiveDocumentFile(out string file)
        {
            file = null;
            try
            {
                file = Package.IDE.ActiveDocument.FullName;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to get active document file: {ex.Message}");
                return false;
            }
        }
    }
}