﻿using Kool.VsDiff.Commands;
using Kool.VsDiff.Models;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace Kool.VsDiff.Pages
{
    [Guid(Ids.OPTIONS_VS_DIFF)]
    internal sealed class VsDiffOptions : UIElementDialogPage
    {
        private readonly DiffToolOptionsPage _page;

        public VsDiffOptions()
        {
            ResetOptions();
            _page = new DiffToolOptionsPage(this);
        }

        public bool DiffClipboardWithCodeEnabled { get; set; }

        public bool DiffClipboardWithFileEnabled { get; set; }

        public bool DiffClipboardWithDocumentEnabled { get; set; }

        public bool UseCustomDiffTool { get; set; }

        public string CustomDiffToolPath { get; set; }

        public string CustomDiffToolArgs { get; set; }

        protected override UIElement Child => _page;

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);
            // Ensure VS Environment Font Settings are applied.
            _page.UpdateDefaultStyle();
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            DiffClipboardWithCodeCommand.Instance.Turn(DiffClipboardWithCodeEnabled);
            DiffClipboardWithFileCommand.Instance.Turn(DiffClipboardWithFileEnabled);
            DiffClipboardWithDocumentCommand.Instance.Turn(DiffClipboardWithDocumentEnabled);

            DiffToolFactory.ClearCache();

            base.OnApply(e);
        }

        public override void ResetSettings()
        {
            ResetOptions();
            base.ResetSettings();
            Debug.WriteLine("The options have been reset.");
        }

        private void ResetOptions()
        {
            DiffClipboardWithCodeEnabled = true;
            DiffClipboardWithFileEnabled = true;
            DiffClipboardWithDocumentEnabled = true;
            UseCustomDiffTool = false;
            CustomDiffToolPath = @"%ProgramFiles(x86)%\WinMerge\WinMergeU.exe";
            CustomDiffToolArgs = "-e -u \"$FILE1\" \"$FILE2\"";
        }
    }
}