﻿using Kool.VsDiff.Models;
using static Kool.VsDiff.Models.VS;

namespace Kool.VsDiff.Commands;

internal sealed class DiffSelectedFilesCommand : BaseCommand
{
    public static DiffSelectedFilesCommand Instance { get; } = new();

    private string _name1;
    private string _name2;
    private string _file1;
    private string _file2;

    private DiffSelectedFilesCommand() : base(Ids.DIFF_SELECTED_FILES_CMD_ID)
    {
    }

    protected override void OnBeforeQueryStatus()
    {
        Visible = SolutionExplorer.TryGetSelectedFiles(out _name1, out _name2, out _file1, out _file2);
    }

    protected override void OnExecute()
    {
        DiffToolFactory.CreateDiffTool().Diff(_name1, _name2, _file1, _file2, null);
    }
}