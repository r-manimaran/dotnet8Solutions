using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PermissionAuthorization.Authentication;
using PermissionAuthorization.Services;

namespace PermissionAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HasPermission(Permission.AccessMembers)]
    public class MembersController : ControllerBase
    {
        private readonly MembersService _membersService;

        public MembersController(MembersService membersService)
        {
            _membersService = membersService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginMember([FromBody] LoginRequest request, 
                                    CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HasPermission(Permission.ReadMember)]
        [HttpGet("id:int")]
        public async Task<IActionResult> GetMemberById(int id, CancellationToken cancellationToken)
        {
            var response = _membersService.GetMemberByIdQuery(id);
            return Ok(response);
        }
    }
}
