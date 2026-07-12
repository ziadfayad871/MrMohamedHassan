using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public class GroupRepository : GenericRepository<Group>, IGroupRepository
{
    public GroupRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Group?> GetGroupWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(g => g.Teacher)
            .Include(g => g.StudentGroups).ThenInclude(sg => sg.Student)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<int> GetStudentCountAsync(int groupId)
    {
        return await _context.StudentGroups
            .CountAsync(sg => sg.GroupId == groupId && sg.IsActive);
    }

    public async Task<IEnumerable<Group>> GetActiveGroupsAsync()
    {
        return await _dbSet.Where(g => g.IsActive).Include(g => g.Teacher).ToListAsync();
    }
}
