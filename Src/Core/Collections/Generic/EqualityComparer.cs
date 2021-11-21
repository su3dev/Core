﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace su3dev.Collections.Generic
{
    public class EqualityComparer
    {
        public static IEqualityComparer<T> Create<T>(
                    [DisallowNull] Func<T, T, bool> comparer,
                    Func<T, int> hash = null)
        {
            _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

            if (hash is null)
            {
                return new FuncEqualityComparer<T>(comparer);
            }

            return new FuncEqualityComparer<T>(comparer, hash);
        }
    }
}