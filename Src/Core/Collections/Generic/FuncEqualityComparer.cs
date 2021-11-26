using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace su3dev.Collections.Generic
{
    public class FuncEqualityComparer<T> : EqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;
        private readonly Func<T, int> _hasher;

        private static readonly Func<T, int> DefaultHasher = obj =>
        {
            _ = obj ?? throw new ArgumentNullException(nameof(obj));
            // ReSharper disable HeapView.PossibleBoxingAllocation
            return obj.GetHashCode();
            // ReSharper restore HeapView.PossibleBoxingAllocation
        };

        public FuncEqualityComparer(Func<T, T, bool> comparer)
            : this(comparer, DefaultHasher)
        { }

        public FuncEqualityComparer(Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            _ = comparer ?? throw new ArgumentNullException(nameof(comparer));
            _ = hasher ?? throw new ArgumentNullException(nameof(hasher));

            _comparer = comparer;
            _hasher = hasher;
        }

        public override bool Equals(T a, T b)
        {
            return _comparer(a, b);
        }

        public override int GetHashCode(T obj)
        {
            return _hasher(obj);
        }
    }
}
