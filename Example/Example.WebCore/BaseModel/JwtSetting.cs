using System;
using System.Collections.Generic;
using System.Text;

namespace Example.WebCore.BaseModelModel
{
    /// <summary>
    /// JWT配置
    /// </summary>
    public class JwtSetting
    {
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 使用者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 密匙
        /// </summary>
        public string SecretKey { get; set; }
    }
}
