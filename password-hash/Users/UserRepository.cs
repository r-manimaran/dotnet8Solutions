
using Microsoft.EntityFrameworkCore;

namespace password_hash;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Exists(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetByEmail(string email)
    {
        User user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task Insert(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}
