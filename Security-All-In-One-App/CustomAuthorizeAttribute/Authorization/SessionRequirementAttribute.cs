using CustomAuthorizeAttribute.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomAuthorizeAttribute.Authorization
{
    public class SessionRequirementAttribute : TypeFilterAttribute
    {
        public SessionRequirementAttribute() :base(typeof(SessionRequirementAttribute)) { }
        
    }
}
