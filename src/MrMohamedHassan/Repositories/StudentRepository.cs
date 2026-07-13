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

    public async Task<string> GenerateStudentCodeAsync(int? groupId = null)
    {
        if (groupId == null)
        {
            var count = await _dbSet.CountAsync();
            return $"N{count + 1}";
        }

        var letter = IdToLetter(groupId.Value);
        var prefix = letter;
        var existing = await _dbSet
            .Where(s => s.StudentCode.StartsWith(prefix) && !s.IsDeleted)
            .Select(s => s.StudentCode)
            .ToListAsync();

        var maxNum = 0;
        foreach (var code in existing)
        {
            var numPart = code[prefix.Length..];
            if (int.TryParse(numPart, out var n) && n > maxNum)
                maxNum = n;
        }

        return $"{prefix}{maxNum + 1}";
    }

    private static string IdToLetter(int id)
    {
        var result = "";
        var n = id;
        while (n > 0)
        {
            n--;
            result = (char)('A' + n % 26) + result;
            n /= 26;
        }
        return result;
    }

    public async Task<int> GetActiveStudentCountAsync()
    {
        return await _dbSet.CountAsync(s => s.Status == StudentStatus.Active && !s.IsDeleted);
    }
}
