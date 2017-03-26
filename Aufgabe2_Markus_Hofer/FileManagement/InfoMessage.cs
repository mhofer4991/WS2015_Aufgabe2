//-----------------------------------------------------------------------
// <copyright file="InfoMessage.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a form of feedback from different file operations.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a form of feedback from different file operations.
    /// </summary>
    public class InfoMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoMessage"/> class.
        /// </summary>
        /// <param name="type">The type of the message.</param>
        /// <param name="code">The code of the message.</param>
        public InfoMessage(MessageType type, MessageCode code)
        {
            this.Type = type;
            this.Code = code;
        }

        /// <summary>
        /// This enumeration contains different types of a message.
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// Indicates that the message brings good news.
            /// </summary>
            Positive,

            /// <summary>
            /// Indicates that the message brings bad news.
            /// </summary>
            Negative
        }

        /// <summary>
        /// This enumeration contains different message codes.
        /// </summary>
        public enum MessageCode
        {
            /// <summary>
            /// Indicates that accessing some file or folder failed.
            /// </summary>
            Restricted_access,

            /// <summary>
            /// Indicates that the user tried to paste a copy of a folder into this folder.
            /// </summary>
            Destination_is_source,

            /// <summary>
            /// Indicates that the reason for the error is not known.
            /// </summary>
            Unknown_error
        }

        /// <summary> Gets the type of the info message. </summary>
        /// <value> The type of the info message. </value>
        public MessageType Type { get; private set; }

        /// <summary> Gets the code of the info message. </summary>
        /// <value> The code of the info message. </value>
        public MessageCode Code { get; private set; }

        /// <summary>
        /// Gets the describing text to the given message code.
        /// </summary>
        /// <param name="code">The message code.</param>
        /// <returns>The describing text to the given message code.</returns>
        public static string GetTextFromCode(MessageCode code)
        {
            string text = string.Empty;

            switch (code)
            {
                case InfoMessage.MessageCode.Restricted_access:
                    text = "The access to this file / folder or it's subfolders is restricted!";

                    break;
                case MessageCode.Destination_is_source:
                    text = "The destination folder is the source folder!";

                    break;
                default:
                    text = "An unknown error occurred while accessing / creating this file / folder.";

                    break;
            }

            return text;
        }
    }
}
