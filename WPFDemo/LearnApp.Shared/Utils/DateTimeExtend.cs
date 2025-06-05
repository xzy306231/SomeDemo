using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Shared.Utils
{
    public static class DateTimeExtend
    {
        public static DateTime GetDateTimeMilliseconds(this long timestamp)//时间戳转日期
        {
            long begtime = timestamp * 10000;
            DateTime dt_1970 = new DateTime(1970, 1, 1, 8, 0, 0);
            long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度
            long time_tricks = tricks_1970 + begtime;//日志日期刻度
            DateTime dt = new DateTime(time_tricks);//转化为DateTime
            return dt;
        }

    }
}
