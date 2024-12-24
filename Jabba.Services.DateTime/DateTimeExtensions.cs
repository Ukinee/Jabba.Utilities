namespace Utilities.DateTimeService
{
    public static class DateTimeExtensions
    {
        public static string ToUniversalLong(this DateTimeOffset dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToUniversalShortDate(this DateTimeOffset dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy-MM-dd");
        }

        public static string ToUniversalShortTime(this DateTimeOffset dateTime)
        {
            return dateTime.ToUniversalTime().ToString("HH:mm:ss");
        }
    }
}
