﻿using Kool.VsDiff.Models;
using System;
using System.Diagnostics;
using System.IO;
using static Kool.VsDiff.Package;

namespace Kool.VsDiff.Commands;

internal sealed class DiffClipboardWithDocumentCommand : BaseCommand
{
    public static DiffClipboardWithDocumentCommand Instance { get; } = new();

    private string _documentName;
    private string _documentFile;
    private string _clipboardText;

    private DiffClipboardWithDocumentCommand() : base(Ids.DIFF_CLIPBOARD_WITH_DOCUMENT_CMD_ID)
    {
    }

    protected override void OnBeforeQueryStatus()
    {
        Visible = ClipboardHelper.TryGetClipboardText(out _clipboardText)
            && TryGetDocumentActiveDocumentFile(out _documentName, out _documentFile);
    }

    protected override void OnExecute()
    {
        var extension = Path.GetExtension(_documentFile);
        var clipboardFile = TempFileHelper.CreateTempFile("Clipboard" + extension, _clipboardText);

        DiffToolFactory.CreateDiffTool().Diff("Clipboard", _documentName, clipboardFile, _documentFile, (f, _) => TempFileHelper.RemoveTempFile(f));
    }

    private bool TryGetDocumentActiveDocumentFile(out string name, out string file)
    {
        name = file = null;
        try
        {
            name = IDE.ActiveWindow.Caption;
            file = IDE.ActiveDocument.FullName;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to get active document file: {ex.Message}");
            return false;
        }
    }
}