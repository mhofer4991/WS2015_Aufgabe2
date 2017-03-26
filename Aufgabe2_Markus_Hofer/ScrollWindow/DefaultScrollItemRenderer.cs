//-----------------------------------------------------------------------
// <copyright file="DefaultScrollItemRenderer.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class draws items of a scroll window on a console.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class draws items of a scroll window on a console.
    /// </summary>
    public class DefaultScrollItemRenderer : IScrollItemRenderer
    {
        /// <summary>
        /// Draws the scroll item on the console.
        /// </summary>
        /// <param name="x">X - position where the item will be rendered.</param>
        /// <param name="y">Y - position where the item will be rendered.</param>
        /// <param name="width">Width of the item.</param>
        /// <param name="height">Height of the item.</param>
        /// <param name="info">Scroll item, which will be rendered.</param>
        public void Draw(int x, int y, int width, int height, ScrollItemInformation info)
        {
            Console.SetCursorPosition(x, y);

            if (info.Text.Length + 2 <= width)
            {
                Console.Write(" " + info.Text + " ");
            }
            else
            {
                string temp = info.Text.Substring(0, width - 4);

                Console.Write(" " + temp + ".. ");
            }
        }

        /// <summary>
        /// Draws the selected scroll item on the console.
        /// </summary>
        /// <param name="x">X - position where the item will be rendered.</param>
        /// <param name="y">Y - position where the item will be rendered.</param>
        /// <param name="width">Width of the item.</param>
        /// <param name="height">Height of the item.</param>
        /// <param name="info">Scroll item, which will be rendered.</param>
        public void DrawSelected(int x, int y, int width, int height, ScrollItemInformation info)
        {
            ConsoleColor tempF = Console.ForegroundColor;
            ConsoleColor tempB = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;

            this.Draw(x, y, width, height, info);

            Console.ForegroundColor = tempF;
            Console.BackgroundColor = tempB;
        }

        /// <summary>
        /// Gets the height of a scroll item.
        /// </summary>
        /// <returns>The height of a scroll item.</returns>
        public int GetItemHeight()
        {
            return 1;
        }
    }
}
