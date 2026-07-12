using MrMohamedHassan.Models;

namespace MrMohamedHassan.Services;

public interface IExamService
{
    Task<Exam?> GetByIdAsync(int id);
    Task<IEnumerable<Exam>> GetAllAsync();
    Task<Exam> CreateAsync(Exam exam);
    Task UpdateAsync(Exam exam);
    Task DeleteAsync(int id);
    Task<Exam?> GetExamWithResultsAsync(int id);
    Task SaveResultsAsync(int examId, List<ExamResult> results);
    Task<IEnumerable<ExamResult>> GetStudentRankingAsync(int examId);
}
