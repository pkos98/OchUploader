using System.Net;

namespace OchUploader.Api.Host
{
    public interface ISimpleHttpHost
    {
        string PatternRegex { get; }
        void Initialize(string username, string password);
        HttpWebRequest Upload(IFileInfoProvider fileInfoProvider);
        string GetDownloadLink(HttpWebResponse response);
    }

}
