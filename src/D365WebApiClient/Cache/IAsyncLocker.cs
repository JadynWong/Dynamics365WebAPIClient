using System;
using System.Threading.Tasks;

namespace D365WebApiClient.Cache
{
    public interface IAsyncLocker
    {
        Task<TResult> RunWithLockAsync<TResult>(string name, Func<Task<TResult>> body);
    }
}