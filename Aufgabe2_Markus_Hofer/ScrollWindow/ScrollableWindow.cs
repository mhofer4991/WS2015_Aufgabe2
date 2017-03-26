//-----------------------------------------------------------------------
// <copyright file="ScrollableWindow.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a scrollable window in a console application.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a scrollable window in a console application.
    /// </summary>
    public class ScrollableWindow : DrawableObject
    {
        /// <summary> Contains all items which are fixed, meaning that they will not move when scrolling. </summary>
        private List<ScrollItemInformation> fixedItems;

        /// <summary> Contains all items which will move, when the cursor is at the bottom of the window. </summary>
        private List<ScrollItemInformation> scrollableItems;

        /// <summary> Renders each scroll item. </summary>
        private IScrollItemRenderer itemRenderer;

        /// <summary> Index of the currently selected item. </summary>
        private int selectionIndex;

        /// <summary> The height of a item, which will be defined by the renderer. </summary>
        private int itemHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollableWindow"/> class.
        /// </summary>
        /// <param name="itemRenderer">The renderer, which will be used to display the items.</param>
        public ScrollableWindow(IScrollItemRenderer itemRenderer)
        {
            this.fixedItems = new List<ScrollItemInformation>();

            this.scrollableItems = new List<ScrollItemInformation>();

            this.selectionIndex = 0;

            this.itemRenderer = itemRenderer;
            this.itemHeight = this.itemRenderer.GetItemHeight();
        }

        /// <summary>
        /// Delegate for event OnItemSelected.
        /// </summary>
        /// <param name="info">Info about the selected item.</param>
        public delegate void ItemSelected(ScrollItemInformation info);

        /// <summary>
        /// Delegate for event OnItemClicked.
        /// </summary>
        /// <param name="info">Info about the clicked item.</param>
        public delegate void ItemClicked(ScrollItemInformation info);

        /// <summary>
        /// Gets called when the user selects an item during the navigation.
        /// </summary>
        public event ItemSelected OnItemSelected;

        /// <summary>
        /// Gets called when the user clicks on an item.
        /// </summary>
        public event ItemClicked OnItemClicked;

        /// <summary>
        /// Adds a fixed scroll item to the window.
        /// </summary>
        /// <param name="info">Info about the new item.</param>
        public void AddFixedItem(ScrollItemInformation info)
        {
            this.fixedItems.Add(info);
        }

        /// <summary>
        /// Adds a scrollable scroll item to the window.
        /// </summary>
        /// <param name="info">Info about the new item.</param>
        public void AddScrollableItem(ScrollItemInformation info)
        {
            this.scrollableItems.Add(info);
        }

        /// <summary>
        /// Adds a scrollable scroll item to the window at the given index.
        /// </summary>
        /// <param name="info">Info about the new item.</param>
        /// <param name="index">The index where the item will be added.</param>
        public void AddScrollableItem(ScrollItemInformation info, int index)
        {
            this.scrollableItems.Insert(index, info);

            if (this.selectionIndex - this.fixedItems.Count >= index)
            {
                this.selectionIndex++;
            }
        }

        /// <summary>
        /// Gets the currently selected scroll item.
        /// </summary>
        /// <returns>The currently selected scroll item.</returns>
        public ScrollItemInformation GetCurrentlySelectedItem()
        {
            List<ScrollItemInformation> merged = this.fixedItems.ToList();
            merged.AddRange(this.scrollableItems);

            if (merged.Count > 0 && this.selectionIndex >= 0)
            {
                return merged[this.selectionIndex];
            }

            return null;
        }

        /// <summary>
        /// Removes a scroll item from the window.
        /// </summary>
        /// <param name="info">Info about the item, which will be removed.</param>
        public void RemoveItem(ScrollItemInformation info)
        {
            List<ScrollItemInformation> merged = this.fixedItems.ToList();
            merged.AddRange(this.scrollableItems);

            if (merged.IndexOf(info) != -1)
            {
                if (this.selectionIndex > merged.IndexOf(info))
                {
                    this.selectionIndex--;
                }
                else if (this.selectionIndex == merged.IndexOf(info))
                {
                    merged.Remove(info);

                    if (this.selectionIndex == merged.Count)
                    {
                        this.selectionIndex--;
                    }

                    if (this.OnItemSelected != null)
                    {
                        this.OnItemSelected(this.GetCurrentlySelectedItem());
                    }
                }

                this.fixedItems.Remove(info);
                this.scrollableItems.Remove(info);
            }
        }
        
        /// <summary>
        /// Returns the content of the scrollable window.
        /// </summary>
        /// <returns>The content of the scrollable window.</returns>
        public List<ScrollItemInformation> GetContent()
        {
            List<ScrollItemInformation> merged = this.fixedItems.ToList();
            merged.AddRange(this.scrollableItems);

            return merged;
        }

        /// <summary>
        /// Resets the scrollable window.
        /// </summary>
        public void Reset()
        {
            this.fixedItems.Clear();
            this.scrollableItems.Clear();

            this.selectionIndex = 0;
        }

        /// <summary>
        /// Accepts input until the user clicks on an item or cancels the input via pressing escape.
        /// </summary>
        /// <returns>A boolean indicating whether the user canceled the input or not.</returns>
        public bool WaitForItemClick()
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();

            while (cki.Key != ConsoleKey.Escape && cki.Key != ConsoleKey.Enter)
            {
                cki = Console.ReadKey(true);

                this.HandleKey(cki);
            }

            return cki.Key != ConsoleKey.Escape;
        }

        /// <summary>
        /// Handles a given console key information.
        /// </summary>
        /// <param name="cki">The given console key information.</param>
        public void HandleKey(ConsoleKeyInfo cki)
        {
            int oldSelectionIndex = this.selectionIndex;
            List<ScrollItemInformation> merged = this.fixedItems.ToList();
            merged.AddRange(this.scrollableItems);

            switch (cki.Key)
            {
                case ConsoleKey.UpArrow:
                    if (this.selectionIndex > 0)
                    {
                        this.selectionIndex--;
                    }

                    break;
                case ConsoleKey.DownArrow:
                    if (this.selectionIndex < (this.fixedItems.Count + this.scrollableItems.Count) - 1)
                    {
                        this.selectionIndex++;
                    }

                    break;
                case ConsoleKey.Enter:
                    if (this.OnItemClicked != null)
                    {
                        if (this.GetCurrentlySelectedItem() != null)
                        {
                            this.OnItemClicked(this.GetCurrentlySelectedItem());
                        }
                    }

                    break;
            }

            if ((cki.Key == ConsoleKey.UpArrow || cki.Key == ConsoleKey.DownArrow) &&
                (oldSelectionIndex != this.selectionIndex))
            {
                if (((this.selectionIndex + 1) * this.itemHeight) - 1 > this.Height || ((oldSelectionIndex + 1) * this.itemHeight) - 1 > this.Height)
                {
                    this.Draw();
                }
                else
                {
                    this.itemRenderer.Draw(this.X, this.Y + (this.itemHeight * oldSelectionIndex), this.Width, this.itemHeight, merged[oldSelectionIndex]);

                    this.itemRenderer.DrawSelected(this.X, this.Y + (this.itemHeight * this.selectionIndex), this.Width, this.itemHeight, merged[this.selectionIndex]);
                }

                if (this.OnItemSelected != null)
                {
                    this.OnItemSelected(merged[this.selectionIndex]);
                }
            }
        }

        /// <summary>
        /// Draws the scroll window on the console.
        /// </summary>
        public override void Draw()
        {
            // Clear the window.
            string clear = string.Empty;
            int offY = 0;

            for (int i = 0; i < this.Width; i++)
            {
                clear += " ";
            }

            for (int i = 0; i <= this.Height; i++)
            {
                Console.SetCursorPosition(this.X, this.Y + i);

                Console.Write(clear);
            }

            // Draw all fixed items.
            for (int i = 0; i < this.fixedItems.Count; i++)
            {
                Console.SetCursorPosition(this.X, this.Y + offY);

                if (i == this.selectionIndex)
                {
                    this.itemRenderer.DrawSelected(this.X, this.Y + offY, this.Width, this.itemHeight, this.fixedItems[i]);
                }
                else
                {
                    this.itemRenderer.Draw(this.X, this.Y + offY, this.Width, this.itemHeight, this.fixedItems[i]);
                }

                offY += this.itemHeight;
            }

            // Draw all scrollable items, which can be displayed on the remaining space of the window.
            int tempIndex = this.selectionIndex;
            offY = this.fixedItems.Count * this.itemHeight;

            for (int i = 0; i < this.scrollableItems.Count; i++)
            {
                if (((tempIndex + 1) * this.itemHeight) - 1 > this.Height)
                {
                    tempIndex--;
                }
                else if (offY + this.itemHeight - 1 <= this.Height)
                {
                    Console.SetCursorPosition(this.X, this.Y + offY);

                    if (i == this.selectionIndex - this.fixedItems.Count)
                    {
                        this.itemRenderer.DrawSelected(this.X, this.Y + offY, this.Width, this.itemHeight, this.scrollableItems[i]);
                    }
                    else
                    {
                        this.itemRenderer.Draw(this.X, this.Y + offY, this.Width, this.itemHeight, this.scrollableItems[i]);
                    }

                    offY += this.itemHeight;
                }
            }
        }
    }
}
