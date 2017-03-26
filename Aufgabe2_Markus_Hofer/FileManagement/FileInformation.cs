//-----------------------------------------------------------------------
// <copyright file="FileInformation.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class contains properties about a file object.</summary>
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
    /// This class contains properties about a file object.
    /// </summary>
    public class FileInformation
    {
        /// <summary> Gets the size of the file object. </summary>
        /// <value> The size of the file object. </value>
        public long Size { get; private set; }

        /// <summary> Gets the creation time of the file object. </summary>
        /// <value> The creation time of the file object. </value>
        public DateTime CreationTime { get; private set; }

        /// <summary> Gets the last modified time of the file object. </summary>
        /// <value> The last modified time of the file object. </value>
        public DateTime LastModifiedTime { get; private set; }

        /// <summary> Gets the attributes of the file object. </summary>
        /// <value> The attributes of the file object. </value>
        public List<FileAttributes> Attributes { get; private set; }

        /// <summary>
        /// Returns new instance of the file information class with empty values.
        /// </summary>
        /// <returns>A new, empty instance of the file information.</returns>
        public static FileInformation Empty()
        {
            FileInformation info = new FileInformation();

            info.Size = 0;
            info.CreationTime = new DateTime();
            info.LastModifiedTime = new DateTime();
            info.Attributes = new List<FileAttributes>();

            return info;
        }

        /// <summary>
        /// Takes the given file object and tries to gather all needed information about it.
        /// </summary>
        /// <param name="fileObject">The given file object.</param>
        /// <param name="messages">A list of messages, which will be added in case of some error.</param>
        /// <returns>A new Instance of the file information class.</returns>
        public static FileInformation GatherInformationAboutFileObject(IFileObject fileObject, List<InfoMessage> messages)
        {
            FileInformation info = Empty();

            try
            {
                info.CreationTime = fileObject.GetCreationTime();

                info.LastModifiedTime = fileObject.GetLastModifiedTime();

                info.Attributes = fileObject.GetAttributes();

                info.Size = fileObject.GetSizeInBytes();
            }
            catch (Exception e)
            {
                if (e is UnauthorizedAccessException)
                {
                    messages.Add(new InfoMessage(InfoMessage.MessageType.Negative, InfoMessage.MessageCode.Restricted_access));
                }
                else
                {
                    messages.Add(new InfoMessage(InfoMessage.MessageType.Negative, InfoMessage.MessageCode.Unknown_error));
                }
            }

            return info;
        }
    }
}
