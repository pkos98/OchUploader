namespace OchUploader.Model
{
    public enum UploadState: int
    {
        All = 4,
        ToSkip = -1,
        Queued = 0,
        Paused = 1,
        Uploading = 2,
        Finished = 3,
    }

    public interface IStateProvider
    {
        UploadState State { get; }
    }
}