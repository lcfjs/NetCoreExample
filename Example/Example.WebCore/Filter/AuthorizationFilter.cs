using Example.WebCore.BaseModel;
using Example.WebCore.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.WebCore.Filter
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var controller = context.RouteData.Values["controller"].ToString();
            var action = context.RouteData.Values["action"].ToString();

            //netcore<3.0 : context.Filters.Any(x => x is IAllowAnonymousFilter)
            if (context.ActionDescriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute))
            {
                return;
            }
            var token = context.HttpContext.Request.Headers["authorization"].ToString();
            token = token.Replace("Bearer ", "");
            try
            {
                var result = JwtTools.ParseToken(token);
            }
            catch (SecurityTokenExpiredException)
            {
                context.Result = new ObjectResult(new ResponseModel()
                {
                    Code = 99,
                    Message = "Token已过期(AuthorizationFilter)"
                });
            }
            catch (Exception)
            {
                context.Result = new ObjectResult(new ResponseModel()
                {
                    Code = 99,
                    Message = "Token无效(AuthorizationFilter)"
                });
            }
        }
    }
}
