//-----------------------------------------------------------------------
// <copyright file="IFile.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>Classes, which implement this interface, can be treated like a file.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Classes, which implement this interface, can be treated like a file.
    /// </summary>
    public interface IFile : IFileObject
    {
        /// <summary>
        /// Gets the extension of a file.
        /// </summary>
        /// <returns>The extension of a file.</returns>
        string GetExtension();

        /// <summary>
        /// Gets the content of a file represented as an array of all lines.
        /// </summary>
        /// <returns>The content of a file.</returns>
        byte[] GetContent();
    }
}
