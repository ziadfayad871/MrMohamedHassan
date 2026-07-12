using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;
using MrMohamedHassan.Repositories;

namespace MrMohamedHassan.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly ApplicationDbContext _context;

    public AttendanceService(IAttendanceRepository attendanceRepository, ApplicationDbContext context)
    {
        _attendanceRepository = attendanceRepository;
        _context = context;
    }

    public async Task<IEnumerable<Attendance>> GetAttendanceByGroupAndDateAsync(int groupId, DateTime date)
        => await _attendanceRepository.GetAttendanceByGroupAndDateAsync(groupId, date);

    public async Task SaveAttendanceAsync(List<Attendance> attendanceList)
    {
        foreach (var att in attendanceList)
        {
            var existing = await _context.Attendances
                .FirstOrDefaultAsync(a => a.StudentId == att.StudentId && a.GroupId == att.GroupId && a.Date.Date == att.Date.Date);

            if (existing != null)
            {
                existing.Status = att.Status;
                existing.Notes = att.Notes;
                existing.CheckInTime = att.CheckInTime;
                existing.RecordedById = att.RecordedById;
            }
            else
            {
                _context.Attendances.Add(att);
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetTodayPresentCountAsync() => await _attendanceRepository.GetTodayPresentCountAsync();

    public async Task<int> GetTodayAbsentCountAsync() => await _attendanceRepository.GetTodayAbsentCountAsync();

    public async Task<IEnumerable<Attendance>> GetAttendanceByStudentAsync(int studentId)
        => await _attendanceRepository.GetAttendanceByStudentAsync(studentId);

    public async Task<Dictionary<string, int>> GetMonthlyAttendanceStatsAsync(int groupId, int month, int year)
    {
        var stats = await _context.Attendances
            .Where(a => a.GroupId == groupId && a.Date.Month == month && a.Date.Year == year)
            .GroupBy(a => a.Status)
            .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
            .ToListAsync();

        return stats.ToDictionary(s => s.Status, s => s.Count);
    }
}
