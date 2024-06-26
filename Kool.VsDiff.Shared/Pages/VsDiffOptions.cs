﻿using Kool.VsDiff.Commands;
using Kool.VsDiff.Models;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace Kool.VsDiff.Pages;

[Guid(Ids.OPTIONS_VS_DIFF)]
internal sealed class VsDiffOptions : UIElementDialogPage
{
    private DiffToolOptionsPage _page;

    public VsDiffOptions() => SetDefaults();

    public bool UseCustomDiffTool { get; set; }
    public string CustomDiffToolPath { get; set; }
    public string CustomDiffToolArgs { get; set; }

    public bool DiffSelectedFilesEnabled { get; set; }
    public bool DiffClipboardWithCodeEnabled { get; set; }
    public bool DiffClipboardWithFileEnabled { get; set; }
    public bool DiffClipboardWithDocumentEnabled { get; set; }
    public bool PreferToUsePreviewWindow { get; set; }

    protected override UIElement Child => _page ??= new DiffToolOptionsPage(this);

    protected override void OnActivate(CancelEventArgs e)
    {
        base.OnActivate(e);
        _page?.UpdateDefaultStyle();// Ensure VS Environment Font Settings are applied.
    }

    protected override void OnApply(PageApplyEventArgs e)
    {
        RefreshCommandsState();
        DiffToolFactory.ClearCache();

        base.OnApply(e);
    }

    public void RefreshCommandsState()
    {
        DiffSelectedFilesCommand.Instance.Turn(DiffSelectedFilesEnabled);
        DiffClipboardWithCodeCommand.Instance.Turn(DiffClipboardWithCodeEnabled);
        DiffClipboardWithFileCommand.Instance.Turn(DiffClipboardWithFileEnabled);
        DiffClipboardWithDocumentCommand.Instance.Turn(DiffClipboardWithDocumentEnabled);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _page = null;
    }

    public override void ResetSettings()
    {
        SetDefaults();
        base.ResetSettings();
        Debug.WriteLine("The options have been reset.");
    }

    private void SetDefaults()
    {
        UseCustomDiffTool = false;
        CustomDiffToolPath = @"%ProgramFiles(x86)%\WinMerge\WinMergeU.exe";
        CustomDiffToolArgs = "-e -u \"$FILE1\" \"$FILE2\" -dl \"$NAME1\" -dr \"$NAME2\"";
        DiffSelectedFilesEnabled = true;
        DiffClipboardWithCodeEnabled = true;
        DiffClipboardWithFileEnabled = true;
        DiffClipboardWithDocumentEnabled = true;
        PreferToUsePreviewWindow = true;
    }
}