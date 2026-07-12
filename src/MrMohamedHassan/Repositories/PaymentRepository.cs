using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Payment>> GetPaymentsByStudentAsync(int studentId)
    {
        return await _dbSet
            .Include(p => p.Student)
            .Where(p => p.StudentId == studentId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _dbSet
            .Include(p => p.Student)
            .Where(p => p.PaymentDate >= start && p.PaymentDate <= end)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime start, DateTime end)
    {
        return await _dbSet
            .Where(p => p.PaymentDate >= start && p.PaymentDate <= end && p.IsPaid)
            .SumAsync(p => p.PaidAmount);
    }

    public async Task<decimal> GetStudentTotalPaymentsAsync(int studentId)
    {
        return await _dbSet
            .Where(p => p.StudentId == studentId && p.IsPaid)
            .SumAsync(p => p.PaidAmount);
    }

    public async Task<string> GenerateReceiptNumberAsync()
    {
        var count = await _dbSet.CountAsync();
        return $"RCP-{DateTime.Now:yyyyMMdd}-{(count + 1):D4}";
    }
}
