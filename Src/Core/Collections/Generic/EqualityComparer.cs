using System;
using System.Collections.Generic;

namespace su3dev.Collections.Generic
{
    public class EqualityComparer
    {
        public static IEqualityComparer<T> Create<T>(
                    Func<T, T, bool> comparer,
                    Func<T, int>? hasher = null)
        {
            _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

            if (hasher is null)
            {
                return new FuncEqualityComparer<T>(comparer);
            }

            return new FuncEqualityComparer<T>(comparer, hasher);
        }
    }
}
