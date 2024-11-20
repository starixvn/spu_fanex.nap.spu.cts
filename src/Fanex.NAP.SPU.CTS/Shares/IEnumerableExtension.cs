namespace Fanex.NAP.SPU.CTS.Shares
{
    using System.Linq;
    using System.Collections.Generic;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Info Code Smell", "S4136:All 'All 'IsNullOrEmpty' method overloads should be adjacent.", Justification = "<Pending>")]
    public static class IEnumerableExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> arr)
        {
            return arr is null || !arr.Any();
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> arr)
        {
            return !IsNullOrEmpty(arr);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !IsNullOrEmpty(value);
        }
    }
}
