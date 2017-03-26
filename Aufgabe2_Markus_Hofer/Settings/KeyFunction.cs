//-----------------------------------------------------------------------
// <copyright file="KeyFunction.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This enumeration contains different types of key functions.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This enumeration contains different types of key functions.
    /// </summary>
    public enum KeyFunction
    {
        /// <summary>
        /// Function for configuring.
        /// </summary>
        Config,

        /// <summary>
        /// Function for copying.
        /// </summary>
        Copy,

        /// <summary>
        /// Function for cutting.
        /// </summary>
        Cut,

        /// <summary>
        /// Function for pasting.
        /// </summary>
        Paste,

        /// <summary>
        /// Function for making a directory.
        /// </summary>
        Mkdir,

        /// <summary>
        /// Function for deleting.
        /// </summary>
        Delete,

        /// <summary>
        /// Function for toggling.
        /// </summary>
        Toggle,

        /// <summary>
        /// Function for quitting.
        /// </summary>
        Quit,

        /// <summary>
        /// Function for navigating up.
        /// </summary>
        Up,

        /// <summary>
        /// Function for navigating down.
        /// </summary>
        Down,

        /// <summary>
        /// Function for selecting.
        /// </summary>
        Select,

        /// <summary>
        /// Function for canceling.
        /// </summary>
        Cancel
    }
}
