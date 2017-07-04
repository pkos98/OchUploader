using OchUploader.Api;
using OchUploader.Model;
using OchUploader.Service;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OchUploader
{
    public static class Constants
    {
        private static readonly IPluginManager _pluginManager = IocContainer.Get<IPluginManager>();

        public static IEnumerable<string> AvailableInfoProvider => _pluginManager.GetAllFileInfoProviders().Select(x => Path.GetFileName(x));
        public static IEnumerable<string> AvailableFileHoster =>
            new List<string>(){ "All" , "Uploaded.net", "Share-Online.biz" };
        public static IEnumerable<UploadState> AvailableStates => new[]
        {
            UploadState.All,
            UploadState.Uploading,
            UploadState.Queued,
            UploadState.Paused,
            UploadState.Finished,
            UploadState.ToSkip
        };
        public static string KnownDirectoriesPath => "known_directories.txt";

    }
}
