using Example.WebCore.BaseModelModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Example.WebCore.Jwt
{
    public class JwtTools
    {
        private static int expired = 12000;

        public static JwtSetting JwtSetting
        {
            get
            {
                return new JwtSetting
                {
                    Issuer = "server",
                    Audience = "client",
                    SecretKey = "7PC1qL7yyOJuwsMBpoC6tUKgV5Lp8hLo",
                };
            }
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string GetToken(Dictionary<string, string> payload)
        {
            var claims = new List<Claim>();
            if (payload?.Count > 0)
            {
                foreach (var item in payload)
                {
                    claims.Add(new Claim(item.Key, item.Value));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting.SecretKey));

            //签名证书(秘钥，加密算法)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //令牌颁发时间
            var now = DateTime.UtcNow;

            var token = new JwtSecurityToken(JwtSetting.Issuer,
                JwtSetting.Audience,
                claims,//元数据
                now,//颁发时间
                now.AddSeconds(expired), //有效期
                creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 解析Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static IEnumerable<Claim> ParseToken(string token)
        {
            //对称秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting.SecretKey));

            var validationParameters = new TokenValidationParameters() // 生成验证token的参数
            {
                RequireExpirationTime = true, // token是否包含有效期
                ValidateIssuer = true, // 验证秘钥发行人，如果要验证在这里指定发行人字符串即可
                ValidIssuer = JwtSetting.Issuer,
                ValidateAudience = false, // 验证秘钥的接受人，如果要验证在这里提供接收人字符串即可
                IssuerSigningKey = key, // 生成token时的安全秘钥
                ValidateLifetime = true, //是否验证Token有效期
                ClockSkew = new TimeSpan(0, 0, 10), //注意这是过期偏移时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
            };

            // 接受解码后的token对象
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out SecurityToken securityToken);
            return principal.Claims;
        }

    }
}
