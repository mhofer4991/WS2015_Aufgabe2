//-----------------------------------------------------------------------
// <copyright file="Main.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents the main program, where the user can interact with the file manager.</summary>
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
    /// This class represents the main program, where the user can interact with the file manager.
    /// </summary>
    public class Main
    {
        /// <summary> Represents the left window of the console application. </summary>
        private WindowFrame leftFrame;

        /// <summary> Represents the right window of the console application. </summary>
        private WindowFrame rightFrame;

        /// <summary> A scrollable window, which contains items of the file manager. </summary>
        private ScrollableWindow fileBrowser;

        /// <summary> A scrollable window, which contains all available assignable keys. </summary>
        private ScrollableWindow keysWindow;

        /// <summary> A scrollable window, which contains all functions assignable to the keys. </summary>
        private ScrollableWindow functionsWindow;

        /// <summary> Represents the bottom menu bar, which shows available commands. </summary>
        private MenuBar menuBar;

        /// <summary> Represents the info window on the right side, which shows information to a specific file object. </summary>
        private InfoWindow infoWindow;

        /// <summary> The user can assign the functions to the keys with the configurator. </summary>
        private Configurator configurator;

        /// <summary> Watches the state of the console window and fires events if something changes. </summary>
        private WindowStateWatcher windowStateWatcher;

        /// <summary> Contains the current assignments of key to function. </summary>
        private Dictionary<KeyFunction, ConsoleKey> currentKeyAssignments;
        
        /// <summary> Contains the current available commands. </summary>
        private Dictionary<KeyFunction, ConsoleKey> currentAvailableKeyAssignments;

        /// <summary> The file manager manages files. </summary>
        private FileManager manager;

        /// <summary> Indicates whether the browser should be drawn or not. </summary>
        private bool browserActive;

        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            this.leftFrame = new WindowFrame();
            this.rightFrame = new WindowFrame();

            this.fileBrowser = new ScrollableWindow(new FileObjectScrollItemRenderer());
            this.infoWindow = new InfoWindow();

            this.keysWindow = new ScrollableWindow(new DefaultScrollItemRenderer());
            this.functionsWindow = new ScrollableWindow(new DefaultScrollItemRenderer());

            this.menuBar = new MenuBar();

            this.configurator = new Configurator(this.keysWindow, this.functionsWindow);

            this.windowStateWatcher = new WindowStateWatcher();

            this.manager = new FileManager();
        }

        /// <summary>
        /// Starts the main program.
        /// </summary>
        public void Start()
        {
            this.StartFileBrowser(this.GetStartFolder());
        }

        /// <summary>
        /// Starts the file browser beginning at the given start folder.
        /// </summary>
        /// <param name="startFolder">The given start folder.</param>
        public void StartFileBrowser(IFolder startFolder)
        {
            Console.SetBufferSize(4000, 4000);

            this.windowStateWatcher.OnWindowSizeChanged += this.WindowStateWatcher_OnWindowSizeChanged;
            this.configurator.OnNewSettingsApplied += this.Configurator_OnNewSettingsApplied;

            this.fileBrowser.OnItemSelected += this.FileBrowser_OnItemSelected;
            this.fileBrowser.OnItemClicked += this.FileBrowser_OnItemClicked;

            this.windowStateWatcher.Start();

            this.currentKeyAssignments = Main.GetDefaultKeyAssignments();
            this.currentAvailableKeyAssignments = new Dictionary<KeyFunction, ConsoleKey>(this.currentKeyAssignments);

            this.menuBar.SetKeyAssignments(KeyAssignment.ConvertToList(this.currentAvailableKeyAssignments));

            this.browserActive = true;
            this.rightFrame.SetTitle("Details");

            this.AdjustToConsoleSize();
            this.Draw();

            this.ShowContent(startFolder);

            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);

                this.HandleInput(cki);
                this.fileBrowser.HandleKey(cki);
            }
        }

        /// <summary>
        /// Gets the default key function assignment of the main program.
        /// </summary>
        /// <returns>A dictionary representing the default key function assignment.</returns>
        private static Dictionary<KeyFunction, ConsoleKey> GetDefaultKeyAssignments()
        {
            Dictionary<KeyFunction, ConsoleKey> assignments = new Dictionary<KeyFunction, ConsoleKey>();

            assignments.Add(KeyFunction.Config, ConsoleKey.F2);
            assignments.Add(KeyFunction.Copy, ConsoleKey.F3);
            assignments.Add(KeyFunction.Cut, ConsoleKey.F4);
            assignments.Add(KeyFunction.Paste, ConsoleKey.F5);
            assignments.Add(KeyFunction.Mkdir, ConsoleKey.F6);
            assignments.Add(KeyFunction.Delete, ConsoleKey.F7);
            assignments.Add(KeyFunction.Toggle, ConsoleKey.F8);
            assignments.Add(KeyFunction.Quit, ConsoleKey.F9);

            return assignments;
        }
        
        /// <summary>
        /// Gets called when the user clicks on a scroll item.
        /// </summary>
        /// <param name="info">Information about the item.</param>
        private void FileBrowser_OnItemClicked(ScrollItemInformation info)
        {
            this.ClickScrollItem(info);
        }

        /// <summary>
        /// Gets called when the user selects a scroll item during navigation.
        /// </summary>
        /// <param name="info">Information about the item.</param>
        private void FileBrowser_OnItemSelected(ScrollItemInformation info)
        {
            this.SelectScrollItem(info);
        }

        /// <summary>
        /// Handles the scroll item by showing the content of the file object.
        /// </summary>
        /// <param name="item">Information about the item.</param>
        private void ClickScrollItem(ScrollItemInformation item)
        {
            this.infoWindow.SetFileInformation(null);
            this.infoWindow.SetInfoMessages(null);

            if (item.Attachment is IFolder)
            {
                this.ShowContent((IFolder)item.Attachment);
            }
            else if (item.Attachment is Link)
            {
                this.ShowContent((IFolder)((Link)item.Attachment).Target);
            }
        }

        /// <summary>
        /// Handles the scroll item by showing information about the file object.
        /// </summary>
        /// <param name="item">Information about the item.</param>
        private void SelectScrollItem(ScrollItemInformation item)
        {
            this.infoWindow.SetFileInformation(null);
            this.infoWindow.SetInfoMessages(null);

            if (item != null)
            {
                if (!(item.Attachment is Link))
                {
                    if (!(item.Attachment is DriveContainer) && !(this.manager.CurrentFolder is DriveContainer))
                    {
                        this.manager.SetCurrentObject((IFileObject)item.Attachment);

                        List<InfoMessage> messages = new List<InfoMessage>();

                        this.infoWindow.SetFileInformation(this.manager.GetInformationAboutCurrentObject(messages));

                        if (item.Attachment is IFile && ((IFile)item.Attachment).GetExtension().Equals(".txt"))
                        {
                            byte[] bytes = FileManager.GetContentOfFile((IFile)item.Attachment, messages);
                            string[] lines = Encoding.ASCII.GetString(bytes).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                            this.infoWindow.SetFileContent(lines);
                        }

                        this.infoWindow.SetInfoMessages(messages);
                    }
                }
            }

            this.ShowMenuForScrollItem(item);

            this.infoWindow.Draw();
        }

        /// <summary>
        /// Shows content of a given folder in the file browser.
        /// </summary>
        /// <param name="folder">The given folder.</param>
        private void ShowContent(IFolder folder)
        {
            // Clear the browser.
            this.fileBrowser.Reset();

            this.manager.SetCurrentFolder(folder);

            // Add link to the parent folder.
            if (folder.GetParent() != null)
            {
                Link link = new Link("..", folder.GetParent());

                this.fileBrowser.AddFixedItem(new ScrollItemInformation(link.Name, link));
            }

            // Get content of current folder.
            List<InfoMessage> messages = new List<InfoMessage>();

            List<IFileObject> content = this.manager.GetContentOfCurrentFolder(messages);

            foreach (IFileObject fileObject in content)
            {
                this.fileBrowser.AddScrollableItem(new ScrollItemInformation(fileObject.GetName(), fileObject));
            }

            this.infoWindow.SetInfoMessages(messages);

            // Display information.
            this.leftFrame.SetTitle(folder.GetName());

            this.ShowMenuForScrollItem(this.fileBrowser.GetCurrentlySelectedItem());
            this.SelectScrollItem(this.fileBrowser.GetCurrentlySelectedItem());

            this.leftFrame.Draw();
            this.fileBrowser.Draw();

            this.infoWindow.Draw();
        }

        /// <summary>
        /// Show all commands, which are currently available according to the selected file object.
        /// </summary>
        /// <param name="info">Information about the item.</param>
        private void ShowMenuForScrollItem(ScrollItemInformation info)
        {
            this.currentAvailableKeyAssignments = new Dictionary<KeyFunction, ConsoleKey>(this.currentKeyAssignments);

            if (info == null)
            {
                this.currentAvailableKeyAssignments = new Dictionary<KeyFunction, ConsoleKey>();
            }
            else if (info.Attachment is Link || info.Attachment is DriveContainer || this.manager.CurrentFolder is DriveContainer)
            {
                this.currentAvailableKeyAssignments = new Dictionary<KeyFunction, ConsoleKey>();

                this.currentAvailableKeyAssignments.Add(KeyFunction.Config, this.currentKeyAssignments[KeyFunction.Config]);
                this.currentAvailableKeyAssignments.Add(KeyFunction.Quit, this.currentKeyAssignments[KeyFunction.Quit]);
                this.currentAvailableKeyAssignments.Add(KeyFunction.Paste, this.currentKeyAssignments[KeyFunction.Paste]);

                if (!(this.manager.CurrentFolder is DriveContainer))
                {
                    this.currentAvailableKeyAssignments.Add(KeyFunction.Mkdir, this.currentKeyAssignments[KeyFunction.Mkdir]);
                }
            }
            else
            {
                if (!(info.Attachment is IFile && ((IFile)info.Attachment).GetExtension().Equals(".txt")))
                {
                    this.currentAvailableKeyAssignments.Remove(KeyFunction.Toggle);
                }
            }

            if (this.manager.HasEmptyClipboard())
            {
                this.currentAvailableKeyAssignments.Remove(KeyFunction.Paste);
            }
            else
            {
                // The commands Copy and Cut are not available if the user already copied / cut a file / folder.
                this.currentAvailableKeyAssignments.Remove(KeyFunction.Copy);
                this.currentAvailableKeyAssignments.Remove(KeyFunction.Cut);
            }

            this.menuBar.SetKeyAssignments(KeyAssignment.ConvertToList(this.currentAvailableKeyAssignments));
            this.menuBar.Draw();
        }

        /// <summary>
        /// Handles the input of the user.
        /// </summary>
        /// <param name="cki">Information about the user input.</param>
        private void HandleInput(ConsoleKeyInfo cki)
        {
            foreach (KeyFunction function in this.currentAvailableKeyAssignments.Keys)
            {
                if (this.currentAvailableKeyAssignments.ContainsKey(function) && this.currentAvailableKeyAssignments[function] == cki.Key)
                {
                    List<InfoMessage> messages = new List<InfoMessage>();

                    switch (function)
                    {
                        case KeyFunction.Config:
                            this.menuBar.SetKeyAssignments(KeyAssignment.ConvertToList(Configurator.GetDefaultKeyAssignments()));
                            this.menuBar.Draw();

                            this.browserActive = false;
                            this.leftFrame.SetTitle("Keys");
                            this.rightFrame.SetTitle("Functions");
                            this.configurator.RequestNewConfiguration();

                            this.browserActive = true;
                            this.rightFrame.SetTitle("Details");

                            this.menuBar.SetKeyAssignments(KeyAssignment.ConvertToList(this.currentAvailableKeyAssignments));

                            this.Draw();

                            break;
                        case KeyFunction.Copy:
                            this.manager.AddFileObjectToClipboard(this.manager.CurrentSelectedObject, false);

                            this.ShowMenuForScrollItem(this.fileBrowser.GetCurrentlySelectedItem());

                            break;
                        case KeyFunction.Cut:                            
                            this.manager.AddFileObjectToClipboard(this.manager.CurrentSelectedObject, true);

                            this.fileBrowser.RemoveItem(this.fileBrowser.GetCurrentlySelectedItem());

                            this.SelectScrollItem(this.fileBrowser.GetCurrentlySelectedItem());

                            this.fileBrowser.Draw();

                            this.ShowMenuForScrollItem(this.fileBrowser.GetCurrentlySelectedItem());

                            break;
                        case KeyFunction.Paste:
                            IFileObject pasted = this.manager.PasteObjectIntoCurrentFolder(messages);

                            if (pasted != null)
                            {
                                List<IFileObject> content = this.manager.GetContentOfCurrentFolder(messages);

                                int index = content.IndexOf(pasted);

                                this.fileBrowser.AddScrollableItem(new ScrollItemInformation(pasted.GetName(), pasted), index);

                                this.fileBrowser.Draw();
                            }

                            this.ShowMenuForScrollItem(this.fileBrowser.GetCurrentlySelectedItem());

                            break;
                        case KeyFunction.Mkdir:
                            this.windowStateWatcher.Stop();

                            string folderName = this.GetUserInput("Name: ", Console.WindowWidth / 2);

                            this.windowStateWatcher.Start();

                            IFolder created = this.manager.MakeDirectoryInCurrentFolder(folderName, messages);

                            if (created != null)
                            {
                                List<IFileObject> content = this.manager.GetContentOfCurrentFolder(messages);

                                int index = content.IndexOf(created);

                                this.fileBrowser.AddScrollableItem(new ScrollItemInformation(created.GetName(), created), index);
                            }

                            this.Draw();

                            break;
                        case KeyFunction.Delete:
                            if (this.manager.DeleteCurrentObject(messages))
                            {
                                this.fileBrowser.RemoveItem(this.fileBrowser.GetCurrentlySelectedItem());

                                this.SelectScrollItem(this.fileBrowser.GetCurrentlySelectedItem());

                                this.fileBrowser.Draw();
                            }

                            break;
                        case KeyFunction.Toggle:
                            this.infoWindow.EnableContentDisplaying(!this.infoWindow.IsDisplayingContent);

                            this.SelectScrollItem(this.fileBrowser.GetCurrentlySelectedItem());

                            break;
                        case KeyFunction.Quit:
                            if (this.manager.CurrentFolder is SimulatedFolder && !(this.manager.CurrentFolder is DriveContainer))
                            {
                                IFolder root = this.manager.CurrentFolder;

                                while (root.GetParent() != null)
                                {
                                    root = root.GetParent();
                                }

                                SimulatedFolder.Serialize((SimulatedFolder)root, Environment.CurrentDirectory, root.GetName());
                            }

                            Environment.Exit(0);

                            break;
                    }

                    this.infoWindow.SetInfoMessages(messages);
                    this.infoWindow.Draw();
                }
            }
        }

        /// <summary>
        /// Gets called when the user successfully assigned all keys to functions.
        /// </summary>
        /// <param name="settings">The new commands setting.</param>
        private void Configurator_OnNewSettingsApplied(Dictionary<KeyFunction, ConsoleKey> settings)
        {
            this.currentKeyAssignments = settings;
            Dictionary<KeyFunction, ConsoleKey> temp = new Dictionary<KeyFunction, ConsoleKey>(this.currentAvailableKeyAssignments);

            foreach (KeyFunction function in this.currentAvailableKeyAssignments.Keys)
            {
                temp[function] = this.currentKeyAssignments[function];
            }

            this.currentAvailableKeyAssignments = temp;
        }

        /// <summary>
        /// Gets called when the size of the console gets changed.
        /// </summary>
        /// <param name="oldWindowWidth">The old width of the console window.</param>
        /// <param name="oldWindowHeight">The old height of the console window.</param>
        private void WindowStateWatcher_OnWindowSizeChanged(int oldWindowWidth, int oldWindowHeight)
        {
            this.AdjustToConsoleSize();
            this.Draw();
        }

        /// <summary>
        /// Adjusts all elements like scroll window to the console window size.
        /// </summary>
        private void AdjustToConsoleSize()
        {
            this.leftFrame.SetPosition(0, 0);
            this.leftFrame.SetSize(Console.WindowWidth / 2, Console.WindowHeight - 4);

            this.rightFrame.SetPosition(this.leftFrame.X + this.leftFrame.Width, this.leftFrame.Y);
            this.rightFrame.SetSize(this.leftFrame.Width, this.leftFrame.Height);

            this.fileBrowser.SetPosition(this.leftFrame.X + 1, this.leftFrame.Y + 1);
            this.fileBrowser.SetSize(this.leftFrame.Width - 2, this.leftFrame.Height - 2);

            this.infoWindow.SetPosition(this.rightFrame.X + 1, this.rightFrame.Y + 1);
            this.infoWindow.SetSize(this.rightFrame.Width - 2, this.rightFrame.Height - 2);

            this.keysWindow.SetPosition(this.leftFrame.X + 1, this.leftFrame.Y + 1);
            this.keysWindow.SetSize(this.leftFrame.Width - 2, this.leftFrame.Height - 2);

            this.functionsWindow.SetPosition(this.rightFrame.X + 1, this.rightFrame.Y + 1);
            this.functionsWindow.SetSize(this.rightFrame.Width - 2, this.rightFrame.Height - 2);

            this.menuBar.SetPosition(0, this.leftFrame.Y + this.leftFrame.Height + 1);
            this.menuBar.SetSize(Console.WindowWidth, 3);
        }

        /// <summary>
        /// Draws the main program on the console window.
        /// </summary>
        private void Draw()
        {
            Console.Clear();

            this.leftFrame.Draw();
            this.rightFrame.Draw();

            if (this.browserActive)
            {
                this.fileBrowser.Draw();
                this.infoWindow.Draw();
            }
            else
            {
                this.keysWindow.Draw();
                this.functionsWindow.Draw();
            }

            this.menuBar.Draw();
        }   
        
        /// <summary>
        /// Prompts the user to enter some text.
        /// </summary>
        /// <param name="inputText">Describes the input box.</param>
        /// <param name="width">The width of the input box. The height is always 1.</param>
        /// <returns>A string representing the text the user entered.</returns>
        private string GetUserInput(string inputText, int width)
        {
            Console.Clear();

            WindowFrame frame = new WindowFrame();
            frame.SetSize(width, 2);
            frame.SetPosition((Console.WindowWidth - width) / 2, Console.WindowHeight / 2);
            frame.SetTitle(inputText);

            frame.Draw();

            Console.SetCursorPosition(frame.X + 1, frame.Y + 1);

            string input = Console.ReadLine();

            return input;
        }

        /// <summary>
        /// Gets the start folder for the file browser by prompting the user.
        /// </summary>
        /// <returns>The start folder.</returns>
        private IFolder GetStartFolder()
        {
            WindowFrame frame = new WindowFrame();
            frame.SetSize(Console.WindowWidth / 2, 5);
            frame.SetPosition(Console.WindowWidth / 4, Console.WindowHeight / 2);
            frame.SetTitle("Select a mode: ");

            ScrollableWindow scrollWindow = new ScrollableWindow(new DefaultScrollItemRenderer());
            scrollWindow.SetSize(frame.Width - 2, 2);
            scrollWindow.SetPosition(frame.X + 1, frame.Y + 1);

            scrollWindow.AddScrollableItem(new ScrollItemInformation("Real mode", 1));
            scrollWindow.AddScrollableItem(new ScrollItemInformation("Simulated mode", 2));

            frame.Draw();
            scrollWindow.Draw();

            if (scrollWindow.WaitForItemClick())
            {
                ScrollItemInformation info = scrollWindow.GetCurrentlySelectedItem();

                // User choosed real mode.
                if ((int)info.Attachment == 1)
                {
                    return RealFolder.GetRoot();
                }
                else
                {
                    // User choosed simulated mode. Are there any serialized simulated folders saved?
                    SimulatedFolder folder = SimulatedFolder.DeserializeExistingFolder(Environment.CurrentDirectory);

                    if (folder == null)
                    {
                        // No. Prompt the user the enter the path where the program will begin to generate a simulated directory structure.
                        bool error = false;
                        string path;

                        do
                        {
                            if (!error)
                            {
                                path = this.GetUserInput("Path where the scan will begin: ", Console.WindowWidth - 2);
                            }
                            else
                            {
                                path = this.GetUserInput("An error occurred! Please try it again: ", Console.WindowWidth - 2);
                            }

                            try
                            {
                                folder = SimulatedFolder.GenerateDirectoryStructure(path);

                                error = false;
                            }
                            catch (Exception)
                            {
                                error = true;
                            }
                        }
                        while (error);
                    }

                    return folder;
                }
            }

            return null;
        }
    }
}
