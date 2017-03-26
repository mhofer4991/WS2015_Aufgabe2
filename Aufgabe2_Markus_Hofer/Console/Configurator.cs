//-----------------------------------------------------------------------
// <copyright file="Configurator.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents the configurator mode where the user can change the assignment of the keys to it's functions.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents the configurator mode where the user can change the assignment of the keys to it's functions.
    /// </summary>
    public class Configurator
    {
        /// <summary> This list contains all keys which are currently available for configuring. </summary>
        private List<ConsoleKey> availableKeys;

        /// <summary> This list contains all functions which are currently available for configuring. </summary>
        private List<KeyFunction> availableFunctions;

        /// <summary> Contains all current assignments, which have been set by the user. </summary>
        private Dictionary<KeyFunction, ConsoleKey> currentAssignments;

        /// <summary>
        /// Initializes a new instance of the <see cref="Configurator"/> class.
        /// </summary>
        /// <param name="keysWindow">Window, where all available keys will be displayed.</param>
        /// <param name="functionsWindow">Window, where all available functions will be displayed.</param>
        public Configurator(ScrollableWindow keysWindow, ScrollableWindow functionsWindow)
        {
            this.KeysWindow = keysWindow;
            this.FunctionsWindow = functionsWindow;

            this.currentAssignments = new Dictionary<KeyFunction, ConsoleKey>();

            this.KeysWindow.OnItemClicked += this.KeysWindow_OnItemClicked;
            this.FunctionsWindow.OnItemClicked += this.FunctionsWindow_OnItemClicked;
        }
        
        /// <summary>
        /// Delegate for event OnNewSettingsApplied.
        /// </summary>
        /// <param name="settings">The new settings.</param>
        public delegate void NewSettingsApplied(Dictionary<KeyFunction, ConsoleKey> settings);

        /// <summary>
        /// Gets called when the user applied the new settings.
        /// </summary>
        public event NewSettingsApplied OnNewSettingsApplied;

        /// <summary> Gets the window, where all available keys will be displayed. </summary>
        /// <value> The window, where all available keys will be displayed. </value>
        public ScrollableWindow KeysWindow { get; private set; }

        /// <summary> Gets the window, where all available functions will be displayed. </summary>
        /// <value> The window, where all available functions will be displayed. </value>
        public ScrollableWindow FunctionsWindow { get; private set; }

        /// <summary>
        /// Gets the default key assignments to control the configurator.
        /// </summary>
        /// <returns>A dictionary representing the assignment from function to key.</returns>
        public static Dictionary<KeyFunction, ConsoleKey> GetDefaultKeyAssignments()
        {
            Dictionary<KeyFunction, ConsoleKey> assignments = new Dictionary<KeyFunction, ConsoleKey>();

            assignments.Add(KeyFunction.Up, ConsoleKey.UpArrow);
            assignments.Add(KeyFunction.Down, ConsoleKey.DownArrow);
            assignments.Add(KeyFunction.Select, ConsoleKey.Enter);
            assignments.Add(KeyFunction.Cancel, ConsoleKey.Escape);

            return assignments;
        }

        /// <summary>
        /// Shows the configurator and fires an event if the user assigned all keys.
        /// </summary>
        public void RequestNewConfiguration()
        {
            this.availableKeys = KeyAssignment.GetAllAvailableKeys();

            this.availableFunctions = KeyAssignment.GetAllAvailableFunctions();

            this.currentAssignments.Clear();

            this.KeysWindow.Reset();

            foreach (ConsoleKey key in this.availableKeys)
            {
                this.KeysWindow.AddScrollableItem(new ScrollItemInformation(key.ToString(), key));
            }

            do
            {
                this.FunctionsWindow.Reset();

                this.KeysWindow.Draw();
                this.FunctionsWindow.Draw();
            }
            while (this.availableFunctions.Count > 0 && this.KeysWindow.WaitForItemClick());

            if (this.availableFunctions.Count == 0)
            {
                if (this.OnNewSettingsApplied != null)
                {
                    this.OnNewSettingsApplied(this.currentAssignments);
                }
            }
        }

        /// <summary>
        /// Gets called when the user chose a key to configure it.
        /// </summary>
        /// <param name="info">Info about the chosen item.</param>
        private void KeysWindow_OnItemClicked(ScrollItemInformation info)
        {
            foreach (KeyFunction function in this.availableFunctions)
            {
                this.FunctionsWindow.AddScrollableItem(new ScrollItemInformation(function.ToString(), new KeyAssignment((ConsoleKey)info.Attachment, function)));
            }

            this.FunctionsWindow.Draw();

            if (this.FunctionsWindow.WaitForItemClick())
            {
                this.KeysWindow.RemoveItem(info);

                this.availableKeys.Remove((ConsoleKey)info.Attachment);
            }
        }

        /// <summary>
        /// Gets called when the user chose a function for a specified key.
        /// </summary>
        /// <param name="info">Info about the chosen item.</param>
        private void FunctionsWindow_OnItemClicked(ScrollItemInformation info)
        {
            KeyAssignment assignment = (KeyAssignment)info.Attachment;

            this.availableFunctions.Remove(assignment.Function);

            this.currentAssignments.Add(assignment.Function, assignment.Key);
        }
    }
}
