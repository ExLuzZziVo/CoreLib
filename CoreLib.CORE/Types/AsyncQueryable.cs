#if !NETSTANDARD2_0

#region

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace CoreLib.CORE.Types
{
    // https://stackoverflow.com/questions/48743165/toarrayasync-throws-the-source-iqueryable-doesnt-implement-iasyncenumerable
    /// <summary>
    /// The class that helps to convert the <see cref="IEnumerable{T}"/> to <see cref="IQueryable{T}"/> that can be queried asynchronously
    /// </summary>
    /// <typeparam name="T">The type of the elements of source</typeparam>
    internal class AsyncQueryable<T>: EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public AsyncQueryable(IEnumerable<T> enumerable): base(enumerable) { }

        public AsyncQueryable(Expression expression): base(expression) { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncEnumerator(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider => new AsyncQueryProvider(this);

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new AsyncEnumerator(this.AsEnumerable().GetEnumerator());
        }

        private class AsyncQueryProvider: IQueryProvider
        {
            private readonly IQueryProvider _source;

            public AsyncQueryProvider(IQueryProvider source)
            {
                _source = source;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return _source.CreateQuery(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new AsyncQueryable<TElement>(_source.CreateQuery<TElement>(expression));
            }

            public object Execute(Expression expression)
            {
                return Execute<T>(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _source.Execute<TResult>(expression);
            }
        }

        private class AsyncEnumerator: IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _source;

            public AsyncEnumerator(IEnumerator<T> source)
            {
                _source = source;
            }

            public T Current => _source.Current;

            public ValueTask DisposeAsync()
            {
                return new ValueTask(Task.CompletedTask);
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(_source.MoveNext());
            }
        }
    }
}

#endif
