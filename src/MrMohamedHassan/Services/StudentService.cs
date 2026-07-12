using MrMohamedHassan.Models;
using MrMohamedHassan.Repositories;

namespace MrMohamedHassan.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Student?> GetByIdAsync(int id) => await _studentRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Student>> GetAllAsync() => await _studentRepository.GetAllAsync();

    public async Task<Student> CreateAsync(Student student)
    {
        student.StudentCode = await _studentRepository.GenerateStudentCodeAsync();
        student.CreatedAt = DateTime.UtcNow;
        return await _studentRepository.AddAsync(student);
    }

    public async Task UpdateAsync(Student student)
    {
        student.UpdatedAt = DateTime.UtcNow;
        await _studentRepository.UpdateAsync(student);
    }

    public async Task SoftDeleteAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student != null)
        {
            student.IsDeleted = true;
            student.UpdatedAt = DateTime.UtcNow;
            await _studentRepository.UpdateAsync(student);
        }
    }

    public async Task<Student?> GetStudentWithDetailsAsync(int id)
        => await _studentRepository.GetStudentWithDetailsAsync(id);

    public async Task<string> GenerateStudentCodeAsync()
        => await _studentRepository.GenerateStudentCodeAsync();

    public async Task<int> GetActiveCountAsync()
        => await _studentRepository.GetActiveStudentCountAsync();

    public async Task<IEnumerable<Student>> SearchAsync(string? name, string? code, StudentStatus? status)
    {
        var query = _studentRepository.Query();
        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(s => s.FullName.Contains(name));
        if (!string.IsNullOrWhiteSpace(code))
            query = query.Where(s => s.StudentCode.Contains(code));
        if (status.HasValue)
            query = query.Where(s => s.Status == status.Value);
        return query.Where(s => !s.IsDeleted).OrderByDescending(s => s.CreatedAt).ToList();
    }
}
