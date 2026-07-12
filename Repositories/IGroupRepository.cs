using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public interface IGroupRepository : IGenericRepository<Group>
{
    Task<Group?> GetGroupWithDetailsAsync(int id);
    Task<int> GetStudentCountAsync(int groupId);
    Task<IEnumerable<Group>> GetActiveGroupsAsync();
}
