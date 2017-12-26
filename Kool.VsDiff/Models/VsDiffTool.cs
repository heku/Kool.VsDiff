namespace Kool.VsDiff.Models
{
    internal class VsDiffTool : IDiffTool
    {
        private const string VS_DIFF_CMD = "Tools.DiffFiles";
        private readonly VsDiffPackage _package;

        public VsDiffTool(VsDiffPackage package)
        {
            _package = package;
        }

        public void Diff(string file1, string file2)
        {
            string args = $"\"{file1}\" \"{file2}\"";
            _package.IDE.ExecuteCommand(VS_DIFF_CMD, args);
        }
    }
}