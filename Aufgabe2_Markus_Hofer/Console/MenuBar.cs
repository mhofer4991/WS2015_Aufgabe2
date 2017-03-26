//-----------------------------------------------------------------------
// <copyright file="MenuBar.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents the menu bar in a console application.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents the menu bar in a console application.
    /// </summary>
    public class MenuBar : DrawableObject
    {
        /// <summary> This list contains the current assignments of key to function. </summary>
        private List<KeyAssignment> currentAssignments;

        /// <summary> This is the amount of space it takes to display all assignments. </summary>
        private int outputLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuBar"/> class.
        /// </summary>
        public MenuBar()
        {
            this.currentAssignments = new List<KeyAssignment>();
        }

        /// <summary>
        /// Sets the list of current assignments of keys to it's function.
        /// </summary>
        /// <param name="settings">The new list of assignments.</param>
        public void SetKeyAssignments(List<KeyAssignment> settings)
        {
            this.currentAssignments = settings;
            this.outputLength = 0;

            foreach (KeyAssignment assignment in this.currentAssignments)
            {
                this.outputLength += assignment.ToString().Length + 2;
            }
        }

        /// <summary>
        /// Draws the menu bar on the console.
        /// </summary>
        public override void Draw()
        {
            if (this.Y < 0 || this.X < 0 || this.Height < 0 || this.Width < 0)
            {
                return;
            }

            string clear = string.Empty;

            for (int i = 0; i < this.Width; i++)
            {
                clear += " ";
            }

            for (int i = 0; i < this.Height; i++)
            {
                Console.SetCursorPosition(this.X, this.Y + i);
                Console.Write(clear);
            }

            if (this.outputLength <= this.Width)
            {
                Console.SetCursorPosition(this.X, this.Y + (this.Height / 2));

                foreach (KeyAssignment assignment in this.currentAssignments)
                {
                    Console.Write(assignment.ToString() + "  ");
                }
            }
            else if (this.outputLength <= this.Width * 2 && this.Height >= 2)
            {
                int tempLength = 0;

                Console.SetCursorPosition(this.X, this.Y);

                foreach (KeyAssignment assignment in this.currentAssignments)
                {
                    tempLength += assignment.ToString().Length + 2;

                    if (tempLength > this.Width)
                    {
                        Console.SetCursorPosition(this.X, this.Y + this.Height - 1);
                        tempLength = 0;
                    }

                    Console.Write(assignment.ToString() + "  ");
                }
            }
            else
            {
                int tempLength = 0;
                int offY = 0;

                Console.SetCursorPosition(this.X, this.Y);

                foreach (KeyAssignment assignment in this.currentAssignments)
                {
                    if (tempLength + assignment.ToString().Length > this.Width)
                    {
                        offY++;
                        tempLength = 0;
                    }

                    if (offY < this.Height)
                    {
                        Console.SetCursorPosition(this.X + tempLength, this.Y + offY);

                        Console.Write(assignment.ToString() + "  ");

                        tempLength += assignment.ToString().Length + 2;
                    }
                }
            }
        }
    }
}
