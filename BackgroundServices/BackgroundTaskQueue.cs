using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MVCWebApp.BackgroundServices
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }

    public class BackgroundTaskQueue(ILogger<BackgroundTaskQueue> _logger) : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new();
        private readonly SemaphoreSlim _signal = new(0);

        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));

            _logger.LogInformation("QueueBackgroundWorkItem Enqueue: {workItem}", 
                JsonConvert.SerializeObject(workItem.Target));

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            _logger.LogInformation("QueueBackgroundWorkItem Dequeue: {workItem}",
                JsonConvert.SerializeObject(workItem.Target));

            return workItem;
        }
    }
}
