#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace CoreLib.CORE.Helpers.CollectionHelpers
{
    // Source: https://petemontgomery.wordpress.com/2011/02/10/a-universal-predicatebuilder/
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that evaluates to "true"
        /// </summary>
        /// <returns>Predicate that evaluates to "true"</returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return param => true;
        }

        /// <summary>
        /// Creates a predicate that evaluates to "false"
        /// </summary>
        /// <returns>Predicate that evaluates to "false"</returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return param => false;
        }

        /// <summary>
        /// Creates a predicate expression from the specified lambda expression
        /// </summary>
        /// <param name="lambda">Lambda expression</param>
        /// <returns>Predicate expression from the specified lambda expression</returns>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> lambda)
        {
            return lambda;
        }

        /// <summary>
        /// Combines the first predicate with the second using the logical "and"
        /// </summary>
        /// <param name="first">First predicate</param>
        /// <param name="second">Second predicate</param>
        /// <returns>Combined predicate</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// Combines the first predicate with the second using the logical "or"
        /// </summary>
        /// <param name="first">First predicate</param>
        /// <param name="second">Second predicate</param>
        /// <returns>Combined predicate</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>
        /// Negates the predicate
        /// </summary>
        /// <param name="expression">Lambda expression</param>
        /// <returns>Negated predicate</returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);

            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>
        /// Composes two predicates with provided <see cref="BinaryExpression"/>
        /// </summary>
        /// <param name="first">First predicate</param>
        /// <param name="second">Second predicate</param>
        /// <param name="merge"><see cref="BinaryExpression"/> to compose <paramref name="first"/> and <paramref name="second"/></param>
        /// <returns>Composed predicate</returns>
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            var secondBody = new ParameterRebinder(map).Visit(second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// A helper class that rebinds parameters of predicates
        /// </summary>
        private class ParameterRebinder : ExpressionVisitor
        {
            /// <summary>
            /// Map from parameters of predicates
            /// </summary>
            private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

            /// <summary>
            /// Rebinds parameters of predicates
            /// </summary>
            /// <param name="map">Map from parameters of predicates</param>
            internal ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                if (_map.TryGetValue(p, out var replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }
}