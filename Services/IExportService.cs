namespace MrMohamedHassan.Services;

public interface IExportService
{
    byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName, Dictionary<string, string>? columnHeaders = null);
    byte[] ExportToPdf(string htmlContent, string title);
}
