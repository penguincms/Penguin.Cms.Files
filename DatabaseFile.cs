﻿using Penguin.Cms.Entities;
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
    public class DatabaseFile : AuditableEntity, IFile
    {
        /// <summary>
        /// A byte array representing the file data
        /// </summary>
        [DontAllow(DisplayContexts.Any)]
        public byte[] Data { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Attempts to return the extension of the file based on the FileName
        /// </summary>
        [NotMapped]
        public string Extension => Path.GetExtension(FileName);

        /// <summary>
        /// The FileName of the file being stored
        /// </summary>

        public string FileName { get; set; }

        /// <summary>
        /// The full tree path to the file for recursive heirarchies
        /// </summary>

        [DontAllow(DisplayContexts.Edit | DisplayContexts.BatchEdit)]
        public string FilePath { get; set; }

        /// <summary>
        /// The FilePath + File Name
        /// </summary>

        [DontAllow(DisplayContexts.Edit | DisplayContexts.BatchEdit)]
        public string FullName
        {
            get => Path.Combine(FilePath ?? "", FileName ?? "");
            set
            {
                FileName = Path.GetFileName(value);
                FilePath = new FileInfo(value).Directory.FullName;
            }
        }

        /// <summary>
        /// If true, this node represents a folder containing files in a tree and not a concrete file
        /// </summary>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// The size of the file
        /// </summary>
        [DontAllow(DisplayContexts.Edit | DisplayContexts.BatchEdit)]
        public long Length { get; set; }

        /// <summary>
        /// A Guid representing an entity that this file is owned by (Ex an Email guid if this is an email attachment)
        /// </summary>
        public Guid Owner { get; set; }

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
        public DatabaseFile(string filePath, string fileName = "")
        {
            Data = File.ReadAllBytes(filePath);
            Length = Data.Length;
            FileName = string.IsNullOrWhiteSpace(FileName) ? Path.GetFileName(filePath) : fileName;
        }

        /// <summary>
        /// Constructs a new instance of the database file using the specified byte array as a data source
        /// </summary>
        /// <param name="bytes">The byte array representing the file contents</param>
        /// <param name="fileName">The name to give the file</param>
        public DatabaseFile(byte[] bytes, string fileName)
        {
            Data = bytes;
            FileName = fileName;
        }
    }
}