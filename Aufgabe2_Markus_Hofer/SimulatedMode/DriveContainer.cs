//-----------------------------------------------------------------------
// <copyright file="DriveContainer.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents only a container for drives represented as folders.</summary>
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
    /// This class represents only a container for drives represented as folders.
    /// </summary>
    public class DriveContainer : SimulatedFolder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriveContainer"/> class.
        /// </summary>
        /// <param name="name">The name of the container.</param>
        public DriveContainer(string name) : base(name, new List<FileAttributes>())
        {
        }
    }
}
