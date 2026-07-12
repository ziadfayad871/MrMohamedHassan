using MrMohamedHassan.Models;
using MrMohamedHassan.Repositories;

namespace MrMohamedHassan.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Payment?> GetByIdAsync(int id) => await _paymentRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Payment>> GetAllAsync() => await _paymentRepository.GetAllAsync();

    public async Task<Payment> CreateAsync(Payment payment)
    {
        payment.ReceiptNumber = await _paymentRepository.GenerateReceiptNumberAsync();
        payment.PaidAmount = payment.Amount - payment.Discount;
        return await _paymentRepository.AddAsync(payment);
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByStudentAsync(int studentId)
        => await _paymentRepository.GetPaymentsByStudentAsync(studentId);

    public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime start, DateTime end)
        => await _paymentRepository.GetPaymentsByDateRangeAsync(start, end);

    public async Task<decimal> GetTotalRevenueAsync(DateTime start, DateTime end)
        => await _paymentRepository.GetTotalRevenueAsync(start, end);

    public async Task<decimal> GetStudentTotalPaymentsAsync(int studentId)
        => await _paymentRepository.GetStudentTotalPaymentsAsync(studentId);

    public async Task<string> GenerateReceiptNumberAsync()
        => await _paymentRepository.GenerateReceiptNumberAsync();
}
