using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace BlueBit.HR.Docs.BL.Extensions
{
    public static class EnumerableExt
    {
        public static void ForAll<T>(this IEnumerable<T> @this, Action<T> action)
        {
            Contract.Assert(@this != null);
            Contract.Assert(action != null);
            foreach (var item in @this)
                action(item);
        }
    }
}
