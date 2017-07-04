namespace OchUploader.Api
{
    public enum AccessProtocoll: int
    {
        Local = 0,
        Http_s,
        Ftp,
        Torrent,
        Unknown,
        Not_supported,
        All
    }
}