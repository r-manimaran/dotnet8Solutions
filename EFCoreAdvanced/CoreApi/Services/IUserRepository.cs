using CoreApi.Models.Entities;

namespace CoreApi.Services;

public interface IUserRepository
{
    Task<User> GetUserWithRawSql(int id);
    Task ExecuteStoredProcedure(int userId, decimal amount);
    Task<List<User>> GetUserWithComplexQuery();
    Task<List<User>> SearchUsers(string? searchTerm, string? exactMatch);
}
