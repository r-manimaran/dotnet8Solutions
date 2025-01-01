using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoleBasedAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [Authorize(Roles ="Admin")]
        [HttpGet("users")]
        public ActionResult GetUsers()
        {
            var users = new[]
            {
                new { id = 1, Name="James" },
                new { id = 2, Name="Alex" },
                new { id = 3, Name="Philip" }
            };
            return Ok(users);
        }
    }
}
