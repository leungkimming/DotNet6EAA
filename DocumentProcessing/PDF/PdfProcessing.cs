using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Telerik.Documents.Core.Fonts;
using Telerik.Documents.Primitives;
using Telerik.Windows.Documents.Extensibility;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Editing;

namespace DocumentProcessing {
    public class WaterMark {
        public string Text { get; set; }
        public byte Transparency { get; set; }
        public int Angle { get; set; }
    }
    public class PdfProcessing : IPdfProcessing {
        public PdfProcessing() {

        }
        public void ExportToPDF(string filePath, string fileName, object generatedDocument) {
            if (generatedDocument == null) {
                throw new ArgumentNullException(nameof(generatedDocument));
            }
            FontsProviderBase fontsProvider = new FontsProvider();
            FixedExtensibilityManager.FontsProvider = fontsProvider;
            PdfFormatProvider formatProvider = new PdfFormatProvider();
            formatProvider.ExportSettings.ImageQuality = ImageQuality.High;
            var resultPath=Path.Combine(filePath,fileName);
            using (FileStream stream = File.OpenWrite(resultPath)) {
                RadFixedDocument? document = generatedDocument as RadFixedDocument;
                formatProvider.Export(document, stream);
            }
        }
        public byte[] GetPDFByte(object generatedDocument) {
            if (generatedDocument == null) {
                throw new ArgumentNullException(nameof(generatedDocument));
            }
            FontsProviderBase fontsProvider = new FontsProvider();
            FixedExtensibilityManager.FontsProvider = fontsProvider;
            PdfFormatProvider formatProvider = new PdfFormatProvider();
            formatProvider.ExportSettings.ImageQuality = ImageQuality.High;
            using MemoryStream stream = new MemoryStream();
            RadFixedDocument? document = generatedDocument as RadFixedDocument;
            formatProvider.Export(document, stream);
            var fileData = DataConverter.StreamToByte(stream);
            return fileData;

        }
        public void AddWaterMark(object generatedDocument, WaterMark waterMark) {
            if (generatedDocument == null) {
                throw new ArgumentNullException(nameof(generatedDocument));
            }
            RadFixedDocument? document = generatedDocument as RadFixedDocument;
            if (document == null) {
                return;
            }
            foreach (RadFixedPage page in document.Pages) {
                AddWatermarkText(page, waterMark);
            }

        }
        public void MergePDF(string resultFile, List<object> documentList) {
            if (File.Exists(resultFile)) {
                File.Delete(resultFile);
            }

            using (PdfStreamWriter fileWriter = new PdfStreamWriter(File.OpenWrite(resultFile))) {
                foreach (RadFixedDocument document in documentList) {
                    foreach (RadFixedPage page in document.Pages) {
                        fileWriter.WritePage(page);
                    }

                }
            }
        }
        public byte[] MergePDFToByte(List<object> documentList) {
            using MemoryStream stream = new MemoryStream();
            RadFixedDocument mergeDocument = new RadFixedDocument();
            PdfFormatProvider formatProvider = new PdfFormatProvider();
            formatProvider.ExportSettings.ImageQuality = ImageQuality.High;
            foreach (RadFixedDocument document in documentList) {
                mergeDocument.Merge(document);
            }
            formatProvider.Export(mergeDocument, stream);
            var fileData = DataConverter.StreamToByte(stream);
            return fileData;
        }
        private void AddWatermarkText(RadFixedPage page, WaterMark waterMark) {
            FixedContentEditor editor = new FixedContentEditor(page);

            Block block = new Block();
            block.TextProperties.FontSize = 80;
            block.TextProperties.TrySetFont(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold);
            block.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
            block.GraphicProperties.FillColor = new RgbColor(waterMark.Transparency, 255, 0, 0);
            block.InsertText(waterMark.Text);

            double angle = waterMark.Angle;
            editor.Position.Rotate(angle);
            editor.Position.Translate(0, page.Size.Width);
            editor.DrawBlock(block, new Size(page.Size.Width / Math.Abs(Math.Sin(angle)), double.MaxValue));
        }
    }
}
