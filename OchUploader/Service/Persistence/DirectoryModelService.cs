using OchUploader.Api;
using OchUploader.DataAccess;
using OchUploader.Infrastructure.Factory;
using OchUploader.Model;
using System;
using System.Collections.Generic;

namespace OchUploader.Service.Persistence
{

    public delegate void NewDirectoryAddedEventHandler(string path, AccessProtocoll protocoll);

    /// <summary>
    /// Service providing access to uploads
    /// </summary>
    public class DirectoryModelService : DbServiceBase<DirectoryModel>, IDirectoryModelService
    {
        private readonly IDirectoryInfoProviderFactory _directoryFactory;
        private readonly List<IDirectoryInjectorService> _injectorList = new List<IDirectoryInjectorService>();
        private readonly IUploadModelService _uploadModelService;

        public IEnumerable<IDirectoryInjectorService> DirectoryInjectorServices => _injectorList;

        public DirectoryModelService(IRepository<DirectoryModel> repo, 
                                     IDirectoryInfoProviderFactory directoryFactory,
                                     IUploadModelService _uploadModelService) : base(repo)
        {
            _directoryFactory = directoryFactory;
            this._uploadModelService = _uploadModelService;
        }


        /// <summary>
        /// Creates a DirectoryModel-instance with the specified host
        /// </summary>
        /// <param name="path"></param>
        /// <param name="protocoll"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public DirectoryModel CreateDirectory(string path, string providerName, string host)
        {
            var newDirectory = new DirectoryModel(path, providerName, host);
            var infoProvider = _directoryFactory.CreateInfoProvider(path, providerName);
            _uploadModelService.CreateUploads(infoProvider.EnumerateFilenames(path), infoProvider.ProviderName, host);
            return newDirectory;
        }
        
        /// <summary>
        /// Saves all changes made to DirectoryModels and to UploadModels
        /// </summary>
        public override void Save()
        {
            base.Save();
            _uploadModelService.Save();
        }

        public bool ContainsDirectory(string path, string host)
        {
            var id = new { Path = path, Host = host };
            return _repo.Contains(id);
        }

    }

    public interface IDirectoryModelService : IDbService<DirectoryModel>
    {
        DirectoryModel CreateDirectory(string path, string providerName, string host);
        bool ContainsDirectory(string path, string host);
        IEnumerable<IDirectoryInjectorService> DirectoryInjectorServices { get; }
    }
}