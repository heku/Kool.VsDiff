using EnvDTE80;
using Kool.VsDiff.Commands;
using Kool.VsDiff.Models;
using Kool.VsDiff.Pages;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace Kool.VsDiff
{
    [Guid(Ids.PACKAGE)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.VERSION, IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(VsDiffOptions), Vsix.PRODUCT, Vsix.PACKAGE, 0, 0, true)]
    [ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_string)]
    public sealed class VsDiffPackage : Package
    {
        internal DTE2 IDE { get; private set; }

        internal VsDiffOptions Options { get; private set; }

        internal OleMenuCommandService CommandService { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            IDE = GetService(typeof(EnvDTE.DTE)) as DTE2;
            CommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            Options = GetDialogPage(typeof(VsDiffOptions)) as VsDiffOptions;

            VS.Initialize(this);
            DiffToolFactory.Initialize(this);

            DiffSelectedFilesCommand.Initialize(this);
            DiffClipboardWithCodeCommand.Initialize(this);
            DiffClipboardWithFileCommand.Initialize(this);

            VS.OutputWindow.Info("Package is sited and initialized.");
        }
    }
}