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
            var semaphore = GetOrCreate(key);
            semaphore.Semaphore.Wait();

            return semaphore;
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> with the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <returns>A task that results in an <see cref="IDisposable"/> object that should be disposed after the concurrent operation completes</returns>
        public static async Task<IDisposable> WaitAsync(object key)
        {
            var semaphore = GetOrCreate(key);
            await semaphore.Semaphore.WaitAsync().ConfigureAwait(false);

            return semaphore;
        }
    }
}