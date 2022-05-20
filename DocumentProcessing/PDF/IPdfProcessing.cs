using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DocumentProcessing {
    public interface IPdfProcessing {
        void ExportToPDF(string filePath, string fileName, object generatedDocument);
        void AddWaterMark(object generatedDocument, WaterMark waterMark);
        void MergePDF(string resultFile, List<object> documentList);
        byte[] GetPDFByte(object generatedDocument);
        byte[] MergePDFToByte(List<object> documentList);
    }
}
