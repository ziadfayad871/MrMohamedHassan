using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public interface IHomeworkRepository : IGenericRepository<Homework>
{
    Task<Homework?> GetHomeworkWithSubmissionsAsync(int id);
    Task<IEnumerable<Homework>> GetHomeworkByGroupAsync(int groupId);
}
