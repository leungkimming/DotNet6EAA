using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentProcessing {
    public interface IPdfProcessing {
        void ExportToPDF(string filePath, string resultFile, object generatedDocument);
        void AddWaterMark(object generatedDocument, WaterMark waterMark);
        void MergePDF(string resultFile, List<object> documentList);
    }
}
