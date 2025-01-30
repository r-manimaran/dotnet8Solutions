using CoreApi.Data;
using CoreApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreApi.Services;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteStoredProcedure(int userId, decimal amount)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC spCreateOrder @UserId={userId}, @TotalAmount={amount}");
    }

    public async Task<List<User>> GetUserWithComplexQuery()
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Address)
            .Where(u => u.Orders.Any(o => o.Total > 1000))
            .OrderBy(u => u.FirstName)
            .ToListAsync();
    }

    public async Task<User> GetUserWithRawSql(int id)
    {
        return await _context.Users
            .FromSqlInterpolated($"SELECT * FROM Users WHERE Id= {id} AND IsDeleted=0")
            .Include(u => u.Orders)
            .FirstOrDefaultAsync();
    }

    public async Task<List<User>> SearchUsers(string? searchTerm, string? exactMatch)
    {
        var query = _context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
           query = query.Where(u=>u.EmailAddress.Value.Contains(searchTerm));
        }
        else
        {
            query = query.Where(u => u.EmailAddress.Value == exactMatch);
        }
        return await query.ToListAsync();
    }
}
