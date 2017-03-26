//-----------------------------------------------------------------------
// <copyright file="FileInfoHelper.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class provides methods to get details about files and directories.</summary>
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
    /// This class provides methods to get details about files and directories.
    /// </summary>
    public static class FileInfoHelper
    {
        /// <summary>
        /// Gets the size of a file in bytes.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>The size of a file in bytes.</returns>
        public static long GetSizeOfFile(string path)
        {
            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);

                return info.Length;
            }

            return 0;
        }

        /// <summary>
        /// Gets the size of a directory in bytes.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <param name="ignoreSubfolders">Determines whether sub folders should be ignored or not.</param>
        /// <returns>The size of a directory in bytes.</returns>
        public static long GetSizeOfDirectory(string path, bool ignoreSubfolders)
        {
            long size = 0;

            if (Directory.Exists(path))
            {
                DirectoryInfo info = new DirectoryInfo(path);

                if (!ignoreSubfolders)
                {
                    DirectoryInfo[] folders = info.GetDirectories();

                    foreach (DirectoryInfo folder in folders)
                    {
                        size += FileInfoHelper.GetSizeOfDirectory(folder.FullName, false);
                    }
                }

                FileInfo[] files = info.GetFiles();

                foreach (FileInfo file in files)
                {
                    size += file.Length;
                }
            }

            return size;
        }

        /// <summary>
        /// Takes a size in bytes and returns it as a formatted text with a more suitable size unit.
        /// </summary>
        /// <param name="size">The given size in bytes.</param>
        /// <returns>A formatted text with a more suitable size unit.</returns>
        public static string GetFormattedSizeWithSuffix(double size)
        {
            string[] units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };

            for (int i = 0; i < units.Length; i++)
            {
                if (size >= 1024)
                {
                    size = size / 1024;
                }
                else
                {
                    return Math.Round(size, 2, MidpointRounding.AwayFromZero).ToString() + " " + units[i];
                }
            }

            return size + " B";
        }

        /// <summary>
        /// Gets the creation date of a file or directory.
        /// </summary>
        /// <param name="path">The path of the file or directory.</param>
        /// <returns>The creation date of a file or directory.</returns>
        public static DateTime GetCreationDate(string path)
        {
            return File.GetCreationTime(path);
        }

        /// <summary>
        /// Gets the last modified date of a file or directory.
        /// </summary>
        /// <param name="path">The path of the file or directory.</param>
        /// <returns>The last modified date of a file or directory.</returns>
        public static DateTime GetLastModifiedDate(string path)
        {
            return File.GetLastWriteTime(path);            
        }

        /// <summary>
        /// Takes a DateTime and returns it as a formatted text. 
        /// </summary>
        /// <param name="date">The given DateTime which will be formatted.</param>
        /// <returns>A string containing the formatted DateTime.</returns>
        public static string GetFormattedDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Looks for specific attributes of a file or directory and returns them.
        /// </summary>
        /// <param name="path">The path of the file or directory.</param>
        /// <returns>A list of attributes, which the file or directory have.</returns>
        public static List<FileAttributes> GetAttributesOfFile(string path)
        {
            FileAttributes[] wantedAttributes = new FileAttributes[]
            {
                FileAttributes.ReadOnly,
                FileAttributes.Hidden,
                FileAttributes.System
            };

            List<FileAttributes> attributes = new List<FileAttributes>();

            if (File.Exists(path) || Directory.Exists(path))
            {
                FileAttributes foundAttributes = File.GetAttributes(path);

                for (int i = 0; i < wantedAttributes.Length; i++)
                {
                    if ((foundAttributes & wantedAttributes[i]) != 0)
                    {
                        attributes.Add(wantedAttributes[i]);
                    }
                }
            }

            return attributes;
        }

        /// <summary>
        /// Checks if the given file is a text file.
        /// </summary>
        /// <param name="path">The path of the given file.</param>
        /// <returns>A boolean indicating whether the given file is a text file or not.</returns>
        public static bool IsTextFile(string path)
        {
            string[] wantedFileExtensions = new string[]
            {
                "txt"
            };

            if (File.Exists(path))
            {
                for (int i = 0; i < wantedFileExtensions.Length; i++)
                {
                    if (path.EndsWith(wantedFileExtensions[i]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the content of a file as a list of the containing lines.
        /// </summary>
        /// <param name="path">The path of the given file.</param>
        /// <returns>A list containing the lines of the file.</returns>
        public static string[] GetContentOfFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllLines(path);
            }

            return new string[0];
        }

        /// <summary>
        /// Gets the binary content of a file as an array of bytes.
        /// </summary>
        /// <param name="path">The path of the given file.</param>
        /// <returns>An array of bytes representing the content of the file.</returns>
        public static byte[] GetBinaryContentOfFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }

            return new byte[0];
        }

        /// <summary>
        /// Gets the content of a file as a list of the containing lines.
        /// </summary>
        /// <param name="path">The path of the given file.</param>
        /// <param name="maximumBytes">The maximum amount of bytes which will be read.</param>
        /// <param name="readBytes">The actual amount of bytes which have been read.</param>
        /// <returns>A list containing the lines of the file.</returns>
        public static string[] GetContentOfFile(string path, int maximumBytes, ref int readBytes)
        {
            if (File.Exists(path))
            {
                byte[] array = FileInfoHelper.GetBytesOfFile(path, maximumBytes);

                readBytes = array.Length;

                string result = Encoding.ASCII.GetString(array.Take(readBytes).ToArray());

                return result.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            }

            return new string[0];
        }

        /// <summary>
        /// Gets the content of a file as an array of bytes.
        /// </summary>
        /// <param name="path">The path of the given file.</param>
        /// <param name="maximumBytes">The maximum amount of bytes which will be read.</param>
        /// <returns>An array of bytes representing the content of the file.</returns>
        public static byte[] GetBytesOfFile(string path, int maximumBytes)
        {
            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

                byte[] array = new byte[maximumBytes];

                int readBytes = stream.Read(array, 0, maximumBytes);

                return array.Take(readBytes).ToArray();
            }

            return new byte[0];
        }

        /// <summary>
        /// Takes a text and crops it so that the new text does not exceed the maximum amount of chars per line and the overall amount of lines.
        /// Text, which is too long, will be truncated.
        /// </summary>
        /// <param name="text">The text which will be cropped.</param>
        /// <param name="chars">The maximum amount of chars per line.</param>
        /// <param name="rows">The maximum amount of lines.</param>
        /// <returns>A list of lines which contain the cropped text.</returns>
        public static string[] CropText(string[] text, int chars, int rows)
        {
            List<string> newText = new List<string>();

            for (int i = 0; i < text.Length && newText.Count < rows; i++)
            {
                if (text[i].Length <= chars)
                {
                    newText.Add(text[i]);
                }
                else
                {
                    string temp = text[i];

                    while (temp.Length > chars && newText.Count < rows)
                    {
                        newText.Add(temp.Substring(0, chars));

                        temp = temp.Substring(chars, temp.Length - chars);
                    }

                    if (newText.Count < rows)
                    {
                        newText.Add(temp);
                    }
                }
            }

            return newText.ToArray();
        }
    }
}
