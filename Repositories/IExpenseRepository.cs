using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public interface IExpenseRepository : IGenericRepository<Expense>
{
    Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime start, DateTime end);
    Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId);
    Task<decimal> GetTotalExpensesAsync(DateTime start, DateTime end);
}
