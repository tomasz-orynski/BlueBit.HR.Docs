using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace BlueBit.HR.Docs.BL.Extensions
{
    public static class DictionaryExt
    {
        public static TValue GetOptValue<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key)
            where TValue : class
        {
            Contract.Assert(@this != null);
            var value = default(TValue);
            @this.TryGetValue(key, out value);
            return value;
        }
    }
}