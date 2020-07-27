using System;
using System.Diagnostics.CodeAnalysis;

namespace cTeleport.AirportMeasure.Utils
{
    public static class Check
    {
        public static T NotNull<T>(T value, [NotNull] string parameterName)
        {
            if (value != null)
            {
                return value;
            }

            NotEmpty(parameterName, nameof(parameterName));
            throw new ArgumentNullException(parameterName);
        }

        public static string NotEmpty(string value, [NotNull] string parameterName)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            NotEmpty(parameterName, nameof(parameterName));
            throw new ArgumentNullException(parameterName);
        }
    }
}