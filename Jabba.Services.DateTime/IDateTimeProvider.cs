namespace Utilities.DateTimeService
{
    public interface IDateTimeProvider
    {
        public DateTimeOffset Now { get; }
    }
}
