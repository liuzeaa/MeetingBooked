namespace MeetingModel
{   
    /// <summary>
    /// 日期时间价格类型
    /// </summary>
    public enum DateTimeInterval
    {
        Second, Minute, Hour, Day, Week, Month, Quarter, Year
    }
    /// <summary>
    /// 时间
    /// </summary>
    public class DateTimeManger
    {
        /// <summary>
        /// 日期时间只差
        /// </summary>
        /// <param name="Interval">相差间隔</param>
        /// <param name="StartDate">起始日期</param>
        /// <param name="EndDate">期初日期</param>
        /// <returns>返回长整数</returns>
        public static long DateDiff(DateTimeInterval Interval, System.DateTime StartDate, System.DateTime EndDate)
        {
            long lngDateDiffValue = 0;
            System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (Interval)
            {
                case DateTimeInterval.Second:
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case DateTimeInterval.Minute:
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case DateTimeInterval.Hour:
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case DateTimeInterval.Day:
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case DateTimeInterval.Week:
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case DateTimeInterval.Month:
                    lngDateDiffValue = (long)(TS.Days / 30);
                    break;
                case DateTimeInterval.Quarter:
                    lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    break;
                case DateTimeInterval.Year:
                    lngDateDiffValue = (long)(TS.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }
    }
}