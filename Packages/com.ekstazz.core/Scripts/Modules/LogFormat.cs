namespace Ekstazz.Core.Modules
{
    using System;
    using UnityEngine;

    public enum LogFormat
    {
        Unity,
        TeamCity
    }

    public static class FormatExtensions
    {
        public static Action<string> GetLogMethod(this LogFormat format, MessageSeverity severity)
        {
            return format switch
            {
                LogFormat.Unity when severity == MessageSeverity.Log => LogUnity,
                LogFormat.Unity when severity == MessageSeverity.Warning => LogUnityWarning,
                LogFormat.Unity when severity == MessageSeverity.Error => LogUnityError,
                LogFormat.TeamCity when severity == MessageSeverity.Log => LogTeamCity,
                LogFormat.TeamCity when severity == MessageSeverity.Warning => LogTeamCityWarning,
                LogFormat.TeamCity when severity == MessageSeverity.Error => LogTeamCityError,
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
            };
        }

        public static void LogUnity(string error)
        {
            Debug.Log($"{error}");
        }

        public static void LogTeamCity(string error)
        {
            Console.WriteLine($"##teamcity[message text='{EscapeCharacters(error)}']");
        }

        public static void LogUnityWarning(string error)
        {
            Debug.LogWarning($"{error}");
        }

        public static void LogTeamCityWarning(string error)
        {
            Console.WriteLine($"##teamcity[message text='{EscapeCharacters(error)}' status='WARNING']");
        }

        public static void LogUnityError(string error)
        {
            Debug.LogError($"<color=red>{error}</color>");
        }

        public static void LogTeamCityError(string error)
        {
            Console.WriteLine($"##teamcity[message text='{EscapeCharacters(error)}' status='ERROR']");
        }

        private static string EscapeCharacters(string message)
        {
            return message
                .Replace("|", "||")
                .Replace("'", "|'")
                .Replace("\n", "|n")
                .Replace("\r", "|r")
                .Replace("[", "|[")
                .Replace("]", "|]");
        }
    }
}
