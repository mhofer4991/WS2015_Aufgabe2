//-----------------------------------------------------------------------
// <copyright file="FileManager.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class provides methods to manage folders and it's containing files.</summary>
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
    /// This class provides methods to manage folders and it's containing files.
    /// </summary>
    public class FileManager
    {
        /// <summary> Will be used if the user copies or cuts a file object. </summary>
        private IFileObject clipboard;

        /// <summary> Will be true if the user wants to cut the file in the clipboard. </summary>
        private bool cutting;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileManager"/> class.
        /// </summary>
        public FileManager()
        {
        }

        /// <summary> Gets the file object, which is currently hold by the file manager. </summary>
        /// <value> The file object, which is currently hold by the file manager. </value>
        public IFileObject CurrentSelectedObject { get; private set; }

        /// <summary> Gets the folder where the manager is currently operating. </summary>
        /// <value> The folder where the manager is currently operating. </value>
        public IFolder CurrentFolder { get; private set; }

        /// <summary>
        /// Gets the content of a file as an array of bytes.
        /// </summary>
        /// <param name="file">The given file.</param>
        /// <param name="messages">A list of messages, which will be added in case of some error.</param>
        /// <returns>The content represented as an array of bytes.</returns>
        public static byte[] GetContentOfFile(IFile file, List<InfoMessage> messages)
        {
            byte[] content = new byte[0];

            try
            {
                content = file.GetContent();
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

            return content;
        }

        /// <summary>
        /// Sets the current file object.
        /// </summary>
        /// <param name="currentObject">The current file object.</param>
        public void SetCurrentObject(IFileObject currentObject)
        {
            this.CurrentSelectedObject = currentObject;
        }

        /// <summary>
        /// Sets the current folder, which contains the current file object.
        /// </summary>
        /// <param name="folder">The current folder.</param>
        public void SetCurrentFolder(IFolder folder)
        {
            this.CurrentFolder = folder;
        }

        /// <summary>
        /// Adds a file object to the clipboard.
        /// </summary>
        /// <param name="item">The file object which will be added to the clipboard.</param>
        /// <param name="cutting">Indicates whether the object will be cut or not..</param>
        public void AddFileObjectToClipboard(IFileObject item, bool cutting)
        {
            this.clipboard = item;
            this.cutting = cutting;
        }

        /// <summary>
        /// Checks if the clipboard is empty.
        /// </summary>
        /// <returns>A boolean indicating whether the clipboard is empty or not.</returns>
        public bool HasEmptyClipboard()
        {
            return this.clipboard == null;
        }

        /// <summary>
        /// Gets information about the current file object.
        /// </summary>
        /// <param name="messages">A list of messages, which will be added in case of some error.</param>
        /// <returns>Information about the file object.</returns>
        public FileInformation GetInformationAboutCurrentObject(List<InfoMessage> messages)
        {
            return FileInformation.GatherInformationAboutFileObject(this.CurrentSelectedObject, messages);
        }

        /// <summary>
        /// Gets the content of the current folder.
        /// </summary>
        /// <param name="messages">A list of messages, which will be added in case of some error.</param>
        /// <returns>The content of the current folder as a list of file objects.</returns>
        public List<IFileObject> GetContentOfCurrentFolder(List<InfoMessage> messages)
        {
            List<IFileObject> content = new List<IFileObject>();

            try
            {
                content.AddRange(this.CurrentFolder.GetContent());
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

            return content;
        }

        /// <summary>
        /// Deletes the current file object.
        /// </summary>
        /// <param name="messages">A list of messages, which will be added in case of some error.</param>
        /// <returns>A boolean indicating whether the deletion was successful or not.</returns>
        public bool DeleteCurrentObject(List<InfoMessage> messages)
        {
            try
            {
                if (this.CurrentSelectedObject.Delete())
                {
                    this.CurrentSelectedObject = null;

                    return true;
                }
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

            return false;
        }

        /// <summary>
        /// Pastes the file objects from the clipboard into the current folder.
        /// </summary>
        /// <param name="messages">A list of messages, which will be added in case of some error.</param>
        /// <returns>The new file object, which has been pasted into the current folder.</returns>
        public IFileObject PasteObjectIntoCurrentFolder(List<InfoMessage> messages)
        {
            IFileObject fileObject = null;

            if (this.clipboard.Equals(this.CurrentFolder))
            {
                messages.Add(new InfoMessage(InfoMessage.MessageType.Negative, InfoMessage.MessageCode.Destination_is_source));

                return fileObject;
            }

            if (this.cutting && this.clipboard.GetParent().Equals(this.CurrentFolder))
            {
                fileObject = this.clipboard;

                this.clipboard = null;
                this.cutting = false;

                return fileObject;
            }
                        
            try
            {
                if ((fileObject = this.CurrentFolder.Paste(this.clipboard, this.clipboard.GetName())) != null)
                {
                    if (this.cutting)
                    {
                        this.clipboard.Delete();
                        this.cutting = false;
                    }

                    this.clipboard = null;
                }
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

            return fileObject;
        }

        /// <summary>
        /// Creates a new directory in the current folder.
        /// </summary>
        /// <param name="name">The name of the new directory.</param>
        /// <param name="messages">A list of messages, which will be added in case of some error.</param>
        /// <returns>An instance of the new folder, which has been created.</returns>
        public IFolder MakeDirectoryInCurrentFolder(string name, List<InfoMessage> messages)
        {
            IFolder folder = null;

            try
            {
                folder = this.CurrentFolder.MakeDirectory(name);
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

            return folder;
        }
    }
}
