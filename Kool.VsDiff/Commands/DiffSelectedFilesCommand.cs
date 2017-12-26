using Kool.VsDiff.Models;
using static Kool.VsDiff.Models.VS;

namespace Kool.VsDiff.Commands
{
    internal sealed class DiffSelectedFilesCommand : BaseCommand
    {
        public static DiffSelectedFilesCommand Instance { get; private set; }

        public static void Initialize(VsDiffPackage package)
        {
            Instance = new DiffSelectedFilesCommand(package);
            package.CommandService.AddCommand(Instance);
        }

        private string _file1;
        private string _file2;

        private DiffSelectedFilesCommand(VsDiffPackage package) : base(package, Ids.CMD_SET, Ids.DIFF_SELECTED_FILES_CMD_ID)
        {
        }

        protected override void OnBeforeQueryStatus()
        {
            Visible = SolutionExplorer.TryGetSelectedFiles(out _file1, out _file2);
        }

        protected override void OnExecute()
        {
            DiffToolFactory.CreateDiffTool().Diff(_file1, _file2);
        }
    }
}