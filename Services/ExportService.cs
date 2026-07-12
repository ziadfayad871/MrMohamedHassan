using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MrMohamedHassan.Services;

public class ExportService : IExportService
{
    public byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName, Dictionary<string, string>? columnHeaders = null)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        var properties = typeof(T).GetProperties();
        var headers = columnHeaders ?? properties.ToDictionary(p => p.Name, p => p.Name);

        for (int col = 0; col < headers.Count; col++)
        {
            var header = headers.ElementAt(col);
            worksheet.Cell(1, col + 1).Value = header.Value;
            worksheet.Cell(1, col + 1).Style.Font.Bold = true;
            worksheet.Cell(1, col + 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            worksheet.Cell(1, col + 1).Style.Font.FontColor = XLColor.White;
        }

        var row = 2;
        foreach (var item in data)
        {
            for (int col = 0; col < headers.Count; col++)
            {
                var prop = properties.FirstOrDefault(p => p.Name == headers.ElementAt(col).Key);
                if (prop != null)
                {
                    var value = prop.GetValue(item);
                    worksheet.Cell(row, col + 1).Value = value?.ToString() ?? "";
                }
            }
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportToPdf(string htmlContent, string title)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(20);

                page.Header().Element(header =>
                {
                    header.AlignCenter().Text(title)
                        .FontSize(18)
                        .Bold()
                        .FontColor(Colors.Blue.Medium);
                });

                page.Content().Element(content =>
                {
                    content.AlignCenter().PaddingVertical(10).Text(htmlContent).FontSize(10);
                });

                page.Footer().Element(footer =>
                {
                    footer.AlignCenter().Text(text =>
                    {
                        text.Span("صفحة ");
                        text.CurrentPageNumber();
                        text.Span(" من ");
                        text.TotalPages();
                    });
                });
            });
        });

        using var stream = new MemoryStream();
        document.GeneratePdf(stream);
        return stream.ToArray();
    }
}
