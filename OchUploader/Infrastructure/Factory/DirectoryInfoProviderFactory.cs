using OchUploader.Api;
using OchUploader.Service;

namespace OchUploader.Infrastructure.Factory
{
    public class DirectoryInfoProviderFactory: IDirectoryInfoProviderFactory
    {
        private readonly IPluginManager _pluginManager;

        public DirectoryInfoProviderFactory(IPluginManager pluginManager)
        {
            _pluginManager = pluginManager;
        }

        public IDirectoryInfoProvider CreateInfoProvider(string path, string name)
        {
            var instance = _pluginManager.Resolve<IDirectoryInfoProvider>(name, PluginType.DirectoryInfoProvider);
            return instance;
        }

    }

    public interface IDirectoryInfoProviderFactory
    {
        IDirectoryInfoProvider CreateInfoProvider(string path, string name);
    }
}
