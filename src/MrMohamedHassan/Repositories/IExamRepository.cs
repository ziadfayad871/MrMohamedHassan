using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public interface IExamRepository : IGenericRepository<Exam>
{
    Task<Exam?> GetExamWithResultsAsync(int id);
    Task<IEnumerable<Exam>> GetExamsByGroupAsync(int groupId);
}
