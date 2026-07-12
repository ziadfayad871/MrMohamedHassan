using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<IEnumerable<Payment>> GetPaymentsByStudentAsync(int studentId);
    Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime start, DateTime end);
    Task<decimal> GetTotalRevenueAsync(DateTime start, DateTime end);
    Task<decimal> GetStudentTotalPaymentsAsync(int studentId);
    Task<string> GenerateReceiptNumberAsync();
}
