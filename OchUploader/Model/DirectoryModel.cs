using NPoco;
using OchUploader.Infrastructure;

namespace OchUploader.Model
{
    [TableName("directories")]
    [PrimaryKey("path,host")]
    public class DirectoryModel: Notifyer
    {

        private string _path;
        private UploadState _state = UploadState.Queued;

        [Column("path")]
        public string Path
        {
            get { return _path; }
            set
            {
                if (_path == value)
                    return;

                _path = value;
                RaisePropertyChanged();
            }
        }
        [Column("state")]
        public UploadState State
        {
            get { return _state; }
            set
            {
                if (value == _state)
                    return;

                _state = value;
                RaisePropertyChanged();
            }
        }
        [Column("infoprovider")]
        public string InfoProvider { get; set; } = "LocalFileInfoProvider";
        [Column("host")]
        public string Host { get; set; } = "Unknown";

        public DirectoryModel() { }
        public DirectoryModel(string path, string infoProvider, string host)
        {
            Path = path;
            InfoProvider = infoProvider;
            Host = host;
        }

    }
}
