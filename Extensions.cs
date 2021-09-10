using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShogunOptimizer
{
    public static class Extensions
    {
        public static T PickOne<T>(this IEnumerable<T> enumerable, Random random, T defaultValue = default)
        {
            var array = enumerable.ToArray();
            if (array.Length == 0)
                return defaultValue;

            return array[random.Next(array.Length)];
        }

        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this Dictionary<TKey, TValue> source) 
            => new ReadOnlyDictionary<TKey, TValue>(source);
    }
}
