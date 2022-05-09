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
    public class PdfProcessing: IPdfProcessing {
        public PdfProcessing() {

        }
        public void ExportToPDF(string filePath,string resultFile, object generatedDocument) {
            if (generatedDocument == null) {
                throw new ArgumentNullException(nameof(generatedDocument));
            }
            FontsProviderBase fontsProvider = new FontsProvider();
            FixedExtensibilityManager.FontsProvider = fontsProvider;
            PdfFormatProvider formatProvider = new PdfFormatProvider();
            formatProvider.ExportSettings.ImageQuality = ImageQuality.High;
            this.PrepareDirectory(filePath, resultFile);
            using (FileStream stream = File.OpenWrite(resultFile)) {
                RadFixedDocument document = generatedDocument as RadFixedDocument;
                formatProvider.Export(document, stream);
            }
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = resultFile,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        public void AddWaterMark(object generatedDocument, WaterMark waterMark) {
            if (generatedDocument == null) {
                throw new ArgumentNullException(nameof(generatedDocument));
            }
            RadFixedDocument document = generatedDocument as RadFixedDocument;
            foreach(RadFixedPage page in document.Pages) {
                AddWatermarkText(page, waterMark);
            }
           
        }
        public void MergePDF(string resultFile,List<object> documentList) {
            if (File.Exists(resultFile)) {
                File.Delete(resultFile);
            }
            using (PdfStreamWriter fileWriter = new PdfStreamWriter(File.OpenWrite(resultFile))) {
                foreach(RadFixedDocument document in documentList) {
                    foreach (RadFixedPage page in document.Pages) {
                        fileWriter.WritePage(page);
                    }

                }
               
            }

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = resultFile,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        private void AddWatermarkText(RadFixedPage page,WaterMark waterMark) {
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
        private void PrepareDirectory(string filePath, string resultFile) {
            if (Directory.Exists(filePath)) {
                if (File.Exists(resultFile)) {
                    File.Delete(resultFile);
                }
            } else {
                Directory.CreateDirectory(filePath);
            }
        }
    }
}
