//-----------------------------------------------------------------------
// <copyright file="InfoWindow.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a window which is displaying some information about a file or a folder.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a window which is displaying some information about a file or a folder.
    /// </summary>
    public class InfoWindow : DrawableObject
    {
        /// <summary> List of descriptions for each information. </summary>
        private readonly string[] descriptions = new string[]
        {
            "Size           ",
            "Created        ",
            "Last modified  ",
            "Attributes     ",
            "               "
        };

        /// <summary> General information about a file or a folder. </summary>
        private FileInformation fileInformation;

        /// <summary> Messages, which have occurred while interacting with a certain file or folder. </summary>
        private List<InfoMessage> infoMessages;

        /// <summary> The content of a text file. </summary>
        private string[] fileContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoWindow"/> class.
        /// </summary>
        public InfoWindow()
        {
        }

        /// <summary> Gets a value indicating whether the window shows the content of a text file or not. </summary>
        /// <value> A value indicating whether the window shows the content of a text file or not. </value>
        public bool IsDisplayingContent { get; private set; }

        /// <summary>
        /// Tells the window if he must show the file content or not.
        /// </summary>
        /// <param name="displaying">If it's true the content will be shown.</param>
        public void EnableContentDisplaying(bool displaying)
        {
            this.IsDisplayingContent = displaying;
        }

        /// <summary>
        /// Sets the general information about a file or a folder.
        /// </summary>
        /// <param name="info">Information about a file object.</param>
        public void SetFileInformation(FileInformation info)
        {
            this.fileInformation = info;
            this.fileContent = null;
        }

        /// <summary>
        /// Sets a list of messages which have occurred while interacting with a certain file object.
        /// </summary>
        /// <param name="messages">The list of info messages.</param>
        public void SetInfoMessages(List<InfoMessage> messages)
        {
            this.infoMessages = messages;
        }

        /// <summary>
        /// Sets the content of a text file.
        /// </summary>
        /// <param name="content">The content represented as an array of lines.</param>
        public void SetFileContent(string[] content)
        {
            this.fileContent = content;
        }

        /// <summary>
        /// Draws the information window on the console.
        /// </summary>
        public override void Draw()
        {
            // Clear the window.
            string clear = string.Empty;

            for (int i = 0; i < this.Width; i++)
            {
                clear += " ";
            }

            for (int i = 0; i <= this.Height; i++)
            {
                Console.SetCursorPosition(this.X, this.Y + i);

                Console.Write(clear);
            }

            int offY = 0;

            if (this.fileContent != null && this.IsDisplayingContent)
            {
                this.DrawFileContent(this.fileContent);
            }
            else
            {
                if (this.fileInformation != null)
                {
                    this.DrawFileObjectInformation(this.fileInformation, ref offY);
                }

                offY++;

                if (this.infoMessages != null)
                {
                    foreach (InfoMessage info in this.infoMessages)
                    {
                        this.DrawInfoMessage(info, ref offY);
                    }
                }
            }

            Console.SetCursorPosition(this.X, this.Y + offY);
        }

        /// <summary>
        /// Draws the general file information on the console.
        /// </summary>
        /// <param name="info">Information about a file object.</param>
        /// <param name="offY">Offset of the Y - coordinate.</param>
        private void DrawFileObjectInformation(FileInformation info, ref int offY)
        {
            // Gather the information
            List<string> content = new List<string>();

            content.Add(FileInfoHelper.GetFormattedSizeWithSuffix(info.Size));

            content.Add(FileInfoHelper.GetFormattedDate(info.CreationTime));

            content.Add(FileInfoHelper.GetFormattedDate(info.LastModifiedTime));

            List<FileAttributes> attributes = info.Attributes;

            foreach (FileAttributes attribute in attributes)
            {
                content.Add(attribute.ToString());
            }

            // Print the information
            for (int i = 0; i < content.Count && this.Y + i <= this.Height + 1; i++)
            {
                // If there is no description left for the values, take the last one.
                string description = this.descriptions[this.descriptions.Length - 1];

                if (i < this.descriptions.Length)
                {
                    description = this.descriptions[i];
                }

                Console.SetCursorPosition(this.X + 1, this.Y + offY);

                // If there is not enough place for the description, truncate it.
                if (description.Length >= this.Width)
                {
                    description = description.Substring(0, this.Width - 1);
                }

                Console.Write(description);

                // Check if there is not enough place for the description and the information.
                if (description.Length + content[i].Length + 2 >= this.Width)
                {
                    // There is still enough place for the information, but the description has to be overlapped.
                    if (content[i].Length + 2 <= this.Width)
                    {
                        Console.SetCursorPosition(this.X + this.Width - content[i].Length - 2, this.Y + offY);

                        Console.Write(": {0}", content[i]);
                    }
                    else
                    {
                        // The description will be completely hidden and the information will be truncatted.
                        Console.SetCursorPosition(this.X + 1, this.Y + offY);

                        Console.Write(content[i].Substring(0, this.Width - 1));
                    }
                }
                else
                {
                    Console.Write(": {0}", content[i]);
                }

                offY++;
            }
        }

        /// <summary>
        /// Draws the messages on the console.
        /// </summary>
        /// <param name="message">A list of messages.</param>
        /// <param name="offY">Offset of the Y - coordinate.</param>
        private void DrawInfoMessage(InfoMessage message, ref int offY)
        {
            string text = InfoMessage.GetTextFromCode(message.Code);

            ConsoleColor tempF = Console.ForegroundColor;

            switch (message.Type)
            {
                case InfoMessage.MessageType.Positive:
                    Console.ForegroundColor = ConsoleColor.Green;

                    break;
                case InfoMessage.MessageType.Negative:
                    Console.ForegroundColor = ConsoleColor.Red;

                    break;
            }

            while (text.Length > 0 && this.Y + offY <= this.Height + 1)
            {
                Console.SetCursorPosition(this.X + 1, this.Y + offY);

                if (text.Length < this.Width)
                {
                    Console.Write(text);
                    text = string.Empty;
                }
                else
                {
                    Console.Write(text.Substring(0, this.Width - 1));
                    text = text.Substring(this.Width - 1, text.Length - this.Width + 1);
                }

                offY++;
            }

            Console.ForegroundColor = tempF;
        }

        /// <summary>
        /// Draws the content of a text file on the console.
        /// </summary>
        /// <param name="content">The content represented as an array of lines.</param>
        private void DrawFileContent(string[] content)
        {
            string[] cropped = FileInfoHelper.CropText(this.fileContent, this.Width, this.Height + 1);

            for (int i = 0; i < cropped.Length; i++)
            {
                Console.SetCursorPosition(this.X, this.Y + i);
                Console.Write(cropped[i]);
            }
        }
    }
}
