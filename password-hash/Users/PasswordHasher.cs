using System.Security.Cryptography;

namespace password_hash;

public class PasswordHasher :IPasswordHasher 
{
    private const int SaltSize = 16;
    private const int HashSize =32;
    private const int Iterations = 10000;

    private readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA512;

  

    public string HashPassword(string password) 
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, HashSize);

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

     public bool Verify(string password, string passwordHash)
    {
        string[] parts = passwordHash.Split('-');
        byte[] hash = Convert.FromHexString(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt,Iterations,_hashAlgorithmName, HashSize);
        
        return CryptographicOperations.FixedTimeEquals(hash, inputHash);

    }
}