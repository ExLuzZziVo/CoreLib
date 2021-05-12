#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace CoreLib.CORE.Helpers.CollectionHelpers
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds a sequence of objects to the end of collection
        /// </summary>
        /// <param name="enumerable">The sequence to add objects to</param>
        /// <param name="appendCollection">The sequence of objects to be added</param>
        /// <returns>Filled with objects from <paramref name="appendCollection"/> sequence <paramref name="enumerable"/></returns>
        public static IEnumerable<T> AppendRange<T>(this IEnumerable<T> enumerable, IEnumerable<T> appendCollection)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (appendCollection == null)
            {
                return enumerable;
            }

            foreach (var o in appendCollection)
            {
                enumerable.Append(o);
            }

            return enumerable;
        }

        /// <summary>
        /// Gets the index of an object of sequence
        /// </summary>
        /// <param name="enumerable">Sequence of objects</param>
        /// <param name="value">Target object</param>
        /// <returns>Index of <paramref name="value"/></returns>
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T value)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            return enumerable.IndexOf(value, null);
        }

        /// <summary>
        /// Gets the index of an object of sequence using the equality comparer
        /// </summary>
        /// <param name="enumerable">Sequence of objects</param>
        /// <param name="value">Target object</param>
        /// <param name="comparer">Equality comparer</param>
        /// <returns>Index of <paramref name="value"/></returns>
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T value, IEqualityComparer<T> comparer)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            comparer = comparer ?? EqualityComparer<T>.Default;

            var found = enumerable
                .Select((a, i) => new {a, i})
                .FirstOrDefault(x => comparer.Equals(x.a, value));

            return found?.i ?? -1;
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the end of a sequence
        /// </summary>
        /// <param name="enumerable">The sequence to return elements from</param>
        /// <param name="length">The number of elements to return</param>
        /// <returns>A sequence that contains the specified number of elements from the end of the input sequence</returns>
        public static IEnumerable<T> ReverseTake<T>(this IEnumerable<T> enumerable, int length)
        {
            var count = enumerable.Count();

            return length >= count ? enumerable : enumerable.Skip(count - length);
        }
    }
}