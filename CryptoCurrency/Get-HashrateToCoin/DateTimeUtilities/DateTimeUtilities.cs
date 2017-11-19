using System;

namespace ATAP.DateTimeUtilities
{
    public static class DateTimeHelpers
    {
        public static long ToUnixTime(this DateTime date, int uom)
        {
            return (date.ToUniversalTime().Ticks - 621355968000000000) / (10000*uom);
        }
    }
}
