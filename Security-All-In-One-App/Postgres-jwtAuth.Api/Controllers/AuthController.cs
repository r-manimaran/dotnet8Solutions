using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Postgres_jwtAuth.Api.Constants;
using Postgres_jwtAuth.Api.DTOs;
using Postgres_jwtAuth.Api.Models;
using Postgres_jwtAuth.Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Postgres_jwtAuth.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;
    public AuthController(ILogger<AuthController> logger,
                    UserManager<ApplicationUser> userManager,
                    RoleManager<IdentityRole> roleManager,
                    ITokenService tokenService,
                    AppDbContext context)

    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _logger = logger;
        _context = context;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupModel model)
    {
        try
        {
            var existingUser = await _userManager.FindByNameAsync(model.EmailAddress);
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }

            // create User Role if it doesnot exists
            if ((await _roleManager.RoleExistsAsync(Roles.User)) == false)
            {
                var roleResult = await _roleManager
                    .CreateAsync(new IdentityRole(Roles.User));

                if (roleResult.Succeeded == false)
                {
                    var roleErrors = roleResult.Errors.Select(e => e.Description);
                    _logger.LogError($"Failed to create user role. Errors:{string.Join(",", roleErrors)}");
                    return BadRequest($"Failed to create user role. Errors:{string.Join(",", roleErrors)}");
                }
            }

            ApplicationUser user = new()
            {
                Email = model.EmailAddress,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.EmailAddress,
                Name = model.Name,
                EmailConfirmed = true
            };

            // Attempt to create a user
            var createUserResult = await _userManager.CreateAsync(user, model.Password);

            // Validate user Cration
            if (createUserResult.Succeeded == false)
            {
                var errors = createUserResult.Errors.Select(e => e.Description);
                _logger.LogError(
                    $"Failed to create user. Errors:{string.Join(",", errors)}");
                return BadRequest($"Failed to create user. Errors:{string.Join(",", errors)}");

            }

            // adding role to user
            var addUserToRoleResult = await _userManager.AddToRoleAsync(
                user: user,role: Roles.User);

            if(addUserToRoleResult.Succeeded == false)
            {
                var errors = addUserToRoleResult.Errors.Select(e=>e.Description);
                _logger.LogError($"Failed to add role to the user. Errors:{string.Join(",", errors)}");
            }
            return CreatedAtAction(nameof(Signup), null);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message );
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return BadRequest("User with this username is not registered with us");
            }

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (isValidPassword == false)
            {
                return Unauthorized();
            }

            List<Claim> authClaims = [
                new (ClaimTypes.Name, user.UserName),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                ];
            var userRoles = await _userManager.GetRolesAsync(user);

            // adding roles to the claims.
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            // generating access token
            var token = _tokenService.GenerateAccessToken(authClaims);

            string refreshToken = _tokenService.GenerateRefreshToken();

            // save refreshToken with exp date in the DB
            var tokenInfo = _context.TokenInfos.FirstOrDefault(a=>a.UserName == user.UserName);

            // If tokenInfo is null for the user create one
            if(tokenInfo == null)
            {
                var ti = new TokenInfo
                {
                    UserName = user.UserName,
                    RefreshToken = refreshToken,
                    ExpiredAt = DateTime.UtcNow.AddDays(7)
                };
                _context.TokenInfos.Add(ti);
            }
            else //update refresh token and expiration
            {
                tokenInfo.RefreshToken = refreshToken;
                tokenInfo.ExpiredAt = DateTime.UtcNow.AddDays(7);
            }
            await _context.SaveChangesAsync();
            return Ok(new TokenModel
            {
                AccessToken = token,
                RefreshToken = refreshToken,
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Unauthorized();
        }
    }

    [HttpPost("token/Refresh")]
    public async Task<IActionResult> Refresh(TokenModel tokenModel)
    {
        try
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            var username = principal.Identity.Name;

            var tokenInfo = _context.TokenInfos.SingleOrDefault(u => u.UserName == username);
            if (tokenInfo == null ||
                tokenInfo.RefreshToken != tokenModel.RefreshToken ||
                tokenInfo.ExpiredAt <= DateTime.UtcNow)
            {
                return BadRequest("Invalid Refresh token. Please login again");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            tokenInfo.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            return Ok(new TokenModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("token/revoke")]
    public async Task<IActionResult> Revoke()
    {
        try
        {
            var username = User.Identity.Name;
            var user = _context.TokenInfos.SingleOrDefault(u=>u.UserName == username);
            if (user == null)
            {
                return BadRequest();
            }

            user.RefreshToken = string.Empty;
            await _context.SaveChangesAsync();
            return Ok(true);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

}