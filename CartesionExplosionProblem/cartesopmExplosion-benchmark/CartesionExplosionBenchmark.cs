using cartesionExplosion_api.Database;
using Microsoft.EntityFrameworkCore;
using BenchmarkDotNet.Attributes;

namespace CartesionExplosion.Benchmark;

[MemoryDiagnoser]
public class CartesionExplosionBenchmark
{
    private const int DepartmentId = 12;
    private const int EmployeeId = 1;

    [Benchmark]
    public async Task<object> GetDepartment_Regular()
    {
        using var context = new AppDbContext(new DbContextOptions<AppDbContext>());

        var departments = await context
                        .departments
                        .Include(e=>e.Teams)
                        .ThenInclude(e=>e.Employees)
                        .AsNoTracking()
                        .Where(d=>d.Id == DepartmentId)
                        .ToListAsync();
        return departments;
    }

    [Benchmark]
    public async Task<object> GetDepartment_WithSplitQuery()
    {
        using var context = new AppDbContext(new DbContextOptions<AppDbContext>());

        var departments = await context
                        .departments
                        .Include(e=>e.Teams)
                        .ThenInclude(e=>e.Employees)
                        .AsNoTracking()
                        .Where(d=>d.Id == DepartmentId)
                        .ToListAsync();
        return departments;
    }

    [Benchmark]
    public async Task<object> GetEmployees_Regular()
    {
        using var context = new AppDbContext(new DbContextOptions<AppDbContext>());

        var employees = await context
                        .employees
                        .Include(e=>e.Tasks)
                        .Include(e=>e.SalaryPayments)
                        .AsNoTracking()
                        .Where(d=>d.Id == EmployeeId)
                        .ToListAsync();
        return employees;
    }

    [Benchmark]
    public async Task<object> GetEmployees_SplitQuery()
    {
        using var context = new AppDbContext(new DbContextOptions<AppDbContext>());

        var employees = await context
                        .employees
                        .Include(e=>e.Tasks)
                        .Include(e=>e.SalaryPayments)
                        .AsSplitQuery()
                        .AsNoTracking()
                        .Where(e=>e.Id == EmployeeId)
                        .ToListAsync();
        return employees;
    }





}