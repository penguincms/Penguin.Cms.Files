using Penguin.Cms.Security;
using Penguin.Files.Abstractions;
using Penguin.Persistence.Abstractions.Attributes.Control;
using Penguin.Persistence.Abstractions.Attributes.Relations;
using System;
using System.IO;

namespace Penguin.Cms.Files
{
    /// <summary>
    /// An entity containing file information with the intent of being stored in a database and tracked as an entity from within a CMS
    /// </summary>
    [Table("DatabaseFiles")]
    public class DatabaseFile : PermissionableEntity, IFile
    {
        #region Properties

        /// <summary>
        /// A byte array representing the file data
        /// </summary>
        [DontAllow(DisplayContext.Any)]
        public byte[] Data { get; set; }

        /// <summary>
        /// Attempts to return the extension of the file based on the FileName
        /// </summary>
        [NotMapped]
        public string Extension => Path.GetExtension(this.FileName);

        /// <summary>
        /// The FileName of the file being stored
        /// </summary>

        public string FileName { get; set; }

        /// <summary>
        /// The full tree path to the file for recursive heirarchies
        /// </summary>

        [DontAllow(DisplayContext.Edit | DisplayContext.BatchEdit)]
        public string FilePath { get; set; }

        /// <summary>
        /// The FilePath + File Name
        /// </summary>

        [DontAllow(DisplayContext.Edit | DisplayContext.BatchEdit)]
        public string FullName => Path.Combine(this.FilePath ?? "", this.FileName ?? "");

        /// <summary>
        /// If true, this node represents a folder containing files in a tree and not a concrete file
        /// </summary>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// The size of the file
        /// </summary>
        [DontAllow(DisplayContext.Edit | DisplayContext.BatchEdit)]
        public long Length { get; set; }

        /// <summary>
        /// A Guid representing an entity that this file is owned by (Ex an Email guid if this is an email attachment)
        /// </summary>
        public Guid Owner { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructs a new instance of the database file
        /// </summary>
        public DatabaseFile()
        {
        }

        /// <summary>
        /// Constructs a new instance of the database file by reading in the specified file off disk
        /// </summary>
        /// <param name="filePath">The path the the file to read</param>
        /// <param name="fileName">An optional override file name to assign</param>
        /// <param name="mimeType">An optional MimeType override</param>
        public DatabaseFile(string filePath, string fileName = "", string mimeType = "")
        {
            this.Data = File.ReadAllBytes(filePath);
            this.Length = this.Data.Length;
            this.FileName = string.IsNullOrWhiteSpace(this.FileName) ? Path.GetFileName(filePath) : fileName;
        }

        /// <summary>
        /// Constructs a new instance of the database file using the specified byte array as a data source
        /// </summary>
        /// <param name="bytes">The byte array representing the file contents</param>
        /// <param name="fileName">The name to give the file</param>
        /// <param name="mimeType">An optional MimeType to assign the file</param>
        public DatabaseFile(byte[] bytes, string fileName, string mimeType = "")
        {
            this.Data = bytes;
            this.FileName = fileName;
        }

        #endregion Constructors
    }
}