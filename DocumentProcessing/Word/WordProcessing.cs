using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.Extensibility;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf;
using Telerik.Windows.Documents.Flow.FormatProviders.Txt;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Common.FormatProviders;
using System.IO;
using System.Diagnostics;
using Telerik.Windows.Documents.Fixed.Model;

namespace DocumentProcessing {
    public enum DocumentFormat {
        docx=1,
        html=2,
        rtf=3,
        txt=4,
        pdf=5
    }
    public class WordProcessing : IWordProcessing {
        public void ExportToWord(string filePath, string resultFile, DocumentFormat documentFormat, object generatedDocument) {
            if (generatedDocument is RadFlowDocument flowDocument) {
                string selectedFormat = documentFormat.ToString();
                IFormatProvider<RadFlowDocument> formatProvider = FormatDocumentType(selectedFormat);
                string path = Path.Combine(filePath, $"{resultFile}.{selectedFormat}");
                using (FileStream stream = File.OpenWrite(path)) {
                    formatProvider.Export(flowDocument, stream);
                }

            }
        }
        public byte[]? GetWordByte(DocumentFormat documentFormat, object generatedDocument) {
            if (generatedDocument is RadFlowDocument flowDocument) {
                string selectedFormat = documentFormat.ToString();
                IFormatProvider<RadFlowDocument> formatProvider = FormatDocumentType(selectedFormat);
                using MemoryStream stream = new MemoryStream();
                formatProvider.Export(flowDocument, stream);
                var fileData = DataConverter.StreamToByte(stream);
                return fileData;
            }
            return null;
        }

        private static IFormatProvider<RadFlowDocument> FormatDocumentType(string selectedFormat) {
            FontsProviderBase fontsProvider = new FontsProvider();
            FixedExtensibilityManager.FontsProvider = fontsProvider;

            IFormatProvider<RadFlowDocument> formatProvider;
            switch (selectedFormat) {
                case "docx":
                    formatProvider = new DocxFormatProvider();
                    break;
                case "rtf":
                    formatProvider = new RtfFormatProvider();
                    break;
                case "txt":
                    formatProvider = new TxtFormatProvider();
                    break;
                case "html":
                    formatProvider = new HtmlFormatProvider();
                    break;
                default:
                    formatProvider = new DocxFormatProvider();
                    break;
            }

            return formatProvider;
        }
    }
}
