using OchUploader.Api;
using OchUploader.Model;
using OchUploader.Service.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OchUploader.Service
{
    public delegate void NewDirectoriesAddedEventHandler(IEnumerable<DirectoryModel> directories);
    public delegate void DownDirectoryDetectedEventHandler(DirectoryModel directory);

    /// <summary>
    /// Manages injection of new directories and provides events for those
    /// </summary>
    public class DirectoryManagerService: IDirectoryManagerService
    {
        private readonly IDirectoryModelService _directoryModelService;
        private readonly List<IDirectoryInjectorService> _injectorServices = new List<IDirectoryInjectorService>();

        public IDirectoryModelService DirectoryModelService => _directoryModelService;

        public event NewDirectoriesAddedEventHandler NewDirectoriesAddedEvent;
        public event DownDirectoryDetectedEventHandler DownDirectoryDetectedEvent;

        public List<string> UsedHosts { get; private set; } = new List<string>();
        public IEnumerable<IDirectoryInjectorService> DirectoryInjectorCollection => _injectorServices; 

        public DirectoryManagerService(IDirectoryModelService directoryModelService,
                                       IDirectoryInjectorService injectorService)
        {
            _directoryModelService = directoryModelService;
            InitializeInjector(injectorService);
        }

        private void InitializeInjector(IDirectoryInjectorService injectorService)
        {
            injectorService.AddBaseDirectory(@"C:\Users\patrick\Uploads", "LocalDirectoryInfoProvider");
            injectorService.ChangedPathDetectedEvent += InjectorService_InjectionUpdate;
            _injectorServices.Add(injectorService);
        }

        private void InjectorService_InjectionUpdate(string path, string providerName, InjectionReason reason, string host = null)
        {
            switch (reason)
            {
                case InjectionReason.Directory_New:
                    IEnumerable<DirectoryModel> newDirectories = Enumerable.Empty<DirectoryModel>();
                    //newDirectories = _directoryModelService.CreateDirectoriesWithStandardHosts(path, providerName);
                    System.Diagnostics.Debug.WriteLine("Detected new directory ");
                    if (!newDirectories.Any())
                        NewDirectoriesAddedEvent(null);
                    else
                        NewDirectoriesAddedEvent(newDirectories);
                    break;
                case InjectionReason.Directory_Down:
                    break;
                default:
                    break;
            }
        }
    }

    public interface IDirectoryManagerService
    {
        event NewDirectoriesAddedEventHandler NewDirectoriesAddedEvent;
        event DownDirectoryDetectedEventHandler DownDirectoryDetectedEvent; 
        IEnumerable<IDirectoryInjectorService> DirectoryInjectorCollection { get; }
        IDirectoryModelService DirectoryModelService { get; }

    }
}
