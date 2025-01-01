using Microsoft.AspNetCore.Authorization;

namespace CustomAuthorizeAttribute.Authorization.Requirements
{
    public class SessionRequirement : IAuthorizationRequirement
    {
        public string? SessionName { get; }

        public SessionRequirement(string? sessionName)
        {
            SessionName = sessionName;
        }
    }
}
