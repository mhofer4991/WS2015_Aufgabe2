//-----------------------------------------------------------------------
// <copyright file="RealFileObject.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents any file object, which exists on a physical drive.</summary>
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
    /// This class represents any file object, which exists on a physical drive.
    /// </summary>
    public abstract class RealFileObject : IFileObject
    {
        /// <summary> The name of the file object. </summary>
        private string name;

        /// <summary> Indicates whether the file object is a file or not. </summary>
        private bool isFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealFileObject"/> class.
        /// </summary>
        /// <param name="path">The path of the file object.</param>
        public RealFileObject(string path)
        {
            this.Path = path;

            if (File.Exists(path))
            {
                this.isFile = true;
                this.name = new FileInfo(path).Name;
            }
            else if (Directory.Exists(path))
            {
                this.isFile = false;
                this.name = new DirectoryInfo(path).Name;
            }
            else
            {
                throw new IOException("The given path could not be found!");
            }
        }

        /// <summary> Gets or sets the path of the file object. </summary>
        /// <value> The path of the file object. </value>
        public string Path { get; protected set; }

        /// <summary>
        /// Gets the attributes of the file object.
        /// </summary>
        /// <returns>The attributes of the file object.</returns>
        public List<FileAttributes> GetAttributes()
        {
            return FileInfoHelper.GetAttributesOfFile(this.Path);
        }

        /// <summary>
        /// Gets the creation time of the file object.
        /// </summary>
        /// <returns>The creation time of the file object.</returns>
        public DateTime GetCreationTime()
        {
            return FileInfoHelper.GetCreationDate(this.Path);
        }

        /// <summary>
        /// Gets the last modified time of the file object.
        /// </summary>
        /// <returns>The last modified time of the file object.</returns>
        public DateTime GetLastModifiedTime()
        {
            return FileInfoHelper.GetLastModifiedDate(this.Path);
        }

        /// <summary>
        /// Gets the size of the file object.
        /// </summary>
        /// <returns>The size of the file object.</returns>
        public long GetSizeInBytes()
        {
            if (this.isFile)
            {
                return FileInfoHelper.GetSizeOfFile(this.Path);
            }
            else
            {
                return FileInfoHelper.GetSizeOfDirectory(this.Path, false);
            }
        }

        /// <summary>
        /// Gets the name of the file object.
        /// </summary>
        /// <returns>The name of the file object.</returns>
        public string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Checks if the file object is a file or a folder.
        /// </summary>
        /// <returns>A boolean indicating whether the file object is a file or not.</returns>
        public bool IsFile()
        {
            return this.isFile;
        }

        /// <summary>
        /// Sets the creation time of the file object.
        /// </summary>
        /// <param name="time">The new creation time.</param>
        public void SetCreationTime(DateTime time)
        {
            File.SetCreationTime(this.Path, time);
        }

        /// <summary>
        /// Sets the last modified time of the file object.
        /// </summary>
        /// <param name="time">The new last modified time.</param>
        public void SetLastModifiedTime(DateTime time)
        {
            File.SetLastWriteTime(this.Path, time);
        }

        /// <summary>
        /// Deletes the real file object.
        /// </summary>
        /// <returns>A boolean indicating whether the deletion was successful or not.</returns>
        public bool Delete()
        {
            if (this.IsFile())
            {
                return FileOperationsHelper.DeleteFile(this.Path);
            }
            else
            {
                return FileOperationsHelper.DeleteDirectory(this.Path);
            }
        }

        /// <summary>
        /// Gets the parent folder of a real folder.
        /// </summary>
        /// <returns>The parent folder of a real folder.</returns>
        public IFolder GetParent()
        {
            DirectoryInfo parent;

            if (this.IsFile())
            {
                parent = new FileInfo(this.Path).Directory;
            }
            else
            {
                parent = new DirectoryInfo(this.Path).Parent;
            }

            if (parent != null)
            {
                return new RealFolder(parent.FullName);
            }
            else if (!this.IsFile())
            {
                return RealFolder.GetRoot();
            }

            return null;
        }

        /// <summary>
        /// Copies the real file object to the given destination folder.
        /// </summary>
        /// <param name="destination">The destination folder.</param>
        /// <param name="name">The name of the real file object.</param>
        /// <returns>A new file object, which represents the copy.</returns>
        public IFileObject Copy(IFolder destination, string name)
        {
            return destination.Paste(this, name);
        }

        /// <summary>
        /// Checks if the real file object exists.
        /// </summary>
        /// <returns>A boolean indicating whether the file object exists or not.</returns>
        public bool Exists()
        {
            if (this.IsFile())
            {
                return File.Exists(this.Path);
            }
            else
            {
                return Directory.Exists(this.Path);
            }
        }

        /// <summary>
        /// Moves the real file object to the given destination folder.
        /// </summary>
        /// <param name="destination">The destination folder.</param>
        /// <param name="name">The name of the file object.</param>
        /// <returns>A boolean indicating whether the file object has been moved or not.</returns>
        public bool Move(IFolder destination, string name)
        {
            if (destination.Paste(this, name) != null)
            {
                return this.Delete();
            }

            return false;
        }

        /// <summary>
        /// Checks if another instance of a real file object is equal by comparing their paths.
        /// </summary>
        /// <param name="obj">The instance of another real file object.</param>
        /// <returns>A boolean indicating whether both file objects have the same path or not.</returns>
        public override bool Equals(object obj)
        {
            if (obj is RealFileObject)
            {
                return ((RealFileObject)obj).Path.Equals(this.Path);
            }

            return false;
        }
    }
}
