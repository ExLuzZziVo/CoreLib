﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace CoreLib.CORE.Helpers.CollectionHelpers
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds a value or values to the end of the collection
        /// </summary>
        /// <param name="enumerable">The sequence to add objects to</param>
        /// <param name="elements">The sequence of objects to be added</param>
        /// <returns>Filled with value or values from <paramref name="elements"/> sequence <paramref name="enumerable"/></returns>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> enumerable, params T[] elements)
        {
            return enumerable.AppendRange(elements);
        }

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

            return appendCollection == null ? enumerable : appendCollection.Aggregate(enumerable, Enumerable.Append);
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
                .Select((a, i) => new { a, i })
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

        /// <summary>
        /// Returns a specified page from the target sequence
        /// </summary>
        /// <param name="enumerable">Target sequence</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page <b>index</b> (the first page is zero-based)</param>
        /// <returns>A requested page of target sequence</returns>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> enumerable, int pageSize, int pageIndex)
        {
            return enumerable.Skip(pageIndex * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Returns a specified page from the target query
        /// </summary>
        /// <param name="queryable">Target query</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page <b>index</b> (the first page is zero-based)</param>
        /// <returns>A requested page of target query</returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, int pageSize, int pageIndex)
        {
            return queryable.Skip(pageIndex * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Checks if the target sequence has duplicates
        /// </summary>
        /// <param name="enumerable">Target sequence</param>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <returns>True if the target sequence has duplicates</returns>
        public static bool HasDuplicates<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Distinct().Count() != enumerable.Count();
        }
        
        /// <summary>
        /// Checks if the target sequence has duplicates
        /// </summary>
        /// <param name="enumerable">Target sequence</param>
        /// <param name="keySelector">A function to extract the key for each element</param>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <typeparam name="T2">The type of the key returned by keySelector</typeparam>
        /// <returns>True if the target sequence has duplicates</returns>
        public static bool HasDuplicates<T,T2>(this IEnumerable<T> enumerable,  Func<T,T2> keySelector)
        {
            return enumerable.GroupBy(keySelector).Any(gr => gr.Count() > 1);
        }
    }
}