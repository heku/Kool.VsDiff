using EnvDTE;
using Kool.VsDiff.Models;
using System.IO;
using static Kool.VsDiff.VsDiffPackage;

namespace Kool.VsDiff.Commands;

internal sealed class DiffClipboardWithCodeCommand : BaseCommand
{
    public static DiffClipboardWithCodeCommand Instance { get; } = new();

    private string _selectionText;
    private string _clipboardText;

    private DiffClipboardWithCodeCommand() : base(Ids.DIFF_CLIPBOARD_WITH_CODE_CMD_ID)
    {
    }

    private Document ActiveDocument => IDE.ActiveDocument;

    protected override void OnBeforeQueryStatus()
    {
        Visible = ClipboardHelper.TryGetClipboardText(out _clipboardText)
            && TryGetSelectionText();
    }

    private bool TryGetSelectionText()
    {
        _selectionText = (ActiveDocument?.Selection as TextSelection)?.Text;
        return _selectionText?.Length > 0;
    }

    protected override void OnExecute()
    {
        var extension = Path.GetExtension(ActiveDocument.Name);

        var clipboardFile = TempFileHelper.CreateTempFile("Clipboard" + extension, _clipboardText);
        var selectionFile = TempFileHelper.CreateTempFile("Selection" + extension, _selectionText);

        DiffToolFactory.CreateDiffTool().Diff("Clipboard", "Selection", clipboardFile, selectionFile,
            (file1, file2) =>
             {
                 TempFileHelper.RemoveTempFile(file1);
                 TempFileHelper.RemoveTempFile(file2);
             });
    }
}