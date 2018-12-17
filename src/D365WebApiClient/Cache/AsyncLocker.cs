using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D365WebApiClient.Cache
{
    public class AsyncLocker : IAsyncLocker
    {
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _lockDict = new ConcurrentDictionary<string, SemaphoreSlim>();

        private SemaphoreSlim GetLock(string name)
        {
            return _lockDict.GetOrAdd(name, s => new SemaphoreSlim(1, 1));
        }

        public async Task<TResult> RunWithLockAsync<TResult>(string name, Func<Task<TResult>> body)
        {
            var slimLock = GetLock(name);
            try
            {
                await slimLock.WaitAsync();
                return await body();
            }
            finally
            {
                slimLock.Release();
            }
        }

    }

}
