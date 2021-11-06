using System;
using System.Diagnostics;

namespace Kool.VsDiff.Models
{
    internal sealed class CustomDiffTool : IDiffTool
    {
        private readonly string _command;
        private readonly string _args;

        public CustomDiffTool(string command, string args)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _args = args ?? throw new ArgumentNullException(nameof(args));
        }

        public void Diff(string file1, string file2, Action<string, string> callback)
        {
            var args = _args.Replace("$FILE1", file1).Replace("$FILE2", file2);
            var process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(_command, args)
            };
            process.Exited += (_, _) => callback?.Invoke(file1, file2);
            process.Start();
        }
    }
}