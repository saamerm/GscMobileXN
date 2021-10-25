using System.Globalization;

namespace MobileClaims.Core.Extensions
{
    public static class DoubleExtensions
    {
        public static string AddDolarSignBasedOnCulture(this double value)
        {
            var valueWithDolarSign = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Contains("en")
                ? $"${value}"
                : $"{value} $";

            return valueWithDolarSign;
        }

        public static string AddDolarSignBasedOnCulture(this double? value)
        {
            if (value != null && value.HasValue)
            {
                var valueWithDolarSign = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Contains("en")
                    ? $"${value}"
                    : $"{value} $";

                return valueWithDolarSign;
            }
            return null;
        }
    }
}