using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomAuthorizeAttribute.Services
{
    public class SessionValidationService
    {
        public bool ContainsCustomHeader(AuthorizationFilterContext context, string headerName)
            => context.HttpContext.Request.Headers[headerName].Any();
        
    }
}
