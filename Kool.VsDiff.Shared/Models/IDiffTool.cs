using System;

namespace Kool.VsDiff.Models;

internal interface IDiffTool
{
    void Diff(string file1, string file2, Action<string, string> callback);
}