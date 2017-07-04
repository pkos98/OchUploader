using System.IO;

namespace OchUploader.Api
{
    public interface IFileInfoProvider
    {

        string ProviderName { get; }

        string GetFilename(string path);
        string GetDirectoryPath(string path);
        long GetLength(string path);

        Stream GetStream(string path);

    }
}