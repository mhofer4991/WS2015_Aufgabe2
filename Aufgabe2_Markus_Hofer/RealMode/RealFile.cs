//-----------------------------------------------------------------------
// <copyright file="RealFile.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a file, which exists on a physical drive.</summary>
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
    /// This class represents a file, which exists on a physical drive.
    /// </summary>
    public class RealFile : RealFileObject, IFile
    {
        /// <summary> Information about the file. </summary>
        private FileInfo info;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealFile"/> class.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        public RealFile(string path) : base(path)
        {
            this.info = new FileInfo(path);
        }
        
        /// <summary>
        /// Gets the content of a file represented as an array of all lines.
        /// </summary>
        /// <returns>The content of a file.</returns>
        public byte[] GetContent()
        {
            return FileInfoHelper.GetBinaryContentOfFile(this.Path);
        }

        /// <summary>
        /// Gets the extension of a file.
        /// </summary>
        /// <returns>The extension of a file.</returns>
        public string GetExtension()
        {
            return this.info.Extension;
        }
    }
}
