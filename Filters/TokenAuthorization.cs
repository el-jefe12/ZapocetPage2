using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TexasGuyContractIdentity.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TokenAuthorization : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Security-Token"];
            if (!token.Equals("dG9rZW4=") || string.IsNullOrEmpty(token) )
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
