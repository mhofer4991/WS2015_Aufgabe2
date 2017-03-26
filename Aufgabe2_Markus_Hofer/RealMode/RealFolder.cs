//-----------------------------------------------------------------------
// <copyright file="RealFolder.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a folder, which exists on a physical drive.</summary>
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
    /// This class represents a folder, which exists on a physical drive.
    /// </summary>
    public class RealFolder : RealFileObject, IFolder
    {
        /// <summary> Information about the folder. </summary>
        private DirectoryInfo info;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealFolder"/> class.
        /// </summary>
        /// <param name="path">The path of the folder.</param>
        public RealFolder(string path) : base(path)
        {
            this.info = new DirectoryInfo(path);
        }

        /// <summary>
        /// Returns the root of the real file system, which is the listing of all available drives.
        /// </summary>
        /// <returns>A listing of all available drives.</returns>
        public static DriveContainer GetRoot()
        {
            DriveContainer folder = new DriveContainer("Available drives");

            folder.SetParent(null);

            DriveInfo[] drives = DriveInfo.GetDrives();
            List<IFileObject> children = new List<IFileObject>();

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    RealFolder realFolder = new RealFolder(drive.Name);

                    children.Add(realFolder);
                }
            }

            folder.SetChildren(children);

            return folder;
        }

        /// <summary>
        /// Gets the content of a folder represented as a list of IFileObjects.
        /// </summary>
        /// <returns>The content of a folder.</returns>
        public List<IFileObject> GetContent()
        {
            List<IFileObject> content = new List<IFileObject>();

            DirectoryInfo info = new DirectoryInfo(this.Path);

            DirectoryInfo[] folders = info.GetDirectories();

            foreach (DirectoryInfo folder in folders)
            {
                content.Add(new RealFolder(folder.FullName));
            }

            FileInfo[] files = info.GetFiles();

            foreach (FileInfo file in files)
            {
                content.Add(new RealFile(file.FullName));
            }

            return content;
        }

        /// <summary>
        /// Creates a new folder in the current folder.
        /// </summary>
        /// <param name="name">The name of the new folder.</param>
        /// <returns>An instance of the new folder.</returns>
        public IFolder MakeDirectory(string name)
        {
            return new RealFolder(FileOperationsHelper.MakeDirectory(System.IO.Path.Combine(this.Path, name)));
        }

        /// <summary>
        /// Pastes the given real file object in this folder.
        /// </summary>
        /// <param name="fileObject">The given file object.</param>
        /// <param name="name">The name of the file object.</param>
        /// <returns>A new file object, which represents the copy.</returns>
        public IFileObject Paste(IFileObject fileObject, string name)
        {
            string newPath = string.Empty;

            if (fileObject.IsFile())
            {
                newPath = FileOperationsHelper.CopyFile(((RealFileObject)fileObject).Path, this.Path, name);

                return new RealFile(newPath);
            }
            else
            {
                newPath = FileOperationsHelper.CopyDirectory(((RealFileObject)fileObject).Path, this.Path, name);

                return new RealFolder(newPath);
            }
        }
    }
}
