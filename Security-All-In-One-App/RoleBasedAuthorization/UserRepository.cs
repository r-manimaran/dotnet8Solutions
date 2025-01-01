namespace RoleBasedAuthorization;

public interface IUserRepository
{
    Task<List<string>> GetUserRolesAsync(string? userName, CancellationToken cancellationToken);
}
public class UserRepository : IUserRepository
{
    public Task<List<string>> GetUserRolesAsync(string? userName, CancellationToken cancellationToken)
    {
        // Logic to get from Database. But for example added here with simple return
        return Task.FromResult(new List<string> {"Admin"});
    }
}
