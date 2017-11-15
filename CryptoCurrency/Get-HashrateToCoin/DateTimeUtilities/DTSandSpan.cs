using System;
namespace ATAP.DateTimeUtilities
{
    public class DTSandSpan
    {
        TimeSpan dateTimeSpan;
        DateTime dts;
        public DTSandSpan(DateTime dts, TimeSpan dateTimeSpan)
        {
            this.dts = dts;
            this.dateTimeSpan = dateTimeSpan;
        }
        public TimeSpan DateTimeSpan { get => dateTimeSpan; set => dateTimeSpan = value; }
        public DateTime Dts { get => dts; set => dts = value; }
    }
}
