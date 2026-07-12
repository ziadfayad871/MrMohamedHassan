using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Data;
using MrMohamedHassan.Models;
using MrMohamedHassan.Repositories;

namespace MrMohamedHassan.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ApplicationDbContext _context;

    public ExpenseService(IExpenseRepository expenseRepository, ApplicationDbContext context)
    {
        _expenseRepository = expenseRepository;
        _context = context;
    }

    public async Task<Expense?> GetByIdAsync(int id) => await _expenseRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Expense>> GetAllAsync() => await _expenseRepository.GetAllAsync();

    public async Task<Expense> CreateAsync(Expense expense) => await _expenseRepository.AddAsync(expense);

    public async Task UpdateAsync(Expense expense) => await _expenseRepository.UpdateAsync(expense);

    public async Task DeleteAsync(int id) => await _expenseRepository.DeleteAsync(id);

    public async Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime start, DateTime end)
        => await _expenseRepository.GetExpensesByDateRangeAsync(start, end);

    public async Task<decimal> GetTotalExpensesAsync(DateTime start, DateTime end)
        => await _expenseRepository.GetTotalExpensesAsync(start, end);

    public async Task<IEnumerable<ExpenseCategory>> GetCategoriesAsync()
        => await _context.ExpenseCategories.Where(c => c.IsActive).ToListAsync();
}
