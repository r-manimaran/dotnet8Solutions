namespace password_hash;
public interface IUserRepository{
    Task<bool> Exists(string email);
    Task<User?> GetByEmail(string email);
    Task Insert(User user);
}