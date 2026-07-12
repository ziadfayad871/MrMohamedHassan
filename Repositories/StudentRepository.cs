using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Student?> GetStudentWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(s => s.StudentGroups).ThenInclude(sg => sg.Group)
            .Include(s => s.Payments)
            .Include(s => s.Attendances)
            .Include(s => s.ExamResults).ThenInclude(er => er.Exam)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Student>> GetStudentsByGroupAsync(int groupId)
    {
        return await _dbSet
            .Where(s => s.StudentGroups.Any(sg => sg.GroupId == groupId && sg.IsActive) && !s.IsDeleted)
            .ToListAsync();
    }

    public async Task<string> GenerateStudentCodeAsync()
    {
        var count = await _dbSet.CountAsync();
        return $"STU-{(count + 1):D5}";
    }

    public async Task<int> GetActiveStudentCountAsync()
    {
        return await _dbSet.CountAsync(s => s.Status == StudentStatus.Active && !s.IsDeleted);
    }
}
