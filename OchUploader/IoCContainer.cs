using Ninject;
using NPoco;
using OchUploader.DataAccess;
using OchUploader.Infrastructure;
using OchUploader.Infrastructure.Factory;
using OchUploader.Model;
using OchUploader.Service;
using OchUploader.Service.Persistence;
using OchUploader.ViewModel;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System;

namespace OchUploader
{
    public static class IocContainer
    {
        private static readonly IReadOnlyKernel _kernel = ConfigureKernel();
        private static readonly string _connectionString = @"Data Source=ochuploader.db;Version=3;";

        public static string PluginPath { get; set; }
        public static string SimpleHostsPath => Path.Combine(PluginPath, "hosts");
        public static string ExtendedHostsPath => Path.Combine(PluginPath, "hosts", "extended");
        public static T Get<T>()
        {
            return _kernel.Get<T>();
        }

        private static IReadOnlyKernel ConfigureKernel()
        {
            var config = new KernelConfiguration();
            LoadPlugins(config);

            config.Bind<IDatabase>().ToMethod(x => new Database(_connectionString, DatabaseType.SQLite)).InSingletonScope();
            config.Bind<IRepository<UploadModel>>().To<CachedRepositoryBase<UploadModel>>();
            config.Bind<IRepository<DirectoryModel>>().To<CachedRepositoryBase<DirectoryModel>>();

            config.Bind<IPluginManager>().To<PluginManagerService>();
            config.Bind<IAccessProtocollIdentifier>().To<AccessProtocollIdentifier>();

            config.Bind<IFileInfoProviderFactory>().To<FileInfoProviderFactory>();
            config.Bind<IDirectoryInfoProviderFactory>().To<DirectoryInfoProviderFactory>();
            config.Bind<IHostProviderFactory>().To<HostProviderFactory>();

            config.Bind<IFilterService<DirectoryViewModel>>().To<IncrementalFilterService<DirectoryViewModel>>();
            config.Bind<INotificationService>().To<LogNotificationService>();
            config.Bind<IUploadModelService>().To<UploadModelService>();
            config.Bind<IDirectoryModelService>().To<DirectoryModelService>();
            config.Bind<IDirectoryManagerService>().To<DirectoryManagerService>();
            config.Bind<IDirectoryInjectorService>().To<NewDirectoryInjectionService>().InSingletonScope();
            config.Bind<NewDirectoryInjectionService>().ToSelf().InSingletonScope();
            config.Bind<DirectoryCollectionViewModel>().ToSelf();
            return config.BuildReadonlyKernel();
        }

        private static void LoadPlugins(KernelConfiguration config)
        {
            var pluginPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "OchUploader", "Plugins");
            var directoryInfoProviderPath = Path.Combine(pluginPath, "directoryinfoprovider");
            var fileInfoProviderPath = Path.Combine(pluginPath, "fileinfoprovider");
            var simpleHostsPath = Path.Combine(pluginPath, "hosts");
            var extendedHostsPath = Path.Combine(simpleHostsPath, "extended");
        }
    }
}