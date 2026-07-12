using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Attendance>> GetAttendanceByGroupAndDateAsync(int groupId, DateTime date)
    {
        return await _dbSet
            .Include(a => a.Student)
            .Where(a => a.GroupId == groupId && a.Date.Date == date.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetAttendanceByStudentAsync(int studentId)
    {
        return await _dbSet
            .Include(a => a.Group)
            .Where(a => a.StudentId == studentId)
            .OrderByDescending(a => a.Date)
            .ToListAsync();
    }

    public async Task<int> GetPresentCountAsync(int groupId, DateTime date)
    {
        return await _dbSet.CountAsync(a => a.GroupId == groupId && a.Date.Date == date.Date && a.Status == AttendanceStatus.Present);
    }

    public async Task<int> GetAbsentCountAsync(int groupId, DateTime date)
    {
        return await _dbSet.CountAsync(a => a.GroupId == groupId && a.Date.Date == date.Date && a.Status == AttendanceStatus.Absent);
    }

    public async Task<int> GetTodayPresentCountAsync()
    {
        return await _dbSet.CountAsync(a => a.Date.Date == DateTime.Today && a.Status == AttendanceStatus.Present);
    }

    public async Task<int> GetTodayAbsentCountAsync()
    {
        return await _dbSet.CountAsync(a => a.Date.Date == DateTime.Today && a.Status == AttendanceStatus.Absent);
    }
}
