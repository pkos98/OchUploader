using System.Collections.Generic;

namespace OchUploader.Api
{
    /// <summary>
    /// Defines a module which represents a directory independently from its location
    /// </summary>
    public interface IDirectoryInfoProvider
    {
        string ProviderName { get; }

        IEnumerable<string> EnumerateFilenames(string path);
        IEnumerable<string> EnumerateDirectories(string path);
    }
}