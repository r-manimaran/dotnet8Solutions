using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace PolicyBasedAuthorization;

internal sealed class ClaimsTransformer(UserService userService) : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var userClaims = userService.GetUserClaims(principal.Identity!.Name!);
        
        var identity = new ClaimsIdentity(userClaims.Select(x => new Claim(AuthConstants.UserGroupClaim, x)));
        
        identity.AddClaim(new Claim("role", "guest"));

        principal.AddIdentity(identity);

        return Task.FromResult(principal);
    }
}
