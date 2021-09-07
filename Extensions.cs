using System;
using System.Collections.Generic;
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
    }
}
