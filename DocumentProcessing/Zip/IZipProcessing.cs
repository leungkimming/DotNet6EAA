using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telerik.Zip;

namespace DocumentProcessing {
    public interface IZipProcessing {
        void CreateZip(string zipFileName, string[] zipArchiveFiles);
        void CreateZip(Stream stream, string zipFileName, string[] zipArchiveFiles);
        void CreateZip(string zipFileName, Dictionary<string, Stream> zipArchiveFiles);
        void CreateZip(string zipFileName, Dictionary<string, Stream> zipArchiveFiles, Encoding? entryNameEncoding, CompressionSettings? compressionSettings, EncryptionSettings? encryptionSettings);
    }
}
