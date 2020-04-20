using Example.WebCore.BaseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.WebCore.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is SecurityTokenExpiredException)
            {
                context.Result = new ObjectResult(new ResponseModel()
                {
                    Code = 99,
                    Message = "Token已过期(ExceptionFilter)"
                });
                context.ExceptionHandled = true;
                return;
            }
        }
    }
}
