using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsingRefit.Models;
using UsingRefit.Services;

namespace UsingRefit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserApi _userApi;

        public UserController(IUserApi userApi)
        {
            _userApi = userApi;
        }

        [HttpGet]
        public Task<IEnumerable<User>> Get()
        {
            var response = _userApi.GetAll();
            return response;
        }

        [HttpGet("{id}")]
        public Task<User> GetUserById(int id)
        {
            var response = _userApi.GetUserById(id);
            return response;
        }
    }
}
