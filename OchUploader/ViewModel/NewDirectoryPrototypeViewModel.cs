using OchUploader.Api;

namespace OchUploader.ViewModel
{
    public class NewDirectoryPrototypeViewModel
    {
        public string ProviderName { get; set; }
        public string Path { get; set; }
        public string Host { get; set; }

        public static NewDirectoryPrototypeViewModel Create()
        {
            return new NewDirectoryPrototypeViewModel();
        }
    }
}
