using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Infrastructure.Tools
{
    public class DateTimeUtility
    {
        /// <summary>
        /// 获取时间段内的TimeSpan
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTimeSpan(DateTime time)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (long)(time - startTime).TotalSeconds;
        }
        /// <summary>
        /// timeSpan转换为DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TimeSpanToDateTime(long timestamp)
        {
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan ts = new TimeSpan(long.Parse(timestamp + "0000"));
            DateTime ttt = converted.Add(ts);
            return ttt;
        }


    }
}
