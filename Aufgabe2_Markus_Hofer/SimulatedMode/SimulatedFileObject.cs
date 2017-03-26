//-----------------------------------------------------------------------
// <copyright file="SimulatedFileObject.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents any file object, which only exists virtually in the memory.</summary>
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
    /// This class represents any file object, which only exists virtually in the memory.
    /// </summary>
    [Serializable]
    public abstract class SimulatedFileObject : IFileObject
    {
        /// <summary> Gets the extension of the file used for saving serialized simulated file objects. </summary>
        public const string SerializedFileExtension = ".simul";

        /// <summary> The name of the file object. </summary>
        private string name;

        /// <summary> The creation time of the file object. </summary>
        private DateTime creationTime;

        /// <summary> The last modified time of the file object. </summary>
        private DateTime lastModifiedTime;

        /// <summary> The attributes of the file object. </summary>
        private List<FileAttributes> attributes;

        /// <summary> The parent folder. </summary>
        private IFolder parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedFileObject"/> class.
        /// </summary>
        /// <param name="name">The name of the file object.</param>
        /// <param name="attributes">The attributes of the file object.</param>
        public SimulatedFileObject(string name, List<FileAttributes> attributes)
        {
            this.name = name;
            this.attributes = attributes;
        }

        /// <summary>
        /// Sets the parent folder of this file object.
        /// </summary>
        /// <param name="parent">The parent folder.</param>
        public void SetParent(IFolder parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Gets the parent folder of a folder.
        /// </summary>
        /// <returns>The parent folder of a folder.</returns>
        public IFolder GetParent()
        {
            return this.parent;
        }

        /// <summary>
        /// Gets the attributes of the file object.
        /// </summary>
        /// <returns>The attributes of the file object.</returns>
        public List<FileAttributes> GetAttributes()
        {
            return this.attributes;
        }

        /// <summary>
        /// Gets the creation time of the file object.
        /// </summary>
        /// <returns>The creation time of the file object.</returns>
        public DateTime GetCreationTime()
        {
            return this.creationTime;
        }

        /// <summary>
        /// Gets the last modified time of the file object.
        /// </summary>
        /// <returns>The last modified time of the file object.</returns>
        public DateTime GetLastModifiedTime()
        {
            return this.lastModifiedTime;
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
        /// Gets the size of the file object.
        /// </summary>
        /// <returns>The size of the file object.</returns>
        public abstract long GetSizeInBytes();

        /// <summary>
        /// Checks if the file object is a file or a folder.
        /// </summary>
        /// <returns>A boolean indicating whether the file object is a file or not.</returns>
        public abstract bool IsFile();

        /// <summary>
        /// Sets the creation time of the file object.
        /// </summary>
        /// <param name="time">The new creation time.</param>
        public void SetCreationTime(DateTime time)
        {
            this.creationTime = time;
        }

        /// <summary>
        /// Sets the last modified time of the file object.
        /// </summary>
        /// <param name="time">The new last modified time.</param>
        public void SetLastModifiedTime(DateTime time)
        {
            this.lastModifiedTime = time;
        }

        /// <summary>
        /// Deletes the simulated file object.
        /// </summary>
        /// <returns>A boolean indicating whether the deletion was successful or not.</returns>
        public abstract bool Delete();

        /// <summary>
        /// Copies the simulated file object to the given destination folder.
        /// </summary>
        /// <param name="destination">The destination folder.</param>
        /// <param name="name">The name of the file object.</param>
        /// <returns>A new file object, which represents the copy.</returns>
        public IFileObject Copy(IFolder destination, string name)
        {
            return destination.Paste(this, name);
        }

        /// <summary>
        /// Checks if the simulated file object exists.
        /// </summary>
        /// <returns>A boolean indicating whether the file object exists or not.</returns>
        public bool Exists()
        {
            foreach (IFileObject fileObject in this.GetParent().GetContent())
            {
                if ((this.IsFile() && fileObject is IFile) ||
                    (!this.IsFile() && fileObject is IFolder)) 
                {
                    if (fileObject.GetName().Equals(this.GetName()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Moves the simulated file object to the given destination folder.
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
    }
}
