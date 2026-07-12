using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public interface IStudentRepository : IGenericRepository<Student>
{
    Task<Student?> GetStudentWithDetailsAsync(int id);
    Task<IEnumerable<Student>> GetStudentsByGroupAsync(int groupId);
    Task<string> GenerateStudentCodeAsync();
    Task<int> GetActiveStudentCountAsync();
}
