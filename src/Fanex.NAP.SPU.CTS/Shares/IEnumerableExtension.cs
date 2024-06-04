namespace Fanex.NAP.SPU.CTS.Shares
{
    using Sentry;
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Fanex.Logging.Extensions;

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
