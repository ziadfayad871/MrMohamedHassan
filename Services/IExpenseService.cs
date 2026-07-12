using MrMohamedHassan.Models;

namespace MrMohamedHassan.Services;

public interface IExpenseService
{
    Task<Expense?> GetByIdAsync(int id);
    Task<IEnumerable<Expense>> GetAllAsync();
    Task<Expense> CreateAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(int id);
    Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime start, DateTime end);
    Task<decimal> GetTotalExpensesAsync(DateTime start, DateTime end);
    Task<IEnumerable<ExpenseCategory>> GetCategoriesAsync();
}
