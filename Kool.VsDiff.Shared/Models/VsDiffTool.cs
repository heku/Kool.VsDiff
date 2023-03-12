using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;
using static Kool.VsDiff.Package;

namespace Kool.VsDiff.Models;

internal class VsDiffTool : IDiffTool
{
    private readonly IVsDifferenceService _diffService;

    public VsDiffTool()
    {
        var sp = Instance as IServiceProvider;
        _diffService = (IVsDifferenceService)sp.GetService(typeof(SVsDifferenceService));
        Assumes.Present(_diffService);
    }

    public void Diff(string name1, string name2, string file1, string file2, Action<string, string> callback)
    {
        name1 ??= Path.GetFileName(file1);
        name2 ??= Path.GetFileName(file2);
        var caption = $"{name1} vs {name2}";
        var tooltip = file1 + Environment.NewLine + file2;

        if (Options.PreferToUsePreviewWindow)
        {
            using (new NewDocumentStateScope(__VSNEWDOCUMENTSTATE2.NDS_TryProvisional, VSConstants.NewDocumentStateReason.Navigation))
            {
                _diffService.OpenComparisonWindow2(file1, file2, caption, tooltip, file1, file2, null, null, 0).Show();
            }
        }
        else
        {
            _diffService.OpenComparisonWindow2(file1, file2, caption, tooltip, file1, file2, null, null, 0).Show();
        }

        callback?.Invoke(file1, file2);
    }
}