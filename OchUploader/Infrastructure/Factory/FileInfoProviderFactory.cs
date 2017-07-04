using OchUploader.Api;

namespace OchUploader.Infrastructure.Factory
{
    public class FileInfoProviderFactory: IFileInfoProviderFactory
    {
        public IFileInfoProvider CreateProvider(string path, string name)
        {
            return null;
        }

    }

    public interface IFileInfoProviderFactory
    {
        IFileInfoProvider CreateProvider(string path, string name);
    }
}
