using System;
using System.Reflection;

namespace MobileClaims.Core.Extensions
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("T must be an Enum");
            }

            if (Enum.TryParse<T>(value.Trim(), true, out var tempRel))
            {
                return tempRel;
            }
            return default(T);
        }

        public static bool IsVisionEnhancement(this string value)
        {
            return string.Equals(value, "GLASSES", StringComparison.OrdinalIgnoreCase)
                || string.Equals(value, "CONTACTS", StringComparison.OrdinalIgnoreCase);
        }
    }
}