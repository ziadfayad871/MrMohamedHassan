using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;
using MrMohamedHassan.Repositories;

namespace MrMohamedHassan.Services;

public class ExamService : IExamService
{
    private readonly IExamRepository _examRepository;
    private readonly ApplicationDbContext _context;

    public ExamService(IExamRepository examRepository, ApplicationDbContext context)
    {
        _examRepository = examRepository;
        _context = context;
    }

    public async Task<Exam?> GetByIdAsync(int id) => await _examRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Exam>> GetAllAsync() => await _examRepository.GetAllAsync();

    public async Task<Exam> CreateAsync(Exam exam) => await _examRepository.AddAsync(exam);

    public async Task UpdateAsync(Exam exam) => await _examRepository.UpdateAsync(exam);

    public async Task DeleteAsync(int id) => await _examRepository.DeleteAsync(id);

    public async Task<Exam?> GetExamWithResultsAsync(int id) => await _examRepository.GetExamWithResultsAsync(id);

    public async Task SaveResultsAsync(int examId, List<ExamResult> results)
    {
        foreach (var result in results)
        {
            var existing = await _context.ExamResults
                .FirstOrDefaultAsync(r => r.ExamId == examId && r.StudentId == result.StudentId);

            if (existing != null)
            {
                existing.MarksObtained = result.MarksObtained;
                existing.Notes = result.Notes;
            }
            else
            {
                _context.ExamResults.Add(new ExamResult
                {
                    ExamId = examId,
                    StudentId = result.StudentId,
                    MarksObtained = result.MarksObtained,
                    Notes = result.Notes
                });
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ExamResult>> GetStudentRankingAsync(int examId)
    {
        return await _context.ExamResults
            .Include(r => r.Student)
            .Where(r => r.ExamId == examId)
            .OrderByDescending(r => r.MarksObtained)
            .ToListAsync();
    }
}
