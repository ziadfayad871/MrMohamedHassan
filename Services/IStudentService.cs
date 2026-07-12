using MrMohamedHassan.Models;

namespace MrMohamedHassan.Services;

public interface IStudentService
{
    Task<Student?> GetByIdAsync(int id);
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student> CreateAsync(Student student);
    Task UpdateAsync(Student student);
    Task SoftDeleteAsync(int id);
    Task<Student?> GetStudentWithDetailsAsync(int id);
    Task<string> GenerateStudentCodeAsync();
    Task<int> GetActiveCountAsync();
    Task<IEnumerable<Student>> SearchAsync(string? name, string? code, StudentStatus? status);
}
