namespace WingetGUI.Core.Tests.Utils
{
    internal static class CollectionMatcher
    {
        internal static IEnumerable<T> Match<T>(IEnumerable<T> expected) where T : class
        {
            var properties = typeof(T).GetProperties();
            return It.Is<IEnumerable<T>>((actual) => Match(expected, actual));
        }
        internal static bool Match<T>(IEnumerable<T> expected, IEnumerable<T> actual) where T : class
        {
            if (expected is null && actual is null) return true;
            if (expected is null || actual is null) return false;
            int expectedCount = expected.Count(),
                actualCount = actual.Count();
            if(expectedCount != actualCount) return false;

            return Enumerable.Range(0, expectedCount).All(index => PropertyMatcher.Match<T>(expected.ElementAt(index), actual.ElementAt(index)));
        }
    }
}
