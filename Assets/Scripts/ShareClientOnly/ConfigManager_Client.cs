using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LitJson;
using Hiker.GUI;

public partial class ConfigManager
{
    public static string GetFormattedTimeHours(int hours, int minutes, int secs)
    {
        return string.Format(Localization.Get("HourMinuteSecondsTimes"), hours, minutes, secs);
    }
    public static string GetFormattedTimeDays(int days, int hours, int minutes, int secs)
    {
        return string.Format(Localization.Get("DaysHourMinuteSecondsTimes"), days, hours, minutes, secs);
    }
    public static string GetFormattedDayRemains(int days)
    {
        return string.Format(Localization.Get("DayRemainsLabel"), days);
    }
    public static string GetFormattedHourRemains(int h)
    {
        return string.Format(Localization.Get("HourRemainsLabel"), h);
    }
    public static string GetFormattedMinuteRemains(int m)
    {
        return string.Format(Localization.Get("MinuteRemainsLabel"), m);
    }
    public static string GetFormattedSecondRemains(int s)
    {
        return string.Format(Localization.Get("SecondRemainsLabel"), s);
    }
    public static string GetHighestElementTimeRemain(System.TimeSpan ts)
    {
        var d = ts.Days;
        if (d > 1)
        {
            return GetFormattedDayRemains(d + 1);
        }

        var h = d * 24 + ts.Hours;
        if (h > 1)
        {
            return GetFormattedHourRemains(h + 1);
        }

        var m = h * 60 + ts.Minutes;
        if (m > 1)
        {
            return GetFormattedMinuteRemains(m + 1);
        }

        var s = m * 60 + ts.Seconds;
        return GetFormattedSecondRemains(s + 1);
    }

    public static string ReadConfigString(string configFileName, string path = null)
    {
        TextAsset textAsset = null;

        if (path == null)
        {
            path = string.Format("Configs/");
        }

        if (textAsset == null) textAsset = Resources.Load(path + configFileName) as TextAsset;

        if (textAsset == null)
        {
            return string.Empty;
        }
        return textAsset.text;
    }

    public static void LoadConfigs_Client()
    {
        loaded = true;
        LoadConfigs();
    }

    public static string GetIconTMP_Sprite(string name)
    {
        return string.Format(@"<sprite name=""{0}"">", name);
    }
    
    public static string RoundNumberToInt(float number)
    {
        if (number > 1000000f)
        {
            return string.Format("{0:0.#}", number / 1000000f) + "M";
        }
        else return number < 10000f ? Mathf.RoundToInt(number).ToString() : string.Format("{0:0.#}", number / 1000f) + "K";
    }

}
