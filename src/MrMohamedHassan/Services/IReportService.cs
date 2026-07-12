namespace MrMohamedHassan.Services;

public interface IReportService
{
    Task<byte[]> GenerateStudentReportAsync(int studentId);
    Task<byte[]> GenerateAttendanceReportAsync(int groupId, DateTime start, DateTime end);
    Task<byte[]> GeneratePaymentReportAsync(DateTime start, DateTime end);
    Task<byte[]> GenerateFinancialReportAsync(DateTime start, DateTime end);
    Task<byte[]> GenerateExamReportAsync(int examId);
}
