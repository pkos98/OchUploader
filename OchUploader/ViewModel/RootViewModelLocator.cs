namespace OchUploader.ViewModel
{
    public class RootViewModelLocator
    {
        public DirectoryCollectionViewModel RootViewModel => IocContainer.Get<DirectoryCollectionViewModel>();
    }
}
