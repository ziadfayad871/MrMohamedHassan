using MrMohamedHassan.Models;

namespace MrMohamedHassan.Services;

public interface IPaymentService
{
    Task<Payment?> GetByIdAsync(int id);
    Task<IEnumerable<Payment>> GetAllAsync();
    Task<Payment> CreateAsync(Payment payment);
    Task<IEnumerable<Payment>> GetPaymentsByStudentAsync(int studentId);
    Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime start, DateTime end);
    Task<decimal> GetTotalRevenueAsync(DateTime start, DateTime end);
    Task<decimal> GetStudentTotalPaymentsAsync(int studentId);
    Task<string> GenerateReceiptNumberAsync();
}
