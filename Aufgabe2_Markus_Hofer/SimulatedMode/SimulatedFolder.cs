//-----------------------------------------------------------------------
// <copyright file="SimulatedFolder.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a folder, which only exists virtually in the memory.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe2_Markus_Hofer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a folder, which only exists virtually in the memory.
    /// </summary>
    [Serializable]
    public class SimulatedFolder : SimulatedFileObject, IFolder
    {
        /// <summary> A list of all children folders. </summary>
        private List<IFileObject> childrenFolders;

        /// <summary> A list of all children files. </summary>
        private List<IFileObject> childrenFiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedFolder"/> class.
        /// </summary>
        /// <param name="name">The name of the folder.</param>
        /// <param name="attributes">The attributes of the folder.</param>
        public SimulatedFolder(string name, List<FileAttributes> attributes) : base(name, attributes)
        {
            this.childrenFolders = new List<IFileObject>();
            this.childrenFiles = new List<IFileObject>();
        }

        /// <summary>
        /// Generates a treelike directory structure by starting at the given path and scanning all containing files and folders.
        /// </summary>
        /// <param name="path">The given path where the scanning begins.</param>
        /// <returns>A folder, which represents the root of the directory structure.</returns>
        public static SimulatedFolder GenerateDirectoryStructure(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);

            SimulatedFolder root = new SimulatedFolder(info.Name, FileInfoHelper.GetAttributesOfFile(path));

            root.SetCreationTime(FileInfoHelper.GetCreationDate(info.FullName));
            root.SetLastModifiedTime(FileInfoHelper.GetLastModifiedDate(info.FullName));

            root.SetParent(null);
            root.SetChildren(SimulatedFolder.GetContent(path, root));

            return root;
        }

        /// <summary>
        /// Generates a list of all file objects, which are found at the given path and it's subfolders.
        /// </summary>
        /// <param name="path">The path, where the search begins.</param>
        /// <param name="parent">The folder, which contains all the file objects.</param>
        /// <returns>A list of all file objects, which are found at the given path.</returns>
        public static List<IFileObject> GetContent(string path, SimulatedFolder parent)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            List<IFileObject> content = new List<IFileObject>();

            DirectoryInfo[] folders = new DirectoryInfo[0];

            try
            {
                folders = info.GetDirectories();
            }
            catch (UnauthorizedAccessException)
            {
            }

            foreach (DirectoryInfo folder in folders)
            {
                SimulatedFolder f = new SimulatedFolder(folder.Name, FileInfoHelper.GetAttributesOfFile(folder.FullName));

                f.SetCreationTime(FileInfoHelper.GetCreationDate(folder.FullName));
                f.SetLastModifiedTime(FileInfoHelper.GetLastModifiedDate(folder.FullName));

                f.SetParent(parent);
                f.SetChildren(SimulatedFolder.GetContent(folder.FullName, f));

                content.Add(f);
            }

            FileInfo[] files = new FileInfo[0];

            try
            {
                files = info.GetFiles();
            }
            catch (UnauthorizedAccessException)
            {
            }

            foreach (FileInfo file in files)
            {
                SimulatedFile f = new SimulatedFile(file.Name, file.Length, FileInfoHelper.GetAttributesOfFile(file.FullName), FileInfoHelper.GetBytesOfFile(file.FullName, 1024));

                f.SetCreationTime(FileInfoHelper.GetCreationDate(file.FullName));
                f.SetLastModifiedTime(FileInfoHelper.GetLastModifiedDate(file.FullName));

                f.SetParent(parent);

                content.Add(f);
            }

            return content;
        }

        /// <summary>
        /// Serializes the simulated folder and saves it at the given path under the given name.
        /// </summary>
        /// <param name="folder">The simulated folder which will be serialized.</param>
        /// <param name="path">The given path.</param>
        /// <param name="name">The given file name.</param>
        /// <returns>A boolean indicating whether the serialization was successful or not.</returns>
        public static bool Serialize(SimulatedFolder folder, string path, string name)
        {
            if (Directory.Exists(path))
            {
                FileStream stream = new FileStream(Path.Combine(path, name + SimulatedFileObject.SerializedFileExtension), FileMode.Create, FileAccess.Write, FileShare.None);
                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    formatter.Serialize(stream, folder);

                    stream.Close();

                    return true;
                }
                catch (Exception)
                {
                }
            }
            else
            {
                return SimulatedFolder.Serialize(folder, Environment.CurrentDirectory, name);
            }

            return false;
        }

        /// <summary>
        /// Looks in the given path for the first saved serialized simulated folder and returns it after deserialization.
        /// </summary>
        /// <param name="path">The given path where the method will scan.</param>
        /// <returns>A deserialized simulated folder.</returns>
        public static SimulatedFolder DeserializeExistingFolder(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo info = new DirectoryInfo(path);

                FileInfo[] files = info.GetFiles();

                foreach (FileInfo file in files)
                {
                    if (file.Extension.Equals(SimulatedFileObject.SerializedFileExtension))
                    {
                        FileStream stream = new FileStream(file.Name, FileMode.Open, FileAccess.Read, FileShare.None);
                        BinaryFormatter formatter = new BinaryFormatter();

                        try
                        {
                            SimulatedFolder folder = (SimulatedFolder)formatter.Deserialize(stream);

                            stream.Close();

                            return folder;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            else
            {
                return SimulatedFolder.DeserializeExistingFolder(Environment.CurrentDirectory);
            }

            return null;
        }

        /// <summary>
        /// Sets the children folders of this folder.
        /// </summary>
        /// <param name="children">The children folders of this folder.</param>
        public void SetChildren(List<IFileObject> children)
        {
            this.childrenFiles.Clear();
            this.childrenFolders.Clear();

            foreach (IFileObject fileObject in children)
            {
                if (fileObject is IFile)
                {
                    this.childrenFiles.Add(fileObject);
                }
                else
                {
                    this.childrenFolders.Add(fileObject);
                }
            }
        }

        /// <summary>
        /// Gets the content of a folder represented as a list of IFileObjects.
        /// </summary>
        /// <returns>The content of a folder.</returns>
        public List<IFileObject> GetContent()
        {
            List<IFileObject> copy = this.childrenFolders.ToList();

            copy.AddRange(this.childrenFiles);

            return copy;
        }

        /// <summary>
        /// Gets the size of the file object.
        /// </summary>
        /// <returns>The size of the file object.</returns>
        public override long GetSizeInBytes()
        {
            long size = 0;

            List<IFileObject> fileObjects = this.GetContent();

            foreach (IFileObject fileObject in fileObjects)
            {
                size += fileObject.GetSizeInBytes();
            }

            return size;
        }

        /// <summary>
        /// Checks if the file object is a file or a folder.
        /// </summary>
        /// <returns>A boolean indicating whether the file object is a file or not.</returns>
        public override bool IsFile()
        {
            return false;
        }

        /// <summary>
        /// Deletes the simulated folder.
        /// </summary>
        /// <returns>A boolean indicating whether the deletion was successful or not.</returns>
        public override bool Delete()
        {
            return ((SimulatedFolder)this.GetParent()).DeleteContent(this);
        }

        /// <summary>
        /// Deletes the given file object, which is contained by this folder.
        /// </summary>
        /// <param name="fileObject">The file object which will be deleted.</param>
        /// <returns>A boolean indicating whether the deletion was successful or not.</returns>
        public bool DeleteContent(SimulatedFileObject fileObject)
        {
            if (!this.childrenFolders.Remove(fileObject))
            {
                return this.childrenFiles.Remove(fileObject);
            }

            return true;
        }

        /// <summary>
        /// Creates a new folder in the current folder.
        /// </summary>
        /// <param name="name">The name of the new folder.</param>
        /// <returns>An instance of the new folder.</returns>
        public IFolder MakeDirectory(string name)
        {
            return this.MakeDirectory(name, name);
        }

        /// <summary>
        /// Pastes the given simulated file object in this folder.
        /// </summary>
        /// <param name="fileObject">The given file object.</param>
        /// <param name="name">The name of the file object.</param>
        /// <returns>A new file object, which represents the copy.</returns>
        public IFileObject Paste(IFileObject fileObject, string name)
        {
            if (fileObject is IFile)
            {
                IFile temp = (IFile)fileObject;

                name = name.Substring(0, name.LastIndexOf(temp.GetExtension()));

                return this.PasteFile((IFile)fileObject, name, name);
            }
            else
            {
                return this.PasteDirectory((IFolder)fileObject, name);
            }
        }

        /// <summary>
        /// Creates a new folder in the current folder.
        /// </summary>
        /// <param name="name">The name of the new folder.</param>
        /// <param name="originalName">The original name of the new folder.</param>
        /// <returns>A boolean indicating whether the creation was successful or not.</returns>
        private SimulatedFolder MakeDirectory(string name, string originalName)
        {
            bool alreadyExists = false;

            foreach (IFileObject fileObject in this.GetContent())
            {
                if (fileObject is IFolder)
                {
                    if (fileObject.GetName().Equals(name))
                    {
                        alreadyExists = true;
                    }
                }
            }

            if (alreadyExists)
            {
                if (name.EndsWith(")") && !originalName.Equals(name))
                {
                    if (name.LastIndexOf(')') > name.LastIndexOf('(') + 1)
                    {
                        string number = name.Substring(name.LastIndexOf('(') + 1, name.LastIndexOf(')') - (name.LastIndexOf('(') + 1));

                        name = name.Substring(0, name.Length - (number.Length + 2));

                        return this.MakeDirectory(name + "(" + (int.Parse(number) + 1) + ")", originalName);
                    }
                }
                else
                {
                    return this.MakeDirectory(name + "(1)", originalName);
                }
            }
            else
            {
                SimulatedFolder newFolder = new SimulatedFolder(name, new List<FileAttributes>());

                newFolder.SetCreationTime(DateTime.Now);
                newFolder.SetLastModifiedTime(DateTime.Now);

                newFolder.SetParent(this);

                this.childrenFolders.Add(newFolder);
                this.childrenFolders.Sort(new FileObjectNameComparer());

                this.SetLastModifiedTime(DateTime.Now);

                return newFolder;
            }

            return null;
        }

        /// <summary>
        /// Pastes the given simulated file  in this folder.
        /// </summary>
        /// <param name="file">The given simulated file.</param>
        /// <param name="name">The name of the simulated file.</param>
        /// <param name="originalName">The original name of the simulated file.</param>
        /// <returns>A new simulated file, which represents the copy.</returns>
        private IFile PasteFile(IFile file, string name, string originalName)
        {
            bool alreadyExists = false;

            foreach (IFileObject fileObject in this.childrenFiles)
            {
                if (fileObject.GetName().Equals(name + file.GetExtension()))
                {
                    alreadyExists = true;
                }
            }

            if (alreadyExists)
            {
                if (name.EndsWith(")") && !originalName.Equals(name))
                {
                    if (name.LastIndexOf(')') > name.LastIndexOf('(') + 1)
                    {
                        string number = name.Substring(name.LastIndexOf('(') + 1, name.LastIndexOf(')') - (name.LastIndexOf('(') + 1));
                        
                        name = name.Substring(0, name.Length - (number.Length + 2));

                        return this.PasteFile(file, name + "(" + (int.Parse(number) + 1) + ")", originalName);
                    }
                }
                else
                {
                    return this.PasteFile(file, name + "(1)", originalName);
                }
            }
            else
            {
                SimulatedFile copy = new SimulatedFile(name + file.GetExtension(), file.GetSizeInBytes(), file.GetAttributes(), file.GetContent());

                copy.SetCreationTime(DateTime.Now);
                copy.SetLastModifiedTime(file.GetLastModifiedTime());

                copy.SetParent(this);

                this.childrenFiles.Add(copy);
                this.childrenFiles.Sort(new FileObjectNameComparer());

                this.SetLastModifiedTime(DateTime.Now);

                return copy;
            }

            return null;
        }

        /// <summary>
        /// Pastes the given simulated folder  in this folder.
        /// </summary>
        /// <param name="folder">The given simulated folder.</param>
        /// <param name="name">The name of the folder.</param>
        /// <returns>A new simulated folder, which represents the copy.</returns>
        private IFolder PasteDirectory(IFolder folder, string name)
        {
            SimulatedFolder newFolder = this.MakeDirectory(name, name);

            if (newFolder != null)
            {
                List<IFileObject> content = folder.GetContent();

                foreach (IFileObject fileObject in content)
                {
                    newFolder.Paste(fileObject, fileObject.GetName());
                }

                this.SetLastModifiedTime(DateTime.Now);

                return newFolder;
            }

            return null;
        }
    }
}
