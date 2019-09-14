using System;

namespace Labs.Helpers
{
    static class TimeHelper
    {
        public static string NormalizeTime(TimeSpan timeSpan, string seconds)
        {
            return timeSpan.ToString().Remove(6) + (seconds.Length < 2 ? "00" : seconds);
        }
        private static void SplitUpTimeLine(string time, out TimeSpan timeSpan, out string seconds)
        {
            var timeStrings = time.Split(':');
            timeSpan = new TimeSpan(int.Parse(timeStrings[0]), int.Parse(timeStrings[1]), 00);
            seconds = timeStrings[2];
        }
        public static void GetTime(string time, out TimeSpan timeSpan, out string seconds)
        {
            if (string.IsNullOrEmpty(time))
            {
                timeSpan = TimeSpan.Zero;
                seconds = "00";
            }
            else
            {
                SplitUpTimeLine(time, out var _timeSpan, out var _seconds);
                timeSpan = _timeSpan;
                seconds = _seconds;
            }
        }
    }
}
