using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        string[] roles = { "Admin", "Teacher", "Assistant", "Accountant" };
        string[] roleDescriptions = { "مدير النظام", "معلم", "مساعد", "محاسب" };

        for (int i = 0; i < roles.Length; i++)
        {
            if (!await roleManager.RoleExistsAsync(roles[i]))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = roles[i],
                    NormalizedName = roles[i].ToUpper(),
                    Description = roleDescriptions[i],
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        if (await userManager.FindByEmailAsync("admin@mrmohamedhassan.com") == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@mrmohamedhassan.com",
                Email = "admin@mrmohamedhassan.com",
                FullName = "مدير النظام",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                JobTitle = "مدير"
            };

            var result = await userManager.CreateAsync(admin, "Admin@123456");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        if (!await context.ExpenseCategories.AnyAsync())
        {
            context.ExpenseCategories.AddRange(
                new ExpenseCategory { Name = "إيجار", Description = "إيجار المكان", IsActive = true },
                new ExpenseCategory { Name = "رواتب", Description = "رواتب المعلمين والموظفين", IsActive = true },
                new ExpenseCategory { Name = "مرافق", Description = "كهرباء ومياه وغاز", IsActive = true },
                new ExpenseCategory { Name = "مواد تعليمية", Description = "كتب وأدوات تعليمية", IsActive = true },
                new ExpenseCategory { Name = "صيانة", Description = "صيانة الأجهزة والمعدات", IsActive = true },
                new ExpenseCategory { Name = "تسويق", Description = "إعلانات وتسويق", IsActive = true },
                new ExpenseCategory { Name = "أخرى", Description = "مصروفات أخرى", IsActive = true }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.Teachers.AnyAsync(t => !t.IsDeleted))
        {
            var adminUser = await userManager.FindByEmailAsync("admin@mrmohamedhassan.com");
            context.Teachers.Add(new Teacher
            {
                FullName = "محمد حسن",
                Phone = "01000000000",
                Email = "admin@mrmohamedhassan.com",
                Specialization = "دراسات اجتماعيه",
                Salary = 0,
                IsActive = true,
                UserId = adminUser?.Id
            });
            await context.SaveChangesAsync();
        }

        if (!await context.Settings.AnyAsync())
        {
            context.Settings.AddRange(
                new Setting { Key = "CenterName", Value = "مركز محمد حسن" },
                new Setting { Key = "CenterPhone", Value = "01000000000" },
                new Setting { Key = "CenterEmail", Value = "info@mrmohamedhassan.com" },
                new Setting { Key = "CenterAddress", Value = "القاهرة، مصر" },
                new Setting { Key = "AcademicYear", Value = "2024-2025" },
                new Setting { Key = "CenterLogo", Value = "/images/logo.png" },
                new Setting { Key = "Currency", Value = "ج.م" }
            );
            await context.SaveChangesAsync();
        }
    }
}
