using System.Globalization;

namespace Eric.DotNetCore.Core
{
    public static class StringExtensions
    {
        public static string Fill(this string format, params object[] arguments)
        {
            return string.Format(CultureInfo.CurrentCulture, format, arguments);
        }
    }
}