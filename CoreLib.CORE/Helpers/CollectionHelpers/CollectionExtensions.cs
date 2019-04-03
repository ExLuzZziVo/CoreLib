#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace UIServiceLib.CORE.Helpers.CollectionHelpers
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> AppendRange<T>(this IEnumerable<T> enumerable, IEnumerable<T> appendCollection)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), "The Enumerable<T> must be specified!");
            if (appendCollection == null)
                return enumerable;
#if NET40
            enumerable.AppendRange(appendCollection);
#else
            foreach (var o in appendCollection) enumerable.Append(o);
#endif
            return enumerable;
        }

        public static int IndexOf<T>(this IEnumerable<T> enumerable, T value)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), "The Enumerable<T> must be specified!");
            return enumerable.IndexOf(value, null);
        }

        public static int IndexOf<T>(this IEnumerable<T> enumerable, T value, IEqualityComparer<T> comparer)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), "The Enumerable<T> must be specified!");
            comparer = comparer ?? EqualityComparer<T>.Default;
            var found = enumerable
                .Select((a, i) => new {a, i})
                .FirstOrDefault(x => comparer.Equals(x.a, value));
            return found?.i ?? -1;
        }
    }
}