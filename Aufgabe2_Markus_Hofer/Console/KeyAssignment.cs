//-----------------------------------------------------------------------
// <copyright file="KeyAssignment.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This enumeration represents an assignment of a key to a function.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This enumeration represents an assignment of a key to a function.
    /// </summary>
    public class KeyAssignment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyAssignment"/> class.
        /// </summary>
        /// <param name="key">The key to call the function.</param>
        /// <param name="function">The function which is assigned to the key.</param>
        public KeyAssignment(ConsoleKey key, KeyFunction function)
        {
            this.Key = key;
            this.Function = function;
        }

        /// <summary> Gets the key to call the function. </summary>
        /// <value> The key to call the function. </value>
        public ConsoleKey Key { get; private set; }

        /// <summary> Gets the function which is assigned to the key. </summary>
        /// <value> The function which is assigned to the key. </value>
        public KeyFunction Function { get; private set; }

        /// <summary>
        /// Returns all keys, which are available to assign.
        /// </summary>
        /// <returns>A list of all available keys.</returns>
        public static List<ConsoleKey> GetAllAvailableKeys()
        {
            List<ConsoleKey> keys = new ConsoleKey[]
            {
                ConsoleKey.F2,
                ConsoleKey.F3,
                ConsoleKey.F4,
                ConsoleKey.F5,
                ConsoleKey.F6,
                ConsoleKey.F7,
                ConsoleKey.F8,
                ConsoleKey.F9
            }.ToList();

            return keys;
        }

        /// <summary>
        /// Returns all functions, which can be assigned to a key.
        /// </summary>
        /// <returns>A list of all available functions.</returns>
        public static List<KeyFunction> GetAllAvailableFunctions()
        {
            List<KeyFunction> functions = new KeyFunction[]
            {
                KeyFunction.Config,
                KeyFunction.Copy,
                KeyFunction.Cut,
                KeyFunction.Paste,
                KeyFunction.Mkdir,
                KeyFunction.Delete,
                KeyFunction.Toggle,
                KeyFunction.Quit
            }.ToList();

            return functions;
        }

        /// <summary>
        /// Converts the given dictionary to a list of key assignments.
        /// </summary>
        /// <param name="assignments">The given dictionary.</param>
        /// <returns>The converted list.</returns>
        public static List<KeyAssignment> ConvertToList(Dictionary<KeyFunction, ConsoleKey> assignments)
        {
            List<KeyAssignment> listOfAssignments = new List<KeyAssignment>();

            foreach (KeyFunction function in assignments.Keys)
            {
                listOfAssignments.Add(new KeyAssignment(assignments[function], function));
            }

            return listOfAssignments;
        }
        
        /// <summary>
        /// Creates a string describing the key assignment.
        /// </summary>
        /// <returns>A string describing the key assignment.</returns>
        public override string ToString()
        {
            return this.Key.ToString() + "-" + this.Function.ToString();
        }
    }
}