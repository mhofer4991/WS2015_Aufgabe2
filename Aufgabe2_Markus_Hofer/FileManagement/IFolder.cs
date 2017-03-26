//-----------------------------------------------------------------------
// <copyright file="IFolder.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>Classes, which implement this interface, can be treated like a folder.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Classes, which implement this interface, can be treated like a folder.
    /// </summary>
    public interface IFolder : IFileObject
    {
        /// <summary>
        /// Gets the content of a folder represented as a list of IFileObjects.
        /// </summary>
        /// <returns>The content of a folder.</returns>
        List<IFileObject> GetContent();

        /// <summary>
        /// Creates a new folder in the current folder.
        /// </summary>
        /// <param name="name">The name of the new folder.</param>
        /// <returns>An instance of the new folder.</returns>
        IFolder MakeDirectory(string name);

        /// <summary>
        /// Pastes the given file object in this folder.
        /// </summary>
        /// <param name="fileObject">The given file object.</param>
        /// <param name="name">The name of the file object.</param>
        /// <returns>A boolean indicating whether the file object has been pasted or not.</returns>
        IFileObject Paste(IFileObject fileObject, string name);
    }
}
