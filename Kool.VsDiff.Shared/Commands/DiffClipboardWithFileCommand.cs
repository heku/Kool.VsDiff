using Kool.VsDiff.Models;
using System.IO;
using static Kool.VsDiff.Models.VS;

namespace Kool.VsDiff.Commands;

internal sealed class DiffClipboardWithFileCommand : BaseCommand
{
    public static DiffClipboardWithFileCommand Instance { get; } = new();

    private string _selectedName;
    private string _selectedFile;
    private string _clipboardText;

    private DiffClipboardWithFileCommand() : base(Ids.DIFF_CLIPBOARD_WITH_FILE_CMD_ID)
    {
    }

    protected override void OnBeforeQueryStatus()
    {
        Visible = ClipboardHelper.TryGetClipboardText(out _clipboardText)
            && SolutionExplorer.TryGetSingleSelectedFile(out _selectedName, out _selectedFile);
    }

    protected override void OnExecute()
    {
        var extension = Path.GetExtension(_selectedFile);
        var clipboardFile = TempFileHelper.CreateTempFile("Clipboard" + extension, _clipboardText);

        DiffToolFactory.CreateDiffTool().Diff("Clipboard", _selectedName, clipboardFile, _selectedFile, (f, _) => TempFileHelper.RemoveTempFile(f));
    }
}