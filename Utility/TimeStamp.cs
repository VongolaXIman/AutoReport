using System;

namespace Utility
{
    public class TimeStamp
    {
        private static readonly DateTime baseDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private DateTimeOffset offset;
        private long timestamp;

        public TimeStamp()
        {
            offset = new DateTimeOffset(baseDateTime);
            timestamp = 0;
        }

        public TimeStamp(long milliTimestamp)
        {
            offset = DateTimeOffset.FromUnixTimeMilliseconds(milliTimestamp);
            timestamp = milliTimestamp;
        }

        public long Value
        {
            get { return timestamp; }
        }

        public static TimeStamp Now
        {
            get
            {
                return new TimeStamp(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            }
        }

        public static TimeStamp FromDateTimeOffset(DateTimeOffset datetime)
        {
            long milliSeconds = datetime.ToUnixTimeMilliseconds();
            return new TimeStamp(milliSeconds);
        }

        public static TimeStamp FromDateTime(DateTime datetime, int timezone = 0)
        {
            DateTimeOffset dto = new DateTimeOffset(datetime, new TimeSpan(timezone, 0, 0));
            long milliSeconds = dto.ToUnixTimeMilliseconds();
            return new TimeStamp(milliSeconds);
        }

        public DateTime ToUtc()
        {
            return offset.DateTime;
        }

        public string ToLocal(int? timezone = null, DateTimeKind? kind = null)
        {
            TimeSpan timeZone = timezone == null ? TimeZoneInfo.Local.BaseUtcOffset : new TimeSpan((int)timezone, 0, 0);
            DateTimeKind dateTimeKind = timezone == null ? DateTimeKind.Local : kind ?? DateTimeKind.Unspecified;
            DateTimeOffset dto = offset.DateTime.Add(timeZone);
            return DateTime.SpecifyKind(dto.DateTime, dateTimeKind).ToString("yyyy/MM/dd HH:mm:ss.fff");
        }
    }
}
