using System;
using System.Collections.Generic;
using System.Text;

namespace Example.WebCore.Extension
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// UTC时间戳（秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToTimestampSecond(this DateTime dateTime)
        {
            var now = dateTime.ToUniversalTime();
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);
            return (long)secondsSinceEpoch;
        }

        /// <summary>
        /// UTC时间戳（毫秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToTimestampMillisecond(this DateTime dateTime)
        {
            var now = dateTime.ToUniversalTime();
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalMilliseconds);
            return (long)secondsSinceEpoch;
        }
    }
}
