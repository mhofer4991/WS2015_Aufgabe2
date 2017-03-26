//-----------------------------------------------------------------------
// <copyright file="FileOperationsHelper.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class provides methods to perform different file operations.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class provides methods to perform different file operations.
    /// </summary>
    public static class FileOperationsHelper
    {
        /// <summary>
        /// Copies the file at the given source path to the target path.
        /// If the folder already contains a file with the same name, it appends a (1) at the end of the filename.
        /// </summary>
        /// <param name="source">The source path of the given file.</param>
        /// <param name="target">The target path of the directory where the file will be inserted.</param>
        /// <param name="fileName">The original name of the file.</param>
        /// <returns>A string containing the path of the copied file.</returns>
        public static string CopyFile(string source, string target, string fileName)
        {
            // Check if the directories exist.
            if (File.Exists(source) && Directory.Exists(target))
            {
                // Check if the file already exists.
                if (File.Exists(Path.Combine(target, fileName)))
                {
                    FileInfo info = new FileInfo(source);
                    string newFileName = fileName.Substring(0, fileName.Length - info.Extension.Length);

                    // Check if a duplicate already has been created.
                    if (newFileName.EndsWith(")") && !fileName.Equals(info.Name))
                    {
                        if (newFileName.LastIndexOf(')') > (newFileName.LastIndexOf('(') + 1))
                        {
                            string number = newFileName.Substring(newFileName.LastIndexOf('(') + 1, newFileName.LastIndexOf(')') - (newFileName.LastIndexOf('(') + 1));

                            newFileName = newFileName.Substring(0, newFileName.Length - (number.Length + 2));

                            return CopyFile(source, target, newFileName + "(" + (int.Parse(number) + 1) + ")" + info.Extension);
                        }
                    }
                    else
                    {
                        return CopyFile(source, target, newFileName + "(1)" + info.Extension);
                    }
                }
                else
                {
                    File.Copy(source, Path.Combine(target, fileName));

                    return Path.Combine(target, fileName);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Creates a new directory at the given path.
        /// </summary>
        /// <param name="path">The path of the new directory.</param>
        /// <returns>A string containing the path of the created directory.</returns>
        public static string MakeDirectory(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);

            return MakeDirectory(path, info.Parent.FullName, info.Name);
        }

        /// <summary>
        /// Copies the folder at the given source path to the target path.
        /// If the folder already contains a folder with the same name, it appends a (1) at the end of the filename.
        /// </summary>
        /// <param name="source">The source path of the given folder.</param>
        /// <param name="target">The target path of the directory where the folder will be inserted.</param>
        /// <param name="directoryName">The original name of the folder.</param>
        /// <returns>A string containing the path of the copied folder.</returns>
        public static string CopyDirectory(string source, string target, string directoryName)
        {
            if (Directory.Exists(source))
            {
                DirectoryInfo info = new DirectoryInfo(source);

                string copied = MakeDirectory(Path.Combine(target, directoryName));

                if (!string.IsNullOrWhiteSpace(copied))
                {
                    DirectoryInfo[] folders = info.GetDirectories();

                    foreach (DirectoryInfo folder in folders)
                    {
                        CopyDirectory(folder.FullName, copied, folder.Name);
                    }

                    FileInfo[] files = info.GetFiles();

                    foreach (FileInfo file in files)
                    {
                        File.Copy(file.FullName, Path.Combine(copied, file.Name));
                    }
                }

                return copied;
            }

            return string.Empty;
        }

        /// <summary>
        /// Deletes the given file.
        /// </summary>
        /// <param name="path">The path of the given file.</param>
        /// <returns>A boolean indicating whether the file has been deleted or not.</returns>
        public static bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes the given folder.
        /// </summary>
        /// <param name="path">The path of the given folder.</param>
        /// <returns>A boolean indicating whether the folder has been deleted or not.</returns>
        public static bool DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Moves the file from the source path to the target path.
        /// </summary>
        /// <param name="source">The source path of the given file, which will be deleted.</param>
        /// <param name="target">The target path of the directory where the file will be inserted.</param>
        /// <param name="fileName">The new name of the file.</param>
        /// <returns>A string containing the path of the copied file.</returns>
        public static string MoveFile(string source, string target, string fileName)
        {
            string copied = CopyFile(source, target, fileName);

            if (!string.IsNullOrWhiteSpace(copied))
            {
                DeleteFile(source);
            }

            return copied;
        }

        /// <summary>
        /// Moves the folder from the source path to the target path.
        /// </summary>
        /// <param name="source">The source path of the given folder, which will be deleted.</param>
        /// <param name="target">The target path of the directory where the folder will be inserted.</param>
        /// <param name="directoryName">The new name of the directory.</param>
        /// <returns>A string containing the path of the copied folder.</returns>
        public static string MoveFolder(string source, string target, string directoryName)
        {
            string copied = CopyDirectory(source, target, directoryName);

            if (!string.IsNullOrWhiteSpace(copied))
            {
                DeleteDirectory(source);
            }

            return copied;
        }

        /// <summary>
        /// Creates a new directory at the given path.
        /// If a directory with the name already exists, it appends a (1) at the end of the directory name.
        /// </summary>
        /// <param name="originalPath">The original path of the directory.</param>
        /// <param name="parentPath">The path of the parent directory.</param>
        /// <param name="directoryName">The name of the new directory.</param>
        /// <returns>A string containing the path of the created directory.</returns>
        private static string MakeDirectory(string originalPath, string parentPath, string directoryName)
        {
            // Check if the parent directory exists.
            if (Directory.Exists(parentPath))
            {
                // Check if the directory already exists.
                if (Directory.Exists(Path.Combine(parentPath, directoryName)))
                {
                    DirectoryInfo info = new DirectoryInfo(originalPath);

                    // Check if a duplicate already has been created.
                    if (directoryName.EndsWith(")") && !info.Name.Equals(directoryName))
                    {
                        if (directoryName.LastIndexOf(')') > (directoryName.LastIndexOf('(') + 1))
                        {
                            string number = directoryName.Substring(directoryName.LastIndexOf('(') + 1, directoryName.LastIndexOf(')') - (directoryName.LastIndexOf('(') + 1));

                            directoryName = directoryName.Substring(0, directoryName.Length - (number.Length + 2));

                            return MakeDirectory(originalPath, parentPath, directoryName + "(" + (int.Parse(number) + 1) + ")");
                        }
                    }
                    else
                    {
                        return MakeDirectory(originalPath, parentPath, directoryName + "(1)");
                    }
                }
                else
                {
                    Directory.CreateDirectory(Path.Combine(parentPath, directoryName));

                    return Path.Combine(parentPath, directoryName);
                }
            }

            return string.Empty;
        }
    }
}
