using API;
using DocumentProcessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using Telerik.Documents.Common.Model;
using Telerik.Documents.Core.Fonts;
using Telerik.Documents.Media;
using Telerik.Documents.Primitives;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Editing;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;
using Telerik.Zip;

[ApiController]
[Route("documentprocessing")]
[Authorize]
public class DocumentProcessingController : ControllerBase {

    public static readonly string currentUserTempPath = System.IO.Path.GetTempPath();
    private readonly IPdfProcessing _pdfProcessing;
    private readonly IWordProcessing _wordProcessing;
    private readonly ISpreadProcessing _spreadProcessing;
    private readonly IZipProcessing _zipProcessing;
    private static readonly List<FileDetails> _uploadedFiles =new List<FileDetails>();
    public DocumentProcessingController(IPdfProcessing pdfProcessing, IWordProcessing wordProcessing, ISpreadProcessing spreadProcessing, IZipProcessing zipProcessing) {
        _pdfProcessing = pdfProcessing;
        _wordProcessing = wordProcessing;
        _spreadProcessing = spreadProcessing;
        _zipProcessing = zipProcessing;
    }
    [HttpGet]
    [Route("exporttopdf")]
    [AccessCodeAuthorize("AA01")]
    public async Task<IActionResult> ExportToPDF() {
        RadFixedDocument? document = DocumentGenerator.CreateDocument();
        WaterMark waterMark = new WaterMark {
            Text = "Water Mark Demo",
            Transparency = 100,
            Angle = -45
        };
        _pdfProcessing.AddWaterMark(document, waterMark);
        var pdfFile = _pdfProcessing.GetPDFByte(document);
        return File(pdfFile, "application/pdf");
    }
    [HttpGet]
    [Route("mergepdf")]
    [AccessCodeAuthorize("AA01")]
    public async Task<IActionResult> MergePDF() {
        RadFixedDocument? document = DocumentGenerator.CreateDocument();
        List<object> documentList = new List<object> {
            document,
            document
        };
        var pdfFile = _pdfProcessing.MergePDFToByte(documentList);
        return File(pdfFile, "application/pdf");
    }
    [HttpGet]
    [Route("exporttodocx")]
    [AccessCodeAuthorize("AA01")]
    public async Task<IActionResult> ExportToDocx() {
        RadFlowDocument? document = DocumentGenerator.CreateFlowDocument();
        var wordFile =_wordProcessing.GetWordByte(DocumentFormat.docx, document);
        if (wordFile == null) {
            return NotFound();
        }
        return File(wordFile, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
    }
    [HttpGet]
    [Route("exporttoxlsx")]
    [AccessCodeAuthorize("AA01")]
    public async Task<IActionResult> ExportToXlsx() {
        Workbook workbook = DocumentGenerator.CreateWorkbook();
        var excelFile =_spreadProcessing.GetXlsxByte(workbook);
        if (excelFile == null) {
            return NotFound();
        }
        return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
    [HttpGet]
    [Route("createzip")]
    [AccessCodeAuthorize("AA01")]
    public async Task<IActionResult> CreateZip() {
        string[] files = Directory.GetFiles(currentUserTempPath).Take(20).ToArray();
        using MemoryStream stream = new MemoryStream();
        var zipFile =_zipProcessing.GetZipBytes(stream, files);
        return File(zipFile, "application/x-zip-compressed");
    }
    [HttpPost]
    [Route("uploadfiles")]
    [AccessCodeAuthorize("AA01")]
    public async Task<IActionResult> UploadFiles(IEnumerable<IFormFile> files) {
        if (files != null) {
            IFormFile? file =files.FirstOrDefault();
            if (file == null) {
                return new NotFoundResult();
            }
            FileDetails fileDetails = new FileDetails {
                Name = ContentDispositionHeaderValue.Parse(file.ContentDisposition)?.FileName?.ToString()?.Trim('"') ?? ""
            };
            fileDetails.Data = fileDetails.ReadToEnd(file.OpenReadStream());
            _uploadedFiles.Add(fileDetails);
        }
        return new OkResult();
    }
    [HttpGet]
    [Route("zipfiles")]
    [AccessCodeAuthorize("AA01")]
    public async Task<IActionResult> ZipFiles(string password) {
        DeflateSettings compressionSettings = new DeflateSettings {
            CompressionLevel = CompressionLevel.Best,
            HeaderType = CompressedStreamHeader.ZLib
        };
        DefaultEncryptionSettings encryptionSettings = new DefaultEncryptionSettings {
            Password = password
        };
        Dictionary<string, Stream> zipArchiveFiles=new Dictionary<string, Stream>();
        foreach (FileDetails? fileItem in _uploadedFiles) {
            zipArchiveFiles[fileItem.Name] = new MemoryStream(fileItem.Data);
        }
        var zipFile = _zipProcessing.GetZipBytes(zipArchiveFiles, entryNameEncoding: null, compressionSettings, encryptionSettings);
        return File(zipFile, "application/x-zip-compressed");
    }

}


public static class DocumentGenerator {
    private static readonly double defaultLeftIndent = 50;
    private static readonly double defaultLineHeight = 16;
    private static readonly int IndexColumnQuantity = 5;
    private static readonly int IndexColumnUnitPrice = 6;
    private static readonly int IndexColumnSubTotal = 7;
    private static readonly int IndexRowItemStart = 1;

    private static readonly string AccountFormatString = GenerateCultureDependentFormatString();
    private static readonly ThemableColor InvoiceBackground = new ThemableColor(Color.FromArgb(255, 44, 62, 80));
    private static readonly ThemableColor InvoiceHeaderForeground = new ThemableColor(Color.FromArgb(255, 255, 255, 255));


    public static RadFixedDocument CreateDocument() {
        RadFixedDocument document = new RadFixedDocument();
        RadFixedPage page = document.Pages.AddPage();
        page.Size = new Size(600, 800);
        FixedContentEditor editor = new FixedContentEditor(page);
        editor.Position.Translate(defaultLeftIndent, 50);
        double currentTopOffset = 110;
        editor.Position.Translate(defaultLeftIndent, currentTopOffset);
        double maxWidth = page.Size.Width - defaultLeftIndent * 2;
        DrawDescription(editor, maxWidth);
        currentTopOffset += defaultLineHeight * 4;
        editor.Position.Translate(defaultLeftIndent, currentTopOffset);
        using (editor.SaveProperties()) {
            DrawFunnelFigure(editor);
        }
        editor.Position.Translate(defaultLeftIndent * 4, page.Size.Height - 100);
        DrawText(editor, maxWidth);
        return document;
    }
    private static void DrawDescription(FixedContentEditor editor, double maxWidth) {
        Block block = new Block();
        block.GraphicProperties.FillColor = RgbColors.Black;
        block.HorizontalAlignment = HorizontalAlignment.Left;
        block.TextProperties.FontSize = 14;
        block.TextProperties.TrySetFont(new FontFamily("Calibri"), FontStyles.Italic, FontWeights.Bold);
        block.InsertText("Document Processing");
        block.TextProperties.TrySetFont(new FontFamily("Calibri"));
        block.InsertText(" is a document processing library that enables your application to import and export files to and from PDF format. The document model is entirely independent from UI and allows you to generate sleek documents with differently formatted text, images, shapes and more.");

        editor.DrawBlock(block, new Size(maxWidth, double.PositiveInfinity));
    }
    private static void DrawFunnelFigure(FixedContentEditor editor) {
        editor.GraphicProperties.IsStroked = false;
        editor.GraphicProperties.FillColor = new RgbColor(231, 238, 247);
        editor.DrawEllipse(new Point(250, 70), 136, 48);

        editor.GraphicProperties.IsStroked = true;
        editor.GraphicProperties.StrokeColor = RgbColors.White;
        editor.GraphicProperties.StrokeThickness = 1;
        editor.GraphicProperties.FillColor = new RgbColor(91, 155, 223);
        editor.DrawEllipse(new Point(289, 77), 48, 48);

        editor.Position.Translate(291, 204);
        CenterText(editor, "Fonts");

        editor.Position.Translate(0, 0);
        editor.DrawEllipse(new Point(238, 274), 48, 48);
        editor.Position.Translate(190, 226);
        CenterText(editor, "Images");

        editor.Position.Translate(0, 0);
        editor.DrawEllipse(new Point(307, 347), 48, 48);
        editor.Position.Translate(259, 299);
        CenterText(editor, "Shapes");

        editor.Position.Translate(0, 0);
        PathGeometry arrow = new PathGeometry();
        PathFigure figure = arrow.Figures.AddPathFigure();
        figure.StartPoint = new Point(287, 422);
        figure.IsClosed = true;
        figure.Segments.AddLineSegment(new Point(287, 438));
        figure.Segments.AddLineSegment(new Point(278, 438));
        figure.Segments.AddLineSegment(new Point(300, 454));
        figure.Segments.AddLineSegment(new Point(322, 438));
        figure.Segments.AddLineSegment(new Point(313, 438));
        figure.Segments.AddLineSegment(new Point(313, 422));

        editor.DrawPath(arrow);

        editor.GraphicProperties.FillColor = new RgbColor(80, 255, 255, 255);
        editor.GraphicProperties.IsStroked = true;
        editor.GraphicProperties.StrokeThickness = 1;
        editor.GraphicProperties.StrokeColor = new RgbColor(91, 155, 223);

        PathGeometry funnel = new PathGeometry {
            FillRule = FillRule.EvenOdd
        };
        figure = funnel.Figures.AddPathFigure();
        figure.IsClosed = true;
        figure.StartPoint = new Point(164, 245);
        figure.Segments.AddArcSegment(new Point(436, 245), 136, 48);
        figure.Segments.AddArcSegment(new Point(164, 245), 136, 48);

        figure = funnel.Figures.AddPathFigure();
        figure.IsClosed = true;
        figure.StartPoint = new Point(151, 245);
        figure.Segments.AddArcSegment(new Point(449, 245), 149, 61);
        figure.Segments.AddLineSegment(new Point(332, 415));
        figure.Segments.AddArcSegment(new Point(268, 415), 16, 4);

        editor.DrawPath(funnel);

        editor.Position.Translate(164, 455);
        Block block = new Block();
        block.TextProperties.Font = editor.TextProperties.Font;
        block.GraphicProperties.FillColor = RgbColors.Black;
        block.HorizontalAlignment = HorizontalAlignment.Center;
        block.VerticalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment.Top;
        block.TextProperties.FontSize = 18;
        block.InsertText("Document");
        editor.DrawBlock(block, new Size(272, double.PositiveInfinity));
    }
    private static void CenterText(FixedContentEditor editor, string text) {
        Block block = new Block();
        block.TextProperties.TrySetFont(new FontFamily("Calibri"));
        block.HorizontalAlignment = HorizontalAlignment.Center;
        block.VerticalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment.Center;
        block.GraphicProperties.FillColor = RgbColors.White;
        block.InsertText(text);

        editor.DrawBlock(block, new Size(96, 96));
    }
    private static void DrawText(FixedContentEditor editor, double maxWidth) {
        double currentTopOffset = 470;
        currentTopOffset += defaultLineHeight * 2;
        editor.Position.Translate(defaultLeftIndent, currentTopOffset);
        editor.TextProperties.FontSize = 11;

        Block block = new Block();
        block.TextProperties.FontSize = 11;
        block.TextProperties.TrySetFont(new FontFamily("Arial"));
        block.InsertText("A wizard's job is to vex ");
        using (block.GraphicProperties.Save()) {
            block.GraphicProperties.FillColor = new RgbColor(255, 146, 208, 80);
            block.InsertText("chumps");
        }

        block.InsertText(" quickly in fog.");
        editor.DrawBlock(block, new Size(maxWidth, double.PositiveInfinity));

        currentTopOffset += defaultLineHeight;
        editor.Position.Translate(defaultLeftIndent, currentTopOffset);

        block = new Block();
        block.TextProperties.FontSize = 11;
        block.TextProperties.TrySetFont(new FontFamily("Trebuchet MS"));
        block.InsertText("A wizard's job is to vex chumps ");
        using (block.TextProperties.Save()) {
            block.TextProperties.UnderlinePattern = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.UnderlinePattern.Single;
            block.TextProperties.UnderlineColor = editor.GraphicProperties.FillColor;
            block.InsertText("quickly");
        }

        block.InsertText(" in fog.");
        editor.DrawBlock(block, new Size(maxWidth, double.PositiveInfinity));

        currentTopOffset += defaultLineHeight;
        editor.Position.Translate(defaultLeftIndent, currentTopOffset);
        block = new Block();
        block.TextProperties.FontSize = 11;
        block.TextProperties.TrySetFont(new FontFamily("Algerian"));
        block.InsertText("A ");
        using (block.TextProperties.Save()) {
            block.TextProperties.UnderlinePattern = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.UnderlinePattern.Single;
            block.TextProperties.UnderlineColor = editor.GraphicProperties.FillColor;
            block.InsertText("wizard's");
        }

        block.InsertText(" job is to vex chumps quickly in fog.");
        editor.DrawBlock(block, new Size(maxWidth, double.PositiveInfinity));

        currentTopOffset += defaultLineHeight;
        editor.Position.Translate(defaultLeftIndent, currentTopOffset);

        editor.TextProperties.TrySetFont(new FontFamily("Lucida Calligraphy"));
        editor.DrawText("A wizard's job is to vex chumps quickly in fog.", new Size(maxWidth, double.PositiveInfinity));

        currentTopOffset += defaultLineHeight + 2;
        editor.Position.Translate(defaultLeftIndent, currentTopOffset);
        block = new Block();
        block.TextProperties.FontSize = 11;
        block.TextProperties.TrySetFont(new FontFamily("Consolas"));
        block.InsertText("A wizard's job is to vex chumps ");
        using (block.TextProperties.Save()) {
            block.TextProperties.TrySetFont(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Bold);
            block.InsertText("quickly");
        }

        block.InsertText(" in fog.");
        editor.DrawBlock(block, new Size(maxWidth, double.PositiveInfinity));

        currentTopOffset += defaultLineHeight;
        editor.Position.Translate(defaultLeftIndent, currentTopOffset);
        editor.TextProperties.TrySetFont(new FontFamily("Arial"));

        editor.DrawText("Document testing", new Size(maxWidth, double.PositiveInfinity));

        currentTopOffset += defaultLineHeight;
        editor.Position.Translate(defaultLeftIndent, currentTopOffset);
        editor.DrawText("Document testing", new Size(maxWidth, double.PositiveInfinity));

        currentTopOffset += defaultLineHeight;
        editor.Position.Translate(defaultLeftIndent, currentTopOffset);
        editor.DrawText("Document testing", new Size(maxWidth, double.PositiveInfinity));

    }
    public static RadFlowDocument CreateFlowDocument() {
        RadFlowDocument document = new RadFlowDocument();
        RadFlowDocumentEditor editor = new RadFlowDocumentEditor(document);
        editor.ParagraphFormatting.TextAlignment.LocalValue = Alignment.Justified;

        // Body
        editor.InsertLine("Dear Telerik User,");
        editor.InsertText("We’re happy to introduce the Telerik RadWordsProcessing component. High performance library that enables you to read, write and manipulate documents in DOCX, RTF and plain text format. The document model is independent from UI and ");
        Run run = editor.InsertText("does not require");
        run.Underline.Pattern = Telerik.Windows.Documents.Flow.Model.Styles.UnderlinePattern.Single;
        editor.InsertLine(" Microsoft Office.");

        editor.InsertText("The current community preview version comes with full rich-text capabilities including ");
        editor.InsertText("bold, ").FontWeight = FontWeights.Bold;
        editor.InsertText("italic, ").FontStyle = FontStyles.Italic;
        editor.InsertText("underline,").Underline.Pattern = Telerik.Windows.Documents.Flow.Model.Styles.UnderlinePattern.Single;
        editor.InsertText(" font sizes and ").FontSize = Telerik.Windows.Documents.Media.Unit.PointToDip(20);
        Run coloredRun = editor.InsertText("colors ");
        coloredRun.ForegroundColor = new ThemableColor(Color.FromArgb(255, 92, 230, 0));
        coloredRun.Shading.BackgroundColor = new ThemableColor(Color.FromArgb(255, 255, 255, 0));

        editor.InsertLine("as well as text alignment and indentation. Other options include tables, hyperlinks, inline and floating images. Even more sweetness is added by the built-in styles and themes.");

        editor.InsertText("Here at Telerik we strive to provide the best services possible and fulfill all needs you as a customer may have. We would appreciate any feedback you send our way through the ");
        editor.InsertHyperlink("public forums", "http://www.telerik.com/forums", false, "Telerik Forums");
        editor.InsertLine(" or support ticketing system.");

        editor.InsertLine("We hope you’ll enjoy RadWordsProcessing as much as we do. Happy coding!");
        editor.InsertParagraph();
        editor.InsertText("Kind regards,");

        CreateHeader(editor);

        CreateFooter(editor);

        return document;
    }
    private static void CreateFooter(RadFlowDocumentEditor editor) {
        Footer footer = editor.Document.Sections.First().Footers.Add();
        Paragraph paragraph = footer.Blocks.AddParagraph();
        paragraph.TextAlignment = Alignment.Right;
        paragraph.Inlines.AddRun("www.telerik.com");

        editor.MoveToParagraphStart(paragraph);
    }

    private static void CreateHeader(RadFlowDocumentEditor editor) {
        Header header = editor.Document.Sections.First().Headers.Add();
        Paragraph paragraph = header.Blocks.AddParagraph();
        paragraph.TextAlignment = Alignment.Right;
        paragraph.Inlines.AddRun("Demo");

        editor.MoveToParagraphStart(header.Blocks.AddParagraph());
    }
    public static Workbook CreateWorkbook() {
        Workbook workbook = new Workbook();
        workbook.Sheets.Add(SheetType.Worksheet);

        Worksheet worksheet = workbook.ActiveWorksheet;
        List<Product>? products =new Products().GetData(20).ToList();
        PrepareInvoiceDocument(worksheet, products.Count);

        int currentRow = IndexRowItemStart + 1;
        foreach (Product product in products) {
            worksheet.Cells[currentRow, 0].SetValue(product.Name);
            worksheet.Cells[currentRow, IndexColumnQuantity].SetValue(product.Quantity);
            worksheet.Cells[currentRow, IndexColumnUnitPrice].SetValue(product.UnitPrice);
            worksheet.Cells[currentRow, IndexColumnSubTotal].SetValue(product.SubTotal);

            currentRow++;
        }

        return workbook;
    }
    private static void PrepareInvoiceDocument(Worksheet worksheet, int itemsCount) {
        int lastItemIndexRow = IndexRowItemStart + itemsCount;

        CellIndex firstUsedCellIndex = new CellIndex(0, 0);
        CellIndex lastUsedCellIndex = new CellIndex(lastItemIndexRow + 1, IndexColumnSubTotal);
        CellBorder border = new CellBorder(CellBorderStyle.DashDot, InvoiceBackground);
        worksheet.Cells[firstUsedCellIndex, lastUsedCellIndex].SetBorders(new CellBorders(border, border, border, border, null, null, null, null));

        worksheet.Cells[firstUsedCellIndex].SetValue("INVOICE");
        worksheet.Cells[firstUsedCellIndex].SetFontSize(20);
        worksheet.Cells[firstUsedCellIndex].SetHorizontalAlignment(RadHorizontalAlignment.Center);
        worksheet.Cells[0, 0, 0, IndexColumnSubTotal].MergeAcross();

        worksheet.Columns[IndexColumnUnitPrice].SetWidth(new ColumnWidth(125, true));
        worksheet.Columns[IndexColumnSubTotal].SetWidth(new ColumnWidth(125, true));

        worksheet.Cells[IndexRowItemStart, 0].SetValue("Item");
        worksheet.Cells[IndexRowItemStart, IndexColumnQuantity].SetValue("QTY");
        worksheet.Cells[IndexRowItemStart, IndexColumnUnitPrice].SetValue("Unit Price");
        worksheet.Cells[IndexRowItemStart, IndexColumnSubTotal].SetValue("SubTotal");
        worksheet.Cells[IndexRowItemStart, 0, lastItemIndexRow, IndexColumnQuantity - 1].MergeAcross();

        worksheet.Cells[IndexRowItemStart, 0, IndexRowItemStart, IndexColumnSubTotal].SetFill
            (new GradientFill(GradientType.Horizontal, InvoiceBackground, InvoiceBackground));
        worksheet.Cells[IndexRowItemStart, 0, IndexRowItemStart, IndexColumnSubTotal].SetForeColor(InvoiceHeaderForeground);
        worksheet.Cells[IndexRowItemStart, IndexColumnUnitPrice, lastItemIndexRow, IndexColumnSubTotal].SetFormat(
            new CellValueFormat(AccountFormatString));

        worksheet.Cells[lastItemIndexRow + 1, 6].SetValue("TOTAL: ");
        worksheet.Cells[lastItemIndexRow + 1, 7].SetFormat(new CellValueFormat(AccountFormatString));

        string subTotalColumnCellRange = NameConverter.ConvertCellRangeToName(
                new CellIndex(IndexRowItemStart + 1, IndexColumnSubTotal),
                new CellIndex(lastItemIndexRow, IndexColumnSubTotal));

        worksheet.Cells[lastItemIndexRow + 1, IndexColumnSubTotal].SetValue(string.Format("=SUM({0})", subTotalColumnCellRange));

        worksheet.Cells[lastItemIndexRow + 1, IndexColumnUnitPrice, lastItemIndexRow + 1, IndexColumnSubTotal].SetFontSize(20);
    }
    private static string GenerateCultureDependentFormatString() {
        string gS = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
        string dS = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        return "_($* #" + gS + "##0" + dS + "00_);_($* (#" + gS + "##0" + dS + "00);_($* \"-\"??_);_(@_)";
    }
}
public class Product {
    private int id;
    private string name;
    private double unitPrice;
    private int quantity;
    private DateTime date;
    private double subTotal;

    public Product(int id, string name, double unitPrice, int quantity, DateTime date) {
        ID = id;
        Name = name;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Date = date;
        SubTotal = this.quantity * this.unitPrice;
    }

    public int ID {
        get => id;
        set {
            if (id != value) {
                id = value;
            }
        }
    }

    public string Name {
        get => name;
        set {
            if (name != value) {
                name = value;
            }
        }
    }

    public double UnitPrice {
        get => unitPrice;
        set {
            if (unitPrice != value) {
                unitPrice = value;
            }
        }
    }

    public int Quantity {
        get => quantity;
        set {
            if (quantity != value) {
                quantity = value;
            }
        }
    }

    public DateTime Date {
        get => date;
        set {
            if (date != value) {
                date = value;
            }
        }
    }

    public double SubTotal {
        get => subTotal;
        set {
            if (subTotal != value) {
                subTotal = value;
            }
        }
    }
}
public class Products {
    private static readonly string[] names = new string[] { "Côte de Blaye", "Boston Crab Meat",
            "Singaporean Hokkien Fried Mee", "Gula Malacca", "Rogede sild",
            "Spegesild", "Zaanse koeken", "Chocolade", "Maxilaku", "Valkoinen suklaa" };
    private static readonly double[] prizes = new double[] { 23.2500, 9.0000, 45.6000, 32.0000,
            14.0000, 19.0000, 263.5000, 18.4000, 3.0000, 14.0000 };
    private static readonly DateTime[] dates = new DateTime[] { new DateTime(2007, 5, 10), new DateTime(2008, 9, 13),
            new DateTime(2008, 2, 22), new DateTime(2009, 1, 2), new DateTime(2007, 4, 13),
            new DateTime(2008, 5, 12), new DateTime(2008, 1, 19), new DateTime(2008, 8, 26),
            new DateTime(2008, 7, 31), new DateTime(2007, 7, 16) };


    public IEnumerable<Product> GetData(int itemCount) {
        Random rnd = new Random();

        return from i in Enumerable.Range(1, itemCount)
               select new Product(i,
                   names[rnd.Next(9)],
                   prizes[rnd.Next(9)],
                   rnd.Next(1, 9),
                   dates[rnd.Next(9)]);
    }
}
public class FileDetails {
    public string Name { get; set; }
    public byte[] Data { get; set; }
    public byte[] ReadToEnd(System.IO.Stream stream) {
        long originalPosition = 0;

        if (stream.CanSeek) {
            originalPosition = stream.Position;
            stream.Position = 0;
        }

        try {
            byte[] readBuffer = new byte[4096];

            int totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0) {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length) {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1) {
                        byte[] temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead) {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }
            return buffer;
        } finally {
            if (stream.CanSeek) {
                stream.Position = originalPosition;
            }
        }
    }

}