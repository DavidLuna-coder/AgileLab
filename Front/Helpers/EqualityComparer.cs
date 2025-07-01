using System;
using System.Collections.Generic;

namespace Front.Helpers
{
    public static class EqualityComparer
    {
        public static IEqualityComparer<T> Create<T>(
            Func<T, T, bool> equals,
            Func<T, int> getHashCode)
        {
            return new CustomEqualityComparer<T>(equals, getHashCode);
        }

        private class CustomEqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _equals;
            private readonly Func<T, int> _getHashCode;

            public CustomEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
            {
                _equals = equals ?? throw new ArgumentNullException(nameof(equals));
                _getHashCode = getHashCode ?? throw new ArgumentNullException(nameof(getHashCode));
            }

            public bool Equals(T x, T y) => _equals(x, y);

            public int GetHashCode(T obj) => _getHashCode(obj);
        }
    }
}
