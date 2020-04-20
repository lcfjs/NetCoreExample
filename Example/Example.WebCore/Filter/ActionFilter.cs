using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.WebCore.Filter
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //LoggerTools.GetInstance(LoggerTools.RequestLog).Info("test");
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
