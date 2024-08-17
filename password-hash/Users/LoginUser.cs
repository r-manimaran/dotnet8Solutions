namespace password_hash
{
    public class LoginUser
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public record Request(string Email,string Password);
        public LoginUser(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> Handle(Request request)
        {
            User? user = await _userRepository.GetByEmail(request.Email);
           if(user is null)
            {
                throw new Exception("The user was not found.");
            }
            bool verified = _passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!verified)
            {
                throw new Exception("Unauthorized or invalid Password");
            }
            return user;
        }
    }
}
