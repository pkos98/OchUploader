using OchUploader.Api;
using System.IO;

namespace OchUploader.Infrastructure.FileInfoProvider
{
    /// <summary>
    /// Defines the access to an resource meant to be uploaded
    /// </summary>
    public abstract class FileInfoProvider : IFileInfoProvider
    {
        protected readonly string _path; 

        public abstract string ProviderName { get; }
        public abstract string Filename { get; }
        public abstract string DirectoryPath { get; }
        public abstract long Length { get; }

        /// <summary>
        /// The length of the resource in bytes
        /// </summary>
        public long ResourceLength
        {
            get
            {
                using (var stream = GetStream())
                    return stream.Length;
            }
        }

        public FileInfoProvider(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Gets the underlying stream of the resource
        /// </summary>
        /// <returns></returns>
        public abstract Stream GetStream();
    }
}