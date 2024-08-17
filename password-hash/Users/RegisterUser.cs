
namespace password_hash;

public sealed class RegisterUser
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public sealed record Request(string Email, string FirstName, string LastName, string Password);

    public RegisterUser(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> Handle(Request request)
    {

        if(await _userRepository.Exists(request.Email)){
            throw new Exception("User already exists");
        }

        
        var user= new User();
        user.Id = Guid.NewGuid();
        user.Email = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PasswordHash = _passwordHasher.HashPassword(request.Password);

        await _userRepository.Insert(user);

        return user;
    }
}