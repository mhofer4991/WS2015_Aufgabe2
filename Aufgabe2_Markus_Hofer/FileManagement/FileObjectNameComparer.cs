//-----------------------------------------------------------------------
// <copyright file="FileObjectNameComparer.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a comparison class for comparing two file objects by their name.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a comparison class for comparing two file objects by their name.
    /// </summary>
    public class FileObjectNameComparer : IComparer<IFileObject>
    {
        /// <summary>
        /// Compares the two file object instances by their name.
        /// </summary>
        /// <param name="x">The first file object.</param>
        /// <param name="y">The second file object.</param>
        /// <returns>
        /// Less than zero if the current file object's name is smaller,
        /// zero if the current file object has the same name or
        /// greater than zero if the current file object's name is greater.
        /// </returns>
        public int Compare(IFileObject x, IFileObject y)
        {
            return x.GetName().CompareTo(y.GetName());
        }
    }
}
