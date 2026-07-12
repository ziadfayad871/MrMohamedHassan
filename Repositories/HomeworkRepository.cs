using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public class HomeworkRepository : GenericRepository<Homework>, IHomeworkRepository
{
    public HomeworkRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Homework?> GetHomeworkWithSubmissionsAsync(int id)
    {
        return await _dbSet
            .Include(h => h.Group)
            .Include(h => h.Submissions).ThenInclude(s => s.Student)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<IEnumerable<Homework>> GetHomeworkByGroupAsync(int groupId)
    {
        return await _dbSet
            .Include(h => h.Group)
            .Where(h => h.GroupId == groupId)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();
    }
}
