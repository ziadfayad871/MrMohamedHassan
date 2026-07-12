using MrMohamedHassan.Models;

namespace MrMohamedHassan.Services;

public interface IGroupService
{
    Task<Group?> GetByIdAsync(int id);
    Task<IEnumerable<Group>> GetAllAsync();
    Task<Group> CreateAsync(Group group);
    Task UpdateAsync(Group group);
    Task SoftDeleteAsync(int id);
    Task<Group?> GetGroupWithDetailsAsync(int id);
    Task<int> GetStudentCountAsync(int groupId);
    Task<IEnumerable<Group>> GetActiveGroupsAsync();
    Task AssignStudentsAsync(int groupId, List<int> studentIds);
}
