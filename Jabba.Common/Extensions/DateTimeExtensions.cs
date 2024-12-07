namespace Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToUniversalString(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
