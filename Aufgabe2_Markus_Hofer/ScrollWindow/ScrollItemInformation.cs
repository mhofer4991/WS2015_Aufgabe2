//-----------------------------------------------------------------------
// <copyright file="ScrollItemInformation.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class contains information of an item of a scroll window.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class contains information of an item of a scroll window.
    /// </summary>
    public class ScrollItemInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollItemInformation"/> class.
        /// </summary>
        /// <param name="text">The text of the scroll item.</param>
        public ScrollItemInformation(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollItemInformation"/> class.
        /// </summary>
        /// <param name="text">The text of the scroll item.</param>
        /// <param name="attachment">The attachment of the scroll item.</param>
        public ScrollItemInformation(string text, object attachment)
        {
            this.Text = text;
            this.Attachment = attachment;
        }

        /// <summary> Gets the text of the scroll item. </summary>
        /// <value> The text of the scroll item. </value>
        public string Text { get; private set; }

        /// <summary> Gets the attachment of the scroll item. </summary>
        /// <value> The attachment of the scroll item. </value>
        public object Attachment { get; private set; }

        /// <summary>
        /// Sets the attachment of the scroll item, which will be delivered when the program interacts with this item.
        /// </summary>
        /// <param name="attachment">The attachment of the scroll item.</param>
        public void SetAttachment(object attachment)
        {
            this.Attachment = attachment;
        }
    }
}
