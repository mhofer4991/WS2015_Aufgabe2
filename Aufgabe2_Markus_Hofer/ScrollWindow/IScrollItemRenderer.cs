//-----------------------------------------------------------------------
// <copyright file="IScrollItemRenderer.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>Classes, which implement this interface, are able to draw items of a scroll window.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Classes, which implement this interface, are able to draw items of a scroll window.
    /// </summary>
    public interface IScrollItemRenderer
    {
        /// <summary>
        /// Draws the scroll item on the console.
        /// </summary>
        /// <param name="x">X - position where the item will be rendered.</param>
        /// <param name="y">Y - position where the item will be rendered.</param>
        /// <param name="width">Width of the item.</param>
        /// <param name="height">Height of the item.</param>
        /// <param name="info">Scroll item, which will be rendered.</param>
        void Draw(int x, int y, int width, int height, ScrollItemInformation info);

        /// <summary>
        /// Draws the selected scroll item on the console.
        /// </summary>
        /// <param name="x">X - position where the item will be rendered.</param>
        /// <param name="y">Y - position where the item will be rendered.</param>
        /// <param name="width">Width of the item.</param>
        /// <param name="height">Height of the item.</param>
        /// <param name="info">Scroll item, which will be rendered.</param>
        void DrawSelected(int x, int y, int width, int height, ScrollItemInformation info);

        /// <summary>
        /// Gets the height of a scroll item.
        /// </summary>
        /// <returns>The height of a scroll item.</returns>
        int GetItemHeight();
    }
}
