using NPoco;
using OchUploader.Infrastructure;

namespace OchUploader.Model
{
    [TableName("uploads")]
    [PrimaryKey("id", AutoIncrement = true)]
    public class UploadModel: Notifyer
    {
        private long _uploadedBytes;
        private UploadState _state = UploadState.Queued;

        [Column("Id")]
        public long Id { get; set; }
        [Column("directory_path")]
        public string DirectoryPath { get; set; }
        [Column("filename")]
        public string FileName { get; set; }
        [Column("uploaded_bytes")]
        public long UploadedBytes
        {
            get { return _uploadedBytes; }
            set
            {
                if (value == UploadedBytes)
                    return;
                _uploadedBytes = value;
                RaisePropertyChanged();
            }
        }
        [Column("state")]
        public UploadState State
        {
            get { return _state; }
            set
            {
                if (value == State)
                    return;
                _state = value;
                RaisePropertyChanged();
            }
        }
        [Column("host")]
        public string Host { get; set; }
        [Column("total_bytes")]
        public long Length { get; set; }
    }
}