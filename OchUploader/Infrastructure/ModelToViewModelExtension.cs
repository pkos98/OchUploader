using OchUploader.Model;
using OchUploader.ViewModel;

namespace OchUploader.Infrastructure
{
    public static class ModelToViewModelExtension
    {
        public static UploadViewModel ToViewModel(this UploadModel upload)
        {
            return new UploadViewModel(upload);
        }
    }
}
