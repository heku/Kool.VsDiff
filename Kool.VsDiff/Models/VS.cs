using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Kool.VsDiff.Models
{
    internal static class VS
    {
        private static VsDiffPackage Package;

        public static void Initialize(VsDiffPackage package)
        {
            Package = package;

            OutputWindow.Initialize();
        }

        public static class OutputWindow
        {
            private static IVsOutputWindowPane VsOutputWindowPane;

            public static void Initialize()
            {
                var sp = Package as IServiceProvider;
                VsOutputWindowPane = (IVsOutputWindowPane)sp.GetService(typeof(SVsGeneralOutputWindowPane));
                Assumes.Present(VsOutputWindowPane);
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
                var output = $"{Environment.NewLine}[{Vsix.PACKAGE}]>{category}: {message}";
                ErrorHandler.ThrowOnFailure(VsOutputWindowPane.OutputString(output));
            }
        }

        public static class SolutionExplorer
        {
            private static readonly string SystemDirectorySeparator = Path.DirectorySeparatorChar.ToString();

            public static bool TryGetSingleSelectedFile(out string file)
            {
                file = null;
                try
                {
                    var selectedItems = Package.IDE.SelectedItems;
                    if (selectedItems.Count != 1)
                    {
                        return false;
                    }

                    var files = GetFiles(selectedItems);
                    if (files.Count == 1)
                    {
                        file = files[0];
                        return !string.IsNullOrEmpty(file);
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
                    var selectedItems = Package.IDE.SelectedItems;
                    if (selectedItems.Count != 2)
                    {
                        return false;
                    }

                    var files = GetFiles(selectedItems);
                    if (files.Count == 2)
                    {
                        file1 = files[0];
                        file2 = files[1];
                        return !string.IsNullOrEmpty(file1) && !string.IsNullOrEmpty(file2);
                    }
                }
                catch (Exception ex)
                {
                    OutputWindow.Error("Failed to get selected files.", ex);
                }

                return false;
            }

            private static List<string> GetFiles(SelectedItems selectedItems)
            {
                var files = new List<string>(selectedItems.Count);

                foreach (SelectedItem item in selectedItems)
                {
                    // The index of file names from 1 to FileCount for the project item
                    // https://docs.microsoft.com/en-us/dotnet/api/envdte.projectitem.filenames?redirectedfrom=MSDN&view=visualstudiosdk-2017#EnvDTE_ProjectItem_FileNames_System_Int16_
                    var name = item.ProjectItem?.FileNames[1];
                    if (name == null || name.EndsWith(SystemDirectorySeparator))
                    {
                        continue;
                    }
                    files.Add(name);
                }

                return files;
            }
        }

        public static class MessageBox
        {
            public static void Info(string title, string message)
                => Show(title, message, OLEMSGICON.OLEMSGICON_INFO, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

            public static void Warning(string title, string message)
                => Show(title, message, OLEMSGICON.OLEMSGICON_WARNING, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

            public static void Error(string title, string message)
                => Show(title, message, OLEMSGICON.OLEMSGICON_CRITICAL, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

            public static void Show(string title, string message, OLEMSGICON icon, OLEMSGBUTTON button, OLEMSGDEFBUTTON defaultButton)
            {
                ErrorHandler.ThrowOnFailure(VsShellUtilities.ShowMessageBox(Package, message, title, icon, button, defaultButton));
            }
        }
    }
}