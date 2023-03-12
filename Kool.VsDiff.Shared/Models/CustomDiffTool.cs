using System;
using System.Diagnostics;
using System.IO;
using static Kool.VsDiff.Package;

namespace Kool.VsDiff.Models;

internal sealed class CustomDiffTool : IDiffTool
{
    public void Diff(string name1, string name2, string file1, string file2, Action<string, string> callback)
    {
        name1 ??= Path.GetFileName(file1);
        name2 ??= Path.GetFileName(file2);

        var args = Options.CustomDiffToolArgs.Replace("$FILE1", file1)
                                             .Replace("$FILE2", file2)
                                             .Replace("$NAME1", name1)
                                             .Replace("$NAME2", name2);
        var process = new Process
        {
            EnableRaisingEvents = true,
            StartInfo = new ProcessStartInfo(Options.CustomDiffToolPath, args)
        };
        process.Exited += (_, _) => callback?.Invoke(file1, file2);
        process.Start();
    }
}