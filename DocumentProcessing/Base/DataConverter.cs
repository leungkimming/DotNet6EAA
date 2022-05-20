using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DocumentProcessing {
    public static class DataConverter {
        public static byte[] StreamToByte(MemoryStream stream) {
            return stream.ToArray();
        }
    }
}
