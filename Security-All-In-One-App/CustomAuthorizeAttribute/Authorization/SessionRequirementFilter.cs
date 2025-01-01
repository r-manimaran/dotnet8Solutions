using CustomAuthorizeAttribute.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomAuthorizeAttribute.Authorization
{
    public class SessionRequirementFilter : IAuthorizationFilter
    {
        private readonly SessionValidationService _sessionValidationService;

        public SessionRequirementFilter(SessionValidationService sessionValidationService)
        {
            _sessionValidationService = sessionValidationService;
        }
       
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!_sessionValidationService.ContainsCustomHeader(context, "X-Session-Id"))
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized request");
                return;
            }
        }
    }
}
