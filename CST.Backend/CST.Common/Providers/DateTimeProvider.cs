namespace CST.Common.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrent()
        {
            return DateTime.UtcNow;
        }
    }
}