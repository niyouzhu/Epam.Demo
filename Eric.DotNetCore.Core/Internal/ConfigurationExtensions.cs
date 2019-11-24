using Microsoft.Extensions.Configuration;
using System;

namespace Eric.DotNetCore.Core
{
    public static class ConfigurationExtensions
    {
        public static string GetString(this IConfiguration config, string name, string defaultValue = null)
        {
            Guard.ArgumentNotNull(config, "config");
            return string.IsNullOrEmpty(config[name]) ? defaultValue : config[name];
        }

        public static int? GetInt32(this IConfiguration config, string name, int? defaultValue = null)
        {
            Guard.ArgumentNotNull(config, "config");
            if (string.IsNullOrEmpty(config[name]))
                return defaultValue;
            int value;
            if (int.TryParse(config[name], out value))
                return value;
            throw new InvalidOperationException(
                $"The configuration \"{config.ResolvePath(name)}\" ({config[name]}) can not be parsed as an integer.");
        }

        public static double? GetDouble(this IConfiguration config, string name, double? defaultValue = null)
        {
            Guard.ArgumentNotNull(config, "config");
            if (string.IsNullOrEmpty(config[name]))
                return defaultValue;
            double value;
            if (double.TryParse(config[name], out value))
                return value;
            throw new InvalidOperationException(
                $"The configuration \"{config.ResolvePath(name)}\" ({config[name]}) can not be parsed as a double value.");
        }

        public static bool? GetBoolean(this IConfiguration config, string name, bool? defaultValue = null)
        {
            Guard.ArgumentNotNull(config, "config");
            if (string.IsNullOrEmpty(config[name]))
                return defaultValue;
            switch (config[name].ToUpper())
            {
                case "TRUE":
                case "1":
                case "YES":
                    return true;

                case "FALSE":
                case "0":
                case "NO":
                    return false;
            }
            throw new InvalidOperationException($"The configuration \"{config.ResolvePath(name)}\" ({config[name]}) can not be parsed as a boolean value.");
        }

        public static TimeSpan? GetTimeSpan(this IConfiguration config, string name, TimeSpan? defaultValue = null)
        {
            Guard.ArgumentNotNull(config, "config");
            if (string.IsNullOrEmpty(config[name]))
                return defaultValue;
            TimeSpan value;
            if (TimeSpan.TryParse(config[name], out value))
                return value;
            throw new InvalidOperationException(
                $"The configuration \"{config.ResolvePath(name)}\" ({config[name]}) can not be parsed as a TimeSpan value.");
        }

        public static T? GetEnum<T>(this IConfiguration config, string name, T? defaultValue = null)
            where T : struct
        {
            Guard.ArgumentNotNull(config, "config");
            if (string.IsNullOrEmpty(config[name]))
                return defaultValue;
            T value;
            if (Enum.TryParse(config[name], out value))
                return value;
            throw new InvalidOperationException(
                $"The configuration \"{config.ResolvePath(name)}\" ({config[name]}) can not be parsed as a {typeof(T).FullName} value.");
        }

        internal static string ResolvePath(this IConfiguration config, string name)
        {
            var section = config as IConfigurationSection;
            return section == null ? name : $"{section.Path}:{name}";
        }
    }
}