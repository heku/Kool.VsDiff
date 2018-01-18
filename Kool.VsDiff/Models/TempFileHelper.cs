using System;
using System.IO;
using System.Text;

namespace Kool.VsDiff.Models
{
    internal static class TempFileHelper
    {
        private static readonly Encoding VsDefaultEncoding = new UTF8Encoding(true);

        public static string CreateTempFile(string fileName, string content)
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(path);    // Ensure temp path exists.
            var tempFile = Path.Combine(path, fileName);
            File.AppendAllText(tempFile, content, VsDefaultEncoding);
            VS.OutputWindow.Info($"Created temp file {tempFile}");
            return tempFile;
        }

        public static void RemoveTempFile(string fileName)
        {
            try
            {
                Directory.Delete(Path.GetDirectoryName(fileName), true);
                VS.OutputWindow.Info($"Removed temp file {fileName}");
            }
            catch (Exception ex)
            {
                VS.OutputWindow.Error($"Failed to remove temp file {fileName}, Exception {ex.Message}.");
            }
        }
    }
}