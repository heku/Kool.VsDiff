using System;
using System.Diagnostics;
using static Kool.VsDiff.VsDiffPackage;

namespace Kool.VsDiff.Models;

internal sealed class CustomDiffTool : IDiffTool
{
    public void Diff(string file1, string file2, Action<string, string> callback)
    {
        var args = Options.CustomDiffToolArgs.Replace("$FILE1", file1).Replace("$FILE2", file2);
        var process = new Process
        {
            EnableRaisingEvents = true,
            StartInfo = new ProcessStartInfo(Options.CustomDiffToolPath, args)
        };
        process.Exited += (_, _) => callback?.Invoke(file1, file2);
        process.Start();
    }
}