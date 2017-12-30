using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;

namespace Kool.VsDiff.Models
{
    internal class VsDiffTool : IDiffTool
    {
        private readonly IVsDifferenceService _diffService;

        public VsDiffTool(VsDiffPackage package)
        {
            var sp = package as IServiceProvider;
            _diffService = (IVsDifferenceService)sp.GetService(typeof(SVsDifferenceService));
        }

        public void Diff(string file1, string file2)
        {
            var name1 = Path.GetFileName(file1);
            var name2 = Path.GetFileName(file2);
            var caption = $"{name1} vs {name2}";
            var tooltip = file1 + Environment.NewLine + file2;

            _diffService.OpenComparisonWindow2(file1, file2, caption, tooltip, file1, file2, null, null, 0).Show();
        }
    }
}