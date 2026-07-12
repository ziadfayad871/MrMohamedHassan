using MrMohamedHassan.Models;

namespace MrMohamedHassan.Services;

public interface IHomeworkService
{
    Task<Homework?> GetByIdAsync(int id);
    Task<IEnumerable<Homework>> GetAllAsync();
    Task<Homework> CreateAsync(Homework homework);
    Task UpdateAsync(Homework homework);
    Task DeleteAsync(int id);
    Task<Homework?> GetHomeworkWithSubmissionsAsync(int id);
    Task<HomeworkSubmission> SubmitAsync(HomeworkSubmission submission);
    Task GradeAsync(int submissionId, int marks, string? comments);
}
