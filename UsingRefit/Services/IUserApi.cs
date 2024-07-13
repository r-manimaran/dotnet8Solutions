using Refit;
using UsingRefit.Models;

namespace UsingRefit.Services;

public interface IUserApi
{
    [Get("/users")]
    Task<IEnumerable<User>> GetAll();
}
