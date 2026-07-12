using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;
using MrMohamedHassan.Repositories;

namespace MrMohamedHassan.Services;

public class HomeworkService : IHomeworkService
{
    private readonly IHomeworkRepository _homeworkRepository;
    private readonly ApplicationDbContext _context;

    public HomeworkService(IHomeworkRepository homeworkRepository, ApplicationDbContext context)
    {
        _homeworkRepository = homeworkRepository;
        _context = context;
    }

    public async Task<Homework?> GetByIdAsync(int id) => await _homeworkRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Homework>> GetAllAsync() => await _homeworkRepository.GetAllAsync();

    public async Task<Homework> CreateAsync(Homework homework) => await _homeworkRepository.AddAsync(homework);

    public async Task UpdateAsync(Homework homework) => await _homeworkRepository.UpdateAsync(homework);

    public async Task DeleteAsync(int id) => await _homeworkRepository.DeleteAsync(id);

    public async Task<Homework?> GetHomeworkWithSubmissionsAsync(int id)
        => await _homeworkRepository.GetHomeworkWithSubmissionsAsync(id);

    public async Task<HomeworkSubmission> SubmitAsync(HomeworkSubmission submission)
    {
        submission.SubmittedAt = DateTime.UtcNow;
        _context.HomeworkSubmissions.Add(submission);
        await _context.SaveChangesAsync();
        return submission;
    }

    public async Task GradeAsync(int submissionId, int marks, string? comments)
    {
        var submission = await _context.HomeworkSubmissions.FindAsync(submissionId);
        if (submission != null)
        {
            submission.Marks = marks;
            submission.TeacherComments = comments;
            submission.IsGraded = true;
            await _context.SaveChangesAsync();
        }
    }
}
