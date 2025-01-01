using Microsoft.AspNetCore.Authorization;

namespace CustomAuthorizeAttribute.Authorization.Requirements.Handlers
{
    public class SessionHandler : AuthorizationHandler<SessionRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public SessionHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
                                                       SessionRequirement requirement)
        {
            var httpRequest = _contextAccessor.HttpContext!.Request;
            if (!httpRequest.Headers[requirement.SessionName].Any())
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
