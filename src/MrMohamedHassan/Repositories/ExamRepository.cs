using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public class ExamRepository : GenericRepository<Exam>, IExamRepository
{
    public ExamRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Exam?> GetExamWithResultsAsync(int id)
    {
        return await _dbSet
            .Include(e => e.Group)
            .Include(e => e.Results).ThenInclude(r => r.Student)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Exam>> GetExamsByGroupAsync(int groupId)
    {
        return await _dbSet
            .Include(e => e.Group)
            .Where(e => e.GroupId == groupId)
            .OrderByDescending(e => e.ExamDate)
            .ToListAsync();
    }
}
