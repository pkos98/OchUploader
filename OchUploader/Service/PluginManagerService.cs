using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

namespace OchUploader.Service
{
    public enum PluginType
    {
        AllHosts,
        SimpleHost,
        ExtendedHost,
        DirectoryInfoProvider,
        FileInfoProvider
    }

    public class PluginManagerService : IPluginManager
    {
        private readonly string _pluginPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "OchUploader", "Plugins");
        private readonly string _directoryInfoProviderPath;
        private readonly string _fileInfoProviderPath;
        private readonly string _simpleHostsPath;
        private readonly string _extendedHostsPath;
        private readonly Dictionary<PluginType, string> _pathByPluginType = new Dictionary<PluginType, string>();

        public PluginManagerService()
        {
            _directoryInfoProviderPath = Path.Combine(_pluginPath, "directoryinfoprovider");
            _fileInfoProviderPath = Path.Combine(_pluginPath, "fileinfoprovider");
            _simpleHostsPath = Path.Combine(_pluginPath, "hosts");
            _extendedHostsPath = Path.Combine(_simpleHostsPath, "extended");
            _pathByPluginType.Add(PluginType.SimpleHost, _simpleHostsPath);
            _pathByPluginType.Add(PluginType.ExtendedHost, _extendedHostsPath);
            _pathByPluginType.Add(PluginType.FileInfoProvider, _fileInfoProviderPath);
            _pathByPluginType.Add(PluginType.DirectoryInfoProvider, _directoryInfoProviderPath);
        }


        /// <summary>
        /// Returns the names of all available host plugins
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllHosts()
        {
            return GetAllHosts().Concat(GetAllExtendedHosts());
        }
        public IEnumerable<string> GetAllSimpleHosts()
        {
            return Directory.EnumerateFiles(_simpleHostsPath);
        }
        public IEnumerable<string> GetAllExtendedHosts()
        {
            return Directory.EnumerateFiles(_extendedHostsPath);
        }
        public IEnumerable<string> GetAllFileInfoProviders()
        {
            return Directory.EnumerateFiles(_fileInfoProviderPath);
        }

        public T Resolve<T>(string pluginName, PluginType type)
        {
            pluginName = pluginName.ToLower();
            string fullPluginPath = Path.Combine(_pathByPluginType[type], pluginName) + ".dll";
            if (!File.Exists(fullPluginPath))
                throw new FileNotFoundException($"The given plugin {pluginName} does not exist");
            return LoadPlugin<T>(fullPluginPath);
        }

        private T LoadPlugin<T>(string fullPluginPath)
        {
            try
            {
                var asm = Assembly.LoadFile(fullPluginPath);
                var exportedTypes = asm.GetExportedTypes();
                var selectedType = exportedTypes.First(x => typeof(T).IsAssignableFrom(x));
                return (T)Activator.CreateInstance(selectedType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException();
            }
        }
    }

    public interface IPluginManager
    {
        IEnumerable<string> GetAllHosts();
        IEnumerable<string> GetAllSimpleHosts();
        IEnumerable<string> GetAllExtendedHosts();
        IEnumerable<string> GetAllFileInfoProviders();
        T Resolve<T>(string name, PluginType type);
    }
}
