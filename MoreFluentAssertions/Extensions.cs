using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreFluentAssertions
{
    internal static class Extensions
    {
        internal static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }
    }
}
