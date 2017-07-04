using OchUploader.Model;

namespace OchUploader.ViewModel
{
    public class UploadViewModel : ViewModelBase
    {
        public UploadModel UploadModel { get; private set; }
        public string FileName => UploadModel.FileName;
        public string Host => UploadModel.Host;
        public long Length => UploadModel.Length;
        public long ProgressInBytes => UploadModel.UploadedBytes;
        public long ProgressInPercentage
        {
            get
            {
                if (UploadModel.UploadedBytes == 0)
                    return 0;
                return UploadModel.Length/ UploadModel.UploadedBytes;
            }
        }
        public DirectoryViewModel Directory { get; set; }

        public UploadViewModel(UploadModel uploadModel)
        {
            UploadModel = uploadModel;
        }

        public override string ToString()
        {
            return UploadModel.FileName;
        }
    }
}
