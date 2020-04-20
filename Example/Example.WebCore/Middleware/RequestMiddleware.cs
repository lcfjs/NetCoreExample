using Example.Core.Log;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Example.WebCore.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;
            if (request.Path.Value.ToLower().Contains("api"))
            {
                //var ip = GetClientIP(httpContext);

                //请求数据
                var requestData = request.QueryString.ToString();
                if (request.Method.ToLower().Equals("post"))
                {
                    request.EnableBuffering();
                    var reader = new StreamReader(request.Body);
                    requestData = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }

                //var t = request.Cookies["token"];

                //响应数据
                Stream originalBody = httpContext.Response.Body;
                using (var ms = new MemoryStream())
                {
                    httpContext.Response.Body = ms;

                    await _next(httpContext);

                    ms.Position = 0;
                    var responseData = new StreamReader(ms).ReadToEnd();
                    ms.Position = 0;

                    await ms.CopyToAsync(originalBody);
                    LoggerTools.GetInstance(LoggerTools.RequestLog).RequestInfo(null, httpContext.Request.Path.Value, requestData, responseData);
                }
            }
            else
            {
                await _next(httpContext);
            }
        }

        public string GetClientIP(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].ToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
}
