using System;
using System.Threading;
using System.Threading.Tasks;

namespace Emby.Mobile.Core.Util
{
    public class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly Task<Releaser> _releaser;

        public AsyncLock()
        {
            _semaphore = new SemaphoreSlim(1);
            _releaser = Task.FromResult(new Releaser(this));
        }

        public Task<Releaser> LockAsync()
        {
            var wait = _semaphore.WaitAsync();

            return wait.IsCompleted ? _releaser : wait.ContinueWith((task, reference) => new Releaser((AsyncLock)reference),
                                       this, CancellationToken.None,
                                       TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }


        public struct Releaser : IDisposable
        {
            private readonly AsyncLock _locked;

            internal Releaser(AsyncLock locked) { _locked = locked; }

            public void Dispose()
            {
                if (_locked != null)
                    _locked.Release();
            }
        }

        private void Release()
        {
            if (_semaphore != null)
                _semaphore.Release();
        }
    }
}
