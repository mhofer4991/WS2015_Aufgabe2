//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>The user can use this program to browse his local file system and perform different file operations.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The user can use this program to browse his local file system and perform different file operations.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// This method represents the entry point of the program.
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        public static void Main(string[] args)
        {
            Main main = new Aufgabe2_Markus_Hofer.Main();
            main.Start();
        }
    }
}
