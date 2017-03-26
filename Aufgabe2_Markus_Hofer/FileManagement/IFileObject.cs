//-----------------------------------------------------------------------
// <copyright file="IFileObject.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>Classes, which implement this interface, share common properties of a file or a folder.</summary>
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
    /// Classes, which implement this interface, share common properties of a file or a folder.
    /// </summary>
    public interface IFileObject
    {
        /// <summary>
        /// Gets the name of a file or a folder.
        /// </summary>
        /// <returns>The name of a file or a folder.</returns>
        string GetName();

        /// <summary>
        /// Checks if the file object is a file or a folder.
        /// </summary>
        /// <returns>A boolean indicating whether the file object is a file or not.</returns>
        bool IsFile();

        /// <summary>
        /// Gets the size of a file or a folder.
        /// </summary>
        /// <returns>The size of a file or a folder.</returns>
        long GetSizeInBytes();

        /// <summary>
        /// Gets the creation time of a file or a folder.
        /// </summary>
        /// <returns>The creation time of a file or a folder.</returns>
        DateTime GetCreationTime();

        /// <summary>
        /// Sets the creation time of a file or a folder.
        /// </summary>
        /// <param name="time">The new creation time.</param>
        void SetCreationTime(DateTime time);

        /// <summary>
        /// Gets the last modified time of a file or a folder.
        /// </summary>
        /// <returns>The last modified time of a file or a folder.</returns>
        DateTime GetLastModifiedTime();

        /// <summary>
        /// Sets the last modified time of a file or a folder.
        /// </summary>
        /// <param name="time">The new last modified time.</param>
        void SetLastModifiedTime(DateTime time);

        /// <summary>
        /// Gets the attributes of a file or a folder.
        /// </summary>
        /// <returns>The attributes of a file or a folder.</returns>
        List<FileAttributes> GetAttributes();

        /// <summary>
        /// Gets the parent folder of a file object.
        /// </summary>
        /// <returns>The parent folder of a file object.</returns>
        IFolder GetParent();

        /// <summary>
        /// Checks if the file object exists.
        /// </summary>
        /// <returns>A boolean indicating whether the file object exists or not.</returns>
        bool Exists();

        /// <summary>
        /// Deletes the file object.
        /// </summary>
        /// <returns>A boolean indicating whether the deletion was successful or not.</returns>
        bool Delete();

        /// <summary>
        /// Copies the file object to the given destination folder.
        /// </summary>
        /// <param name="destination">The destination folder.</param>
        /// <param name="name">The name of the file object.</param>
        /// <returns>A new file object, which represents the copy.</returns>
        IFileObject Copy(IFolder destination, string name);

        /// <summary>
        /// Moves the file object to the given destination folder.
        /// </summary>
        /// <param name="destination">The destination folder.</param>
        /// <param name="name">The name of the file object.</param>
        /// <returns>A boolean indicating whether the file object has been moved or not.</returns>
        bool Move(IFolder destination, string name);
    }
}
