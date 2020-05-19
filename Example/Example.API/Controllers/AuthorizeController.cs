using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Example.Model.Dto.User;
using Example.WebCore.BaseModel;
using Example.Core.Extension;
using Example.WebCore.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Example.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizeController : BaseController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("LoginIn")]
        public async Task<ResponseModel<UserLoginInOutput>> LoginIn([FromBody]UserLoginInInput args)
        {
            var uid = Guid.NewGuid().ToString();
            var payload = new Dictionary<string, string>() {
                { "ts",DateTime.Now.ToTimestampSecond().ToString() },
                { "uid", uid}
            };

            if (args.Account == "admin" && args.PassWord == "admin")
            {
                payload.Add("name", args.Account);
            }

            Core.Log.LoggerTools.GetInstance(Core.Log.LoggerTools.RequestLog).Info("开始："+DateTime.Now);
            Task.Run(async () => {
                await Task.Delay(5000);
                Core.Log.LoggerTools.GetInstance(Core.Log.LoggerTools.RequestLog).Info("结束：" + DateTime.Now);
            });

            var token = JwtTools.GetToken(payload);

            return Success(new UserLoginInOutput { Token = token, UserId = uid });
        }

        /// <summary>
        /// 验证Token有效性
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckToken")]
        public async Task<ResponseModel<Dictionary<string, string>>> CheckToken([Required(ErrorMessage = "Token不能为空")]string token)
        {

            var claims = JwtTools.ParseToken(token);
            var payload = new Dictionary<string, string>();
            foreach (var item in claims)
            {
                payload[item.Type] = item.Value;
            }

            return Success(payload);
        }







        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };



        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
