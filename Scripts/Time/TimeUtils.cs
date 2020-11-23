using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class TimeUtils
{
    public static string GetTimeString(this TimeSpan timespan, string format)
    {
        var timeString = string.Format(format, timespan.Hours, timespan.Minutes, timespan.Seconds);
        return timeString;
    }
    public static string GetTimeString(this TimeSpan timespan)
    {
        return GetTimeString(timespan, "{0:D2}:{1:D2}:{2:D2}");
    }

    public static string GetSecondsString(this TimeSpan timespan, string format)
    {
        var timeString = string.Format(format, timespan.TotalSeconds);
        return timeString;
    }

    public static string GetSecondsString(this TimeSpan timespan)
    {
        return GetSecondsString(timespan, "{0}");
    }

}
