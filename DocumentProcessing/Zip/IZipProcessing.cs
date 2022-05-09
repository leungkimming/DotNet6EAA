using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DocumentProcessing {
    public interface IZipProcessing {
        void CreateZip(string zipFileName, string[] zipArchiveFiles);
        void CreateZip(Stream stream, string zipFileName, string[] zipArchiveFiles);
    }
}
