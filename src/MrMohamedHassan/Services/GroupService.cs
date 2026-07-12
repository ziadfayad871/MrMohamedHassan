using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;
using MrMohamedHassan.Repositories;

namespace MrMohamedHassan.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly ApplicationDbContext _context;

    public GroupService(IGroupRepository groupRepository, ApplicationDbContext context)
    {
        _groupRepository = groupRepository;
        _context = context;
    }

    public async Task<Group?> GetByIdAsync(int id) => await _groupRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Group>> GetAllAsync() => await _groupRepository.GetAllAsync();

    public async Task<Group> CreateAsync(Group group) => await _groupRepository.AddAsync(group);

    public async Task UpdateAsync(Group group) => await _groupRepository.UpdateAsync(group);

    public async Task SoftDeleteAsync(int id)
    {
        var group = await _groupRepository.GetByIdAsync(id);
        if (group != null)
        {
            group.IsDeleted = true;
            await _groupRepository.UpdateAsync(group);
        }
    }

    public async Task<Group?> GetGroupWithDetailsAsync(int id)
        => await _groupRepository.GetGroupWithDetailsAsync(id);

    public async Task<int> GetStudentCountAsync(int groupId)
        => await _groupRepository.GetStudentCountAsync(groupId);

    public async Task<IEnumerable<Group>> GetActiveGroupsAsync()
        => await _groupRepository.GetActiveGroupsAsync();

    public async Task AssignStudentsAsync(int groupId, List<int> studentIds)
    {
        var existing = await _context.StudentGroups
            .Where(sg => sg.GroupId == groupId)
            .ToListAsync();

        foreach (var sg in existing)
        {
            sg.IsActive = studentIds.Contains(sg.StudentId);
        }

        foreach (var studentId in studentIds)
        {
            if (!existing.Any(e => e.StudentId == studentId))
            {
                _context.StudentGroups.Add(new StudentGroup
                {
                    GroupId = groupId,
                    StudentId = studentId,
                    IsActive = true,
                    EnrolledAt = DateTime.UtcNow
                });
            }
        }
        await _context.SaveChangesAsync();
    }
}
