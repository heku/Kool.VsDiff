using System;
using System.Diagnostics;
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
            Debug.WriteLine($"Created temp file {tempFile}");
            return tempFile;
        }

        public static void RemoveTempFile(string fileName)
        {
            try
            {
                Directory.Delete(Path.GetDirectoryName(fileName), true);
                Debug.WriteLine($"Removed temp file {fileName}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to remove temp file {fileName}, exception: {ex.Message}.");
            }
        }
    }
}