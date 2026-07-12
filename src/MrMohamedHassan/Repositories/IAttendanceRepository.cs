using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public interface IAttendanceRepository : IGenericRepository<Attendance>
{
    Task<IEnumerable<Attendance>> GetAttendanceByGroupAndDateAsync(int groupId, DateTime date);
    Task<IEnumerable<Attendance>> GetAttendanceByStudentAsync(int studentId);
    Task<int> GetPresentCountAsync(int groupId, DateTime date);
    Task<int> GetAbsentCountAsync(int groupId, DateTime date);
    Task<int> GetTodayPresentCountAsync();
    Task<int> GetTodayAbsentCountAsync();
}
