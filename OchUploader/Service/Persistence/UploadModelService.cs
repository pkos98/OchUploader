using OchUploader.Api;
using OchUploader.DataAccess;
using OchUploader.Infrastructure.Factory;
using OchUploader.Model;
using System.Collections.Generic;
using System.Linq;

namespace OchUploader.Service.Persistence
{
    /// <summary>
    /// Service providing access to uploads
    /// </summary>
    public class UploadModelService : DbServiceBase<UploadModel>, IUploadModelService
    {

        private readonly IFileInfoProviderFactory _fileFactory;

        public IEnumerable<string> UsedHosts { get; set; } = new[] { "Share-Online.biz" };
        public delegate void UploadModelAddedEventHandler(UploadModel addedModel, bool addedByInjector);

        public UploadModelService(IRepository<UploadModel> repo, IFileInfoProviderFactory fileFactory) : base(repo)
        {
            _fileFactory = fileFactory;
        }

        public IEnumerable<UploadModel> CreateUploads(IEnumerable<string> pathCollection, string providerName, string host) 
        {
            var uploadList = new List<UploadModel>();
            foreach (var path in pathCollection)
            {
                var newUpload = InitializeUpload(path, providerName, host);
                uploadList.Add(newUpload);
            }
            return uploadList;
        }
        public IEnumerable<UploadModel> GetUploadsByDirectory(string directoryPath, string host)
        {
            return _repo.GetAll().Where(x => x.DirectoryPath == directoryPath && x.Host == host);
        }
        public void AddUploadToDb(UploadModel model)
        {
            _repo.Add(model);
        }

        private UploadModel InitializeUpload(string path, string providerName, string host)
        {
            var fileInfoProvider = _fileFactory.CreateProvider(path, providerName);
            var newUpload = new UploadModel();
            newUpload.FileName = fileInfoProvider.GetFilename(path);
            newUpload.DirectoryPath = fileInfoProvider.GetDirectoryPath(path);
            newUpload.Host = host;
            newUpload.Length = fileInfoProvider.GetLength(path);

            return newUpload;
        }

    }

    public interface IUploadModelService : IDbService<UploadModel>
    {
        IEnumerable<UploadModel> CreateUploads(IEnumerable<string> pathCollection, string providerName, 
                                                string host);
        void AddUploadToDb(UploadModel model);
        IEnumerable<UploadModel> GetUploadsByDirectory(string directoryPath, string host);
    }
}