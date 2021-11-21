using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace su3dev.Collections.Generic
{
    public class FuncEqualityComparer<T> : EqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;
        private readonly Func<T, int> _hash;

        public FuncEqualityComparer([DisallowNull] Func<T, T, bool> comparer)
            : this(comparer, t => t.GetHashCode())
        { }

        public FuncEqualityComparer([DisallowNull] Func<T, T, bool> comparer, [DisallowNull] Func<T, int> hash)
        {
            _ = comparer ?? throw new ArgumentNullException(nameof(comparer));
            _ = hash ?? throw new ArgumentNullException(nameof(hash));

            _comparer = comparer;
            _hash = hash;
        }

        public override bool Equals(T a, T b)
        {
            return _comparer(a, b);
        }

        public override int GetHashCode(T obj)
        {
            return _hash(obj);
        }
    }
}
