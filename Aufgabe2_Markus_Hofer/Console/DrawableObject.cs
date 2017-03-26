//-----------------------------------------------------------------------
// <copyright file="DrawableObject.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents an extended object which can be drawn by containing specific properties.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents an extended object which can be drawn by containing specific properties.
    /// </summary>
    public abstract class DrawableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableObject"/> class.
        /// </summary>
        public DrawableObject()
        {
        }

        /// <summary> Gets the width of the window frame. </summary>
        /// <value> The width of the window frame. </value>
        public int Width { get; private set; }

        /// <summary> Gets the height of the window frame. </summary>
        /// <value> The height of the window frame. </value>
        public int Height { get; private set; }

        /// <summary> Gets the X - position of the window frame. </summary>
        /// <value> The X - position of the window frame. </value>
        public int X { get; private set; }

        /// <summary> Gets the Y - position of the window frame. </summary>
        /// <value> The Y - position of the window frame. </value>
        public int Y { get; private set; }

        /// <summary>
        /// Sets the size of the window.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public void SetSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Sets the position of the window.
        /// </summary>
        /// <param name="x">The new X - position.</param>
        /// <param name="y">The new Y - position.</param>
        public void SetPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Draws the object on the console.
        /// </summary>
        public abstract void Draw();
    }
}