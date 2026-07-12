using MrMohamedHassan.Models;

namespace MrMohamedHassan.Services;

public interface IAttendanceService
{
    Task<IEnumerable<Attendance>> GetAttendanceByGroupAndDateAsync(int groupId, DateTime date);
    Task SaveAttendanceAsync(List<Attendance> attendanceList);
    Task<int> GetTodayPresentCountAsync();
    Task<int> GetTodayAbsentCountAsync();
    Task<IEnumerable<Attendance>> GetAttendanceByStudentAsync(int studentId);
    Task<Dictionary<string, int>> GetMonthlyAttendanceStatsAsync(int groupId, int month, int year);
}
