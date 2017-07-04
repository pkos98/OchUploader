//using OchUploader.Api;
//using System.IO;

//namespace OchUploader.Infrastructure
//{
//    public class ChunkedStreamWriter : IStreamWriter
//    {
//        #region Fields
//        private readonly Stream _sourceStream;
//        private int _chunkLength = 1024;
//        #endregion

//        #region Methods
//        public long Length => _sourceStream.Length;

//        public ChunkedStreamWriter(Stream sourceStream)
//        {
//            this._sourceStream = sourceStream;
//        }

//        public void WriteTo(Stream destinationStream)
//        {
//            byte[] chunkBuffer = new byte[_chunkLength];
//            var amountOfChunks = _sourceStream.Length / _chunkLength;
//            for (int i = 0; i < amountOfChunks && amountOfChunks > 1; i++)
//            {
//                _sourceStream.Read(chunkBuffer, 0, _chunkLength);
//                destinationStream.Write(chunkBuffer, 0, _chunkLength);
//            }
//            var bytesOfLastChunk = _sourceStream.Length - (amountOfChunks * _chunkLength);
//            _sourceStream.Read(chunkBuffer, 0, (int)bytesOfLastChunk);
//            _sourceStream.Close();
//            destinationStream.Write(chunkBuffer, 0, (int)bytesOfLastChunk);
//            destinationStream.Close();
//        }
//        #endregion
//    }
//}