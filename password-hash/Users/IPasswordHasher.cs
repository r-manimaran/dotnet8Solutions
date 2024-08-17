namespace password_hash;

public interface IPasswordHasher 
{
    string HashPassword(string password);
    bool Verify(string password, string passwordHash);
}