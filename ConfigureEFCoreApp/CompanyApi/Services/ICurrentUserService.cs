namespace CompanyApi.Services;

public interface ICurrentUserService
{
    string UserId { get; }
    string UserName { get; }
    string Email { get; }
    IEnumerable<String> Roles { get; }
    bool IsAuthenticated { get; }
}
