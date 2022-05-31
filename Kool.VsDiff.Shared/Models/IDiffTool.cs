using System;

namespace Kool.VsDiff.Models;

internal interface IDiffTool
{
    void Diff(string name1, string name2, string file1, string file2, Action<string, string> callback);
}