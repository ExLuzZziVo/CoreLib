using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreLib.CORE.Types
{
    // https://stackoverflow.com/questions/31138179/asynchronous-locking-based-on-a-key/
    /// <summary>
    /// A helper class that provides methods for properly handling <see cref="SemaphoreSlim"/>s for concurrent operations
    /// </summary>
    public static class ConcurrentSemaphore
    {
        /// <summary>
        /// The global collection of semaphores currently in use
        /// </summary>
        private static readonly Dictionary<object, LockedSemaphore> Semaphores
            = new Dictionary<object, LockedSemaphore>();

        /// <summary>
        /// Gets or creates a new <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <returns>A special semaphore object with the specified <paramref name="key"/> and the current number of references to it, including the current reference</returns>
        private static LockedSemaphore GetOrCreate(object key)
        {
            LockedSemaphore item;

            lock (Semaphores)
            {
                if (Semaphores.TryGetValue(key, out item))
                {
                    ++item.RefCount;
                }
                else
                {
                    item = new LockedSemaphore(key, new SemaphoreSlim(1, 1));
                    Semaphores[key] = item;
                }
            }

            return item;
        }

        /// <summary>
        /// Waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <returns>An <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        public static IDisposable Wait(object key)
        {
            return Wait(key, Timeout.Infinite, CancellationToken.None);
        }

        /// <summary>
        /// Waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>, using the <see cref="TimeSpan"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds to wait, or a <see cref="TimeSpan"/> that represents -1 milliseconds to wait indefinitely</param>
        /// <returns>An <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        /// <exception cref="TimeoutException">Timeout expired</exception>
        public static IDisposable Wait(object key, TimeSpan timeout)
        {
            return Wait((int)timeout.TotalMilliseconds, CancellationToken.None);
        }

        /// <summary>
        /// Waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>, using the <see cref="TimeSpan"/> and the <see cref="CancellationToken"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds to wait, or a <see cref="TimeSpan"/> that represents -1 milliseconds to wait indefinitely</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe</param>
        /// <returns>An <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        /// <exception cref="TimeoutException">Timeout expired</exception>
        public static IDisposable Wait(object key, TimeSpan timeout, CancellationToken cancellationToken)
        {
            return Wait((int)timeout.TotalMilliseconds, cancellationToken);
        }

        /// <summary>
        /// Waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>, using the time interval in milliseconds
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite"/>(-1) to wait indefinitely</param>
        /// <returns>An <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        /// <exception cref="TimeoutException">Timeout expired</exception>
        public static IDisposable Wait(object key, int millisecondsTimeout)
        {
            return Wait(millisecondsTimeout, CancellationToken.None);
        }

        /// <summary>
        /// Waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>, using the <see cref="CancellationToken"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe</param>
        /// <returns>An <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        public static IDisposable Wait(object key, CancellationToken cancellationToken)
        {
            return Wait(key, Timeout.Infinite, cancellationToken);
        }

        /// <summary>
        /// Waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>, using the time interval milliseconds and the <see cref="CancellationToken"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite"/>(-1) to wait indefinitely</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe</param>
        /// <returns>An <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        /// <exception cref="TimeoutException">Timeout expired</exception>
        public static IDisposable Wait(object key, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            var semaphore = GetOrCreate(key);
            var entered = semaphore.Semaphore.Wait(millisecondsTimeout, cancellationToken);

            if (!entered)
            {
                throw new TimeoutException("The specified semaphore is busy(timed out)");
            }

            return semaphore;
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <returns>A task that results in an <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        public static Task<IDisposable> WaitAsync(object key)
        {
            return WaitAsync(key, Timeout.Infinite, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>, using the <see cref="TimeSpan"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds to wait, or a <see cref="TimeSpan"/> that represents -1 milliseconds to wait indefinitely</param>
        /// <returns>A task that results in an <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        /// <exception cref="TimeoutException">Timeout expired</exception>
        public static Task<IDisposable> WaitAsync(object key, TimeSpan timeout)
        {
            return WaitAsync((int)timeout.TotalMilliseconds, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>, using the <see cref="TimeSpan"/> and the <see cref="CancellationToken"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds to wait, or a <see cref="TimeSpan"/> that represents -1 milliseconds to wait indefinitely</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe</param>
        /// <returns>A task that results in an <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        /// <exception cref="TimeoutException">Timeout expired</exception>
        public static Task<IDisposable> WaitAsync(object key, TimeSpan timeout, CancellationToken cancellationToken)
        {
            return WaitAsync((int)timeout.TotalMilliseconds, cancellationToken);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>, using the time interval in milliseconds
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite"/>(-1) to wait indefinitely</param>
        /// <returns>A task that results in an <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        /// <exception cref="TimeoutException">Timeout expired</exception>
        public static Task<IDisposable> WaitAsync(object key, int millisecondsTimeout)
        {
            return WaitAsync(millisecondsTimeout, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>, using the <see cref="CancellationToken"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe</param>
        /// <returns>A task that results in an <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        public static Task<IDisposable> WaitAsync(object key, CancellationToken cancellationToken)
        {
            return WaitAsync(key, Timeout.Infinite, cancellationToken);
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>, using the time interval milliseconds and the <see cref="CancellationToken"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite"/>(-1) to wait indefinitely</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe</param>
        /// <returns>A task that results in an <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        /// <exception cref="TimeoutException">Timeout expired</exception>
        public static async Task<IDisposable> WaitAsync(object key, int millisecondsTimeout,
            CancellationToken cancellationToken)
        {
            var semaphore = GetOrCreate(key);

            var entered = await semaphore.Semaphore.WaitAsync(millisecondsTimeout, cancellationToken)
                .ConfigureAwait(false);

            if (!entered)
            {
                throw new TimeoutException("The specified semaphore is busy(timed out)");
            }

            return semaphore;
        }

        /// <summary>
        /// A helper class to handle the current number of references to the provided <see cref="SemaphoreSlim"/>
        /// </summary>
        private class LockedSemaphore : IDisposable
        {
            internal LockedSemaphore(object key, SemaphoreSlim semaphore)
            {
                RefCount = 1;
                Key = key;
                Semaphore = semaphore;
            }

            /// <summary>
            /// The current number of references to this semaphore
            /// </summary>
            internal int RefCount { get; set; }

            /// <summary>
            /// Semaphore
            /// </summary>
            internal SemaphoreSlim Semaphore { get; }

            /// <summary>
            /// Semaphore's unique key
            /// </summary>
            internal object Key { get; }

            public void Dispose()
            {
                lock (Semaphores)
                {
                    --RefCount;

                    if (RefCount == 0)
                    {
                        Semaphores.Remove(Key);
                    }
                }

                Semaphore.Release();
            }
        }
    }
}