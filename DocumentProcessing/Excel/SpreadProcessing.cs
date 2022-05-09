using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace DocumentProcessing {
    public class SpreadProcessing: ISpreadProcessing {
        public SpreadProcessing() {
        }
        public void ExportToXlsx(string filePath, string resultFile, object workbook) {
            if (workbook is Workbook telerikWook) {
                IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
                string path = Path.Combine(filePath,resultFile);
                using (FileStream stream = File.OpenWrite(path)) {
                    formatProvider.Export(telerikWook, stream);
                }
                ProcessStartInfo psi = new ProcessStartInfo(){
                    FileName = path,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }

        }
    }
}
