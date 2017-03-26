//-----------------------------------------------------------------------
// <copyright file="WindowFrame.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a frame around a window in a console application.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a frame around a window in a console application.
    /// </summary>
    public class WindowFrame : DrawableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowFrame"/> class.
        /// </summary>
        public WindowFrame()
        {
            this.Title = string.Empty;
        }

        /// <summary> Gets the title of the window frame. </summary>
        /// <value> The title of the window frame. </value>
        public string Title { get; private set; }

        /// <summary>
        /// Sets the title of the window.
        /// </summary>
        /// <param name="title">The new title.</param>
        public void SetTitle(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// Draws the window frame on the console.
        /// </summary>
        public override void Draw()
        {
            if (this.Y < 0 || this.X < 0 || this.Height < 0 || this.Width < 0)
            {
                return;
            }

            // Draw bottom border
            Console.SetCursorPosition(this.X, this.Y + this.Height);

            string paint = "+";

            for (int i = 0; i < this.Width - 2; i++)
            {
                paint += "-";
            }

            paint += "+";

            Console.Write(paint);

            // Draw title and top border
            Console.SetCursorPosition(this.X, this.Y);

            Console.Write(paint);

            if ((this.Width - this.Title.Length) >= 2)
            {
                Console.SetCursorPosition(this.X + ((this.Width - this.Title.Length) / 2), this.Y);

                Console.Write(this.Title);
            }
            else
            {
                string temp = this.Title.Substring(0, this.Width - 2);

                Console.SetCursorPosition(this.X + 1, this.Y);

                Console.Write(temp);
            }

            // Draw left border
            for (int i = 1; i < this.Height; i++)
            {
                Console.SetCursorPosition(this.X, this.Y + i);
                Console.Write("|");
            }

            // Draw right border
            for (int i = 1; i < this.Height; i++)
            {
                Console.SetCursorPosition(this.X + this.Width - 1, this.Y + i);
                Console.Write("|");
            }
        }
    }
}
