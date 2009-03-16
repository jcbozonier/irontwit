using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.UI.Utilities
{
    public static class Unit3Extensions
    {
        public static IEnumerable<K> Convert<T, K>(this IEnumerable<T> items, Func<T, K> converter)
        {
            var result = new List<K>();

            foreach(var item in items)
            {
                result.Add(converter(item));
            }

            return result;
        }
    }
}
