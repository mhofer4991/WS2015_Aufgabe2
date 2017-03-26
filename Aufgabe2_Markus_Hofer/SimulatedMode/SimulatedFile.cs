//-----------------------------------------------------------------------
// <copyright file="SimulatedFile.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a file, which only exists virtually in the memory.</summary>
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
    /// This class represents a file, which only exists virtually in the memory.
    /// </summary>
    [Serializable]
    public class SimulatedFile : SimulatedFileObject, IFile
    {
        /// <summary> The extension of the file. </summary>
        private string extension;

        /// <summary> The content of the file. </summary>
        private byte[] content;

        /// <summary> The size of the file. </summary>
        private long size;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedFile"/> class.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <param name="size">The size of the file.</param>
        /// <param name="attributes">The attributes of the file.</param>
        /// <param name="content">The content of the file.</param>
        public SimulatedFile(string name, long size, List<FileAttributes> attributes, byte[] content) : base(name, attributes)
        {
            this.extension = string.Empty;

            if (name.LastIndexOf('.') > 0)
            {
                this.extension = name.Substring(name.LastIndexOf('.'), name.Length - name.LastIndexOf('.'));
            }

            // Encoding.ASCII.GetString(content).Split(new string[] { "\r\n" }, StringSplitOptions.None);
            this.content = content;
            this.size = size;
        }

        /// <summary>
        /// Gets the content of a file represented as an array of all lines.
        /// </summary>
        /// <returns>The content of a file.</returns>
        public byte[] GetContent()
        {
            return this.content;
        }

        /// <summary>
        /// Gets the extension of a file.
        /// </summary>
        /// <returns>The extension of a file.</returns>
        public string GetExtension()
        {
            return this.extension;
        }

        /// <summary>
        /// Gets the size of the file object.
        /// </summary>
        /// <returns>The size of the file object.</returns>
        public override long GetSizeInBytes()
        {
            return this.size;
        }

        /// <summary>
        /// Checks if the file object is a file or a folder.
        /// </summary>
        /// <returns>A boolean indicating whether the file object is a file or not.</returns>
        public override bool IsFile()
        {
            return true;
        }

        /// <summary>
        /// Deletes the simulated file.
        /// </summary>
        /// <returns>A boolean indicating whether the deletion was successful or not.</returns>
        public override bool Delete()
        {
            return ((SimulatedFolder)this.GetParent()).DeleteContent(this);
        }
    }
}
