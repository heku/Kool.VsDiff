﻿using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Kool.VsDiff.Models
{
    internal static class VS
    {
        private static VsDiffPackage Package;

        public static void Initialize(VsDiffPackage package)
        {
            Package = package;
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

                    var files = GetSelectedFiles(selectedItems);
                    if (files?.Count == 1)
                    {
                        file = files[0];
                        return !string.IsNullOrEmpty(file);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to get a single selected file: {ex.Message}.");
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

                    var files = GetSelectedFiles(selectedItems);
                    if (files?.Count == 2)
                    {
                        file1 = files[0];
                        file2 = files[1];
                        return !string.IsNullOrEmpty(file1) && !string.IsNullOrEmpty(file2);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to get selected files: {ex.Message}.");
                }

                return false;
            }

            private static List<string> GetSelectedFiles(SelectedItems selectedItems)
            {
                List<string> files = null;
                foreach (SelectedItem item in selectedItems)
                {
                    // The index of file names from 1 to FileCount for the project item
                    // https://docs.microsoft.com/en-us/dotnet/api/envdte.projectitem.filenames?redirectedfrom=MSDN&view=visualstudiosdk-2017#EnvDTE_ProjectItem_FileNames_System_Int16_
                    var file = item.ProjectItem?.FileNames[1];  // Not works for Folder View
                    if (file == null || file.EndsWith(SystemDirectorySeparator))
                    {
                        continue;
                    }
                    if (files == null)
                    {
                        files = new List<string>();
                    }
                    files.Add(file);
                }
                return files ?? GetSelectedFilesInsideFolderView();
            }

            private static List<string> GetSelectedFilesInsideFolderView()
            {
                var hierarchyPtr = IntPtr.Zero;
                var containerPtr = IntPtr.Zero;
                try
                {
                    if (Package.MonitorSelection != null &&
                        Package.MonitorSelection.GetCurrentSelection(out hierarchyPtr, out var itemid, out var multiSelect, out containerPtr) == VSConstants.S_OK)
                    {
                        var files = new List<string>();
                        if (itemid != VSConstants.VSITEMID_SELECTION)
                        {
                            if (itemid != VSConstants.VSCOOKIE_NIL &&
                                hierarchyPtr != IntPtr.Zero &&
                                Marshal.GetObjectForIUnknown(hierarchyPtr) is IVsHierarchy hierarchy &&
                                TryGetFile(hierarchy, itemid, out var file))
                            {
                                files.Add(file);
                            }
                        }
                        else if (multiSelect != null)
                        {
                            if (multiSelect.GetSelectionInfo(out var numberOfSelectedItems, out _) == VSConstants.S_OK &&
                                numberOfSelectedItems == 2)
                            {
                                var vsItemSelections = new VSITEMSELECTION[numberOfSelectedItems];
                                if (multiSelect.GetSelectedItems(0, numberOfSelectedItems, vsItemSelections) == VSConstants.S_OK)
                                {
                                    foreach (var selection in vsItemSelections)
                                    {
                                        if (TryGetFile(selection.pHier, selection.itemid, out var file))
                                        {
                                            files.Add(file);
                                        }
                                    }
                                }
                            }
                        }
                        return files;
                    }
                    return null;
                }
                finally
                {
                    if (hierarchyPtr != IntPtr.Zero)
                    {
                        Marshal.Release(hierarchyPtr);
                    }
                    if (containerPtr != IntPtr.Zero)
                    {
                        Marshal.Release(containerPtr);
                    }
                }
            }

            private static bool TryGetFile(IVsHierarchy hierarchy, uint itemid, out string file)
            {
                if (hierarchy != null &&
                    hierarchy.GetCanonicalName(itemid, out file) == VSConstants.S_OK &&
                    file != null &&
                    File.Exists(file))
                {
                    return true;
                }
                file = null;
                return false;
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
                => ErrorHandler.ThrowOnFailure(VsShellUtilities.ShowMessageBox(Package, message, title, icon, button, defaultButton));
        }
    }
}