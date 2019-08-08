using Penguin.Services.Files;
using System.IO;

namespace Penguin.Cms.Files.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class FileServiceExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region Methods
        /// <summary>
        /// If the file requested exists on disk, this populates the internal byte array with its contents
        /// </summary>
        /// <param name="fs">The file service providing access (not required?)</param>
        /// <param name="df">The database file containing the file definition, and the target of the fill</param>
        public static void FillData(this FileService fs, DatabaseFile df)
        {
            if (df.Data.Length == 0 && File.Exists(df.FullName))
            {
                df.Data = File.ReadAllBytes(df.FullName);
            }
        }

        #endregion Methods
    }
}