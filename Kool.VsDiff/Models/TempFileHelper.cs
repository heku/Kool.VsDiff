using System;
using System.IO;
using System.Text;

namespace Kool.VsDiff.Models
{
    internal static class TempFileHelper
    {
        public static string CreateTempFile(string name, string extension, string content)
        {
            var fileName = $"{name}_{DateTime.UtcNow.ToFileTimeUtc()}{extension}";
            var tempFile = Path.Combine(Path.GetTempPath(), fileName);

            File.AppendAllText(tempFile, content, Encoding.Unicode);
            VS.OutputWindow.Debug($"Created temp file {tempFile}");

            return tempFile;
        }
    }
}