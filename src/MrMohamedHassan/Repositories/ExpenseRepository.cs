using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Repositories;

public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
{
    public ExpenseRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _dbSet
            .Include(e => e.Category)
            .Where(e => e.ExpenseDate >= start && e.ExpenseDate <= end)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId)
    {
        return await _dbSet
            .Include(e => e.Category)
            .Where(e => e.ExpenseCategoryId == categoryId)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalExpensesAsync(DateTime start, DateTime end)
    {
        return await _dbSet
            .Where(e => e.ExpenseDate >= start && e.ExpenseDate <= end)
            .SumAsync(e => e.Amount);
    }
}
