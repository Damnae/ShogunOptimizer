using System;
using System.Collections.Generic;
using System.Linq;

namespace ShogunOptimizer
{
    public static class Extensions
    {
        public static T PickOne<T>(this IEnumerable<T> enumerable, Random random)
        {
            var array = enumerable.ToArray();
            return array[random.Next(array.Length)];
        }
    }
}
