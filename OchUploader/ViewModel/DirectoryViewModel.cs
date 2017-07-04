using OchUploader.Infrastructure;
using OchUploader.Model;
using OchUploader.Service.Persistence;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using IOPath = System.IO.Path;

namespace OchUploader.ViewModel
{
    public class DirectoryViewModel : ViewModelBase
    {

        private readonly IUploadModelService _uploadModelService;
        private readonly ObservableCollection<UploadViewModel> _allUploads =
            new ObservableCollection<UploadViewModel>();

        #region Properties
        public DirectoryModel DirectoryModel { get; private set; }

        public string Path => DirectoryModel.Path;
        public string InfoProvider 
        {
            get { return DirectoryModel.InfoProvider; }
            set
            {
                DirectoryModel.InfoProvider = value;
            }
        }
        public UploadState State
        {
            get { return DirectoryModel.State; }
            set { DirectoryModel.State = value; }
        }
        public string Host => _allUploads.First().Host ?? "Share-Online.biz";
        public string Name => IOPath.GetFileName(Path);
        public float ProgressInPercent { get; }
        public float ProgressInBytes => TotalLength / (0.1f * new Random().Next(1, 100));
        public long TotalLength { get; private set; }

        public ObservableCollection<UploadViewModel> DisplayedUploads => _allUploads;
        public RelayCommand AddUploadCommand { get; private set; } = new RelayCommand();
        #endregion

        public DirectoryViewModel(DirectoryModel directory, IUploadModelService uploadModelService)
        {
            _uploadModelService = uploadModelService;
            DirectoryModel = directory;

            foreach (var upload in _uploadModelService
                                   .GetUploadsByDirectory(DirectoryModel.Path, DirectoryModel.Host))
            {
                _allUploads.Add(upload.ToViewModel());
                TotalLength += upload.Length;
            }
        }

        private void UploadModelService_UploadModelAddedEvent(UploadModel addedModel, bool addedByInjector)
        {
            if (addedByInjector == false)
                return;
            _allUploads.Add(addedModel.ToViewModel());
        }

        public override string ToString()
        {
            return Name;
        }


    }
}