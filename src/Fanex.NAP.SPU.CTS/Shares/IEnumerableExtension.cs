namespace Fanex.NAP.SPU.CTS.Shares
{
    using System.Collections.Generic;
    using System.Linq;

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
    }
}
