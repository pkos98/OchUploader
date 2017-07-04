using OchUploader.Model;
using OchUploader.Service.Persistence;

namespace OchUploader.ViewModel
{
    public static class ModelToViewModelExtension
    {
        public static DirectoryViewModel ToViewModel(this DirectoryModel directoryModel)
        {
            return new DirectoryViewModel(directoryModel, IocContainer.Get<IUploadModelService>());
        }
    }

    
}
