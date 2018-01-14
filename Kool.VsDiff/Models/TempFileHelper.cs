using System;
using System.IO;
using System.Text;

namespace Kool.VsDiff.Models
{
    internal static class TempFileHelper
    {
        public static string CreateTempFile(string fileName, string content)
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(path);    // Ensure temp path exists.
            var tempFile = Path.Combine(path, fileName);
            File.AppendAllText(tempFile, content, Encoding.Unicode);
            VS.OutputWindow.Debug($"Created temp file {tempFile}");
            return tempFile;
        }

        public static void RemoveTempFile(string fileName)
        {
            try
            {
                Directory.Delete(Path.GetDirectoryName(fileName), true);
            }
            catch (Exception ex)
            {
                VS.OutputWindow.Debug($"Failed to remove temp file : {fileName}");
                VS.OutputWindow.Debug($"Exception : {ex.Message}");
            }
        }
    }
}