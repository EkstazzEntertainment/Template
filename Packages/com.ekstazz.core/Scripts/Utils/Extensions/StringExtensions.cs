namespace Ekstazz.Utils.Extensions
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    
    public static class StringExtensions
    {
        public static string CamelCaseToUnderscoredUpper(this string s)
        {
            var enumerable = s.Select((x, i) => i > 0 && char.IsUpper(x) ? $"_{x}" : $"{x}").ToArray();
            return string.Concat(enumerable).ToUpper();
        }

        public static string CamelCaseToReadable(this string s)
        {
            return Regex.Replace(Regex.Replace(s, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static string TimeString(this TimeSpan ts)
        {
            if (ts.TotalDays >= 1)
            {
                return $"{ts.TotalDays:0}d {ts.Hours:00}h";
            }
            if (ts.TotalHours >= 1)
            {
                return $"{ts.TotalHours:0}h {ts.Minutes:00}m";
            }
            if (ts.TotalMinutes >= 1)
            {
                return $"{ts.TotalMinutes:0}m {ts.Seconds:00}s";
            }
            return $"{ts.TotalSeconds:00}s";
        }
    }
}
