using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Kool.VsDiff.Models
{
    internal static class VS
    {
        private static VsDiffPackage Package;

        public static void Initialize(VsDiffPackage package)
        {
            Package = package;

            OutputWindow.Initialize();
            SolutionExplorer.Initialize();
        }

        public static class OutputWindow
        {
            private static IVsOutputWindowPane VsOutputWindowPane;

            public static void Initialize()
            {
                var sp = Package as IServiceProvider;
                VsOutputWindowPane = (IVsOutputWindowPane)sp.GetService(typeof(SVsGeneralOutputWindowPane));
            }

            [Conditional("DEBUG")]
            public static void Debug(string message) => WriteLine("DEBUG", message);

            public static void Info(string message)
            {
                if (Package.Options.DiagnosticsMode)
                {
                    WriteLine("INFO", message);
                }
            }

            public static void Error(string message, Exception exception = null)
            {
                string FlattenMessage(Exception ex)
                {
                    if (ex.InnerException == null)
                    {
                        return ex.Message;
                    }
                    else
                    {
                        return $"{ex.Message} => {FlattenMessage(ex.InnerException)}";
                    }
                }

                WriteLine("ERROR", message + $"[{FlattenMessage(exception)}]");
            }

            private static void WriteLine(string category, string message)
            {
                var output = $"{Environment.NewLine}[{Vsix.PACKAGE}] >> {category}:{message}";
                System.Diagnostics.Debug.Write(output);
                VsOutputWindowPane.OutputString(output);
            }
        }

        public static class SolutionExplorer
        {
            private static UIHierarchy VsSolutionExplorer;

            public static void Initialize()
            {
                VsSolutionExplorer = Package.IDE.ToolWindows.SolutionExplorer;
            }

            public static bool TryGetSingleSelectedFile(out string file)
            {
                file = null;
                try
                {
                    var selectedItems = VsSolutionExplorer.SelectedItems as UIHierarchyItem[];
                    if (selectedItems.Length == 1)
                    {
                        var files = GetFiles(selectedItems);
                        if (files.Length == 1)
                        {
                            file = files[0];
                            return !string.IsNullOrEmpty(file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    OutputWindow.Error("Failed to get a single selected file.", ex);
                }

                return false;
            }

            public static bool TryGetSelectedFiles(out string file1, out string file2)
            {
                file1 = file2 = null;
                try
                {
                    var selectedItems = VsSolutionExplorer.SelectedItems as UIHierarchyItem[];
                    if (selectedItems.Length == 2)
                    {
                        var files = GetFiles(selectedItems);
                        if (files.Length == 2)
                        {
                            file1 = files[0];
                            file2 = files[1];
                            return !string.IsNullOrEmpty(file1) && !string.IsNullOrEmpty(file2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    OutputWindow.Error("Failed to get selected files.", ex);
                }

                return false;
            }

            private static string[] GetFiles(UIHierarchyItem[] selectedItems)
            {
                var projectItems = selectedItems.Select(x => x.Object).OfType<ProjectItem>();
                // The index of file names from 1 to FileCount for the project item
                // https://docs.microsoft.com/en-us/dotnet/api/envdte.projectitem.filenames?redirectedfrom=MSDN&view=visualstudiosdk-2017#EnvDTE_ProjectItem_FileNames_System_Int16_
                var projectNames = projectItems.Select(x => x.FileNames[1]);
                var fileNames = projectNames.Where(name => name != null && !name.EndsWith(Path.DirectorySeparatorChar.ToString()));
                return fileNames.ToArray();
            }
        }
    }
}