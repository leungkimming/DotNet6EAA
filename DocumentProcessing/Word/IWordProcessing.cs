using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentProcessing {
    public interface IWordProcessing {
        void ExportToWord(string filePath, string resultFile, DocumentFormat documentFormat, object generatedDocument);
    }
}
