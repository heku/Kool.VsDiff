﻿using EnvDTE80;
using Kool.VsDiff.Commands;
using Kool.VsDiff.Models;
using Kool.VsDiff.Pages;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace Kool.VsDiff
{
    [Guid(Ids.PACKAGE)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.VERSION, IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(VsDiffOptions), Vsix.PRODUCT, Vsix.PACKAGE, 0, 0, true, Sort = 100)]
    [ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.ShellInitialized_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class VsDiffPackage : AsyncPackage
    {
        internal DTE2 IDE { get; private set; }
        internal VsDiffOptions Options { get; private set; }
        internal OleMenuCommandService CommandService { get; private set; }
        internal IVsMonitorSelection MonitorSelection { get; private set; }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            IDE = await GetServiceAsync(typeof(EnvDTE.DTE)) as DTE2;
            CommandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            MonitorSelection = await GetServiceAsync(typeof(IVsMonitorSelection)) as IVsMonitorSelection;
            Options = GetDialogPage(typeof(VsDiffOptions)) as VsDiffOptions;

            VS.Initialize(this);
            DiffToolFactory.Initialize(this);

            DiffSelectedFilesCommand.Initialize(this);
            DiffClipboardWithCodeCommand.Initialize(this);
            DiffClipboardWithFileCommand.Initialize(this);
            DiffClipboardWithDocumentCommand.Initialize(this);

            Debug.WriteLine("Package is sited and initialized.");
        }
    }
}