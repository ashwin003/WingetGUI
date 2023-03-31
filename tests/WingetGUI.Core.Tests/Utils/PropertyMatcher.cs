using System.Diagnostics;
using System.Reflection;

namespace WingetGUI.Core.Tests.Utils
{
    internal static class PropertyMatcher
    {
        internal static T Match<T>(T expected) where T : class
        {
            var properties = typeof(T).GetProperties();
            return It.Is<T>((actual) => Match(expected, actual, properties));
        }

        internal static bool Match<T>(T expected, T actual) where T : class
        {
            var properties = typeof(T).GetProperties();
            return Match(expected, actual, properties);
        }

        internal static bool Match<T>(T expected, T actual, IEnumerable<string> propertiesToTest) where T : class
        {
            var properties = typeof(T).GetProperties().Where(p => propertiesToTest.Contains(p.Name));
            return Match(expected, actual, properties);
        }

        internal static bool Match<T>(T expected, T actual, IEnumerable<PropertyInfo> properties) where T : class
        {
            return properties.All(property => Match(expected, actual, property));
        }

        private static bool Match<T>(T expected, T actual, PropertyInfo property) where T : class
        {
            if (expected is null && actual is null) return true;

            if (expected is null || actual is null) return false;

            var expectedValue = property.GetValue(expected);
            var actualValue = property.GetValue(actual);

            if (expectedValue is null && actualValue is null) return true;

            if (expectedValue is null || actualValue is null) return false;

            var isMatch = expectedValue.Equals(actualValue);
            if(!isMatch)
            {
                Debug.WriteLine($"Mismatch found for ${property.Name}. Expected: ${expectedValue}. Actual: ${actualValue}");
            }
            return isMatch;

        }
    }
}
