using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Restaurant.Events;

namespace Restaurant.Core
{
    public class MoreFairDispatcher<T> : IHandler<T>, IStartable where T : Message
    {
        private const int WaitTimeMs = 100;
        private const int MaxQueueLength = 5;
        private readonly IEnumerable<QueuedHandle<T>> _handlers;
        private int _processedMessageCount;
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        public MoreFairDispatcher(string name, IEnumerable<QueuedHandle<T>> handlers)
        {
            Name = name;
            _handlers = handlers;
        }

        public void Handle(T message)
        {
            _queue.Enqueue(message);
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var readyHandler = _handlers.FirstOrDefault(h => h.QueueDepth < MaxQueueLength);
                    if (readyHandler != null)
                    {
                        if (_queue.TryDequeue(out var message))
                        {
                            try
                            {
                                readyHandler.Handle(message);
                                Interlocked.Increment(ref _processedMessageCount);
                            }
                            catch
                            {
                                _queue.Enqueue(message);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(WaitTimeMs);
                    }
                }
            });
        }

        public int QueueDepth => _queue.Count;
        public string Name { get; }
        public int ProcessedMessageCount => _processedMessageCount;
    }
}