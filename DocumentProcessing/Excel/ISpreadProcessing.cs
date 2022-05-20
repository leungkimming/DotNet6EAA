using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentProcessing {
    public interface ISpreadProcessing {
        void ExportToXlsx(string filePath, string resultFile, object workbook);
        byte[]? GetXlsxByte(object workbook);
    }
}
