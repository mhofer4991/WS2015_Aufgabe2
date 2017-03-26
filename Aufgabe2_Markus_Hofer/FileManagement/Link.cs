//-----------------------------------------------------------------------
// <copyright file="Link.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a link to any file or folder.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a link to any file or folder.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        /// <param name="name">The name of the link.</param>
        /// <param name="target">Target of the link, which is any file object.</param>
        public Link(string name, IFileObject target)
        {
            this.Name = name;
            this.Target = target;
        }

        /// <summary> Gets the name of the link. </summary>
        /// <value> The name of the link. </value>
        public string Name { get; private set; }

        /// <summary> Gets the target of the link. </summary>
        /// <value> The target of the link. </value>
        public IFileObject Target { get; private set; }
    }
}
