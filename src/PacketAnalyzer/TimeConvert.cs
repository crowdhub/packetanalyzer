using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    public class TimeConvert
    {
        public static ulong DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (ulong)(TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }
        public static DateTime UnixTimestampToDateTime(ulong unixTime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
            return dtDateTime;
        }
    }
}
