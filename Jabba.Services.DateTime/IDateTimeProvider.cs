namespace Utilities.DateTimeService
{
    public interface IDateTimeProvider
    {
        public DateTimeOffset Now { get; }
        
        public string ToReadableShortNow();
        public string ToReadableLongNow();
    }
}
