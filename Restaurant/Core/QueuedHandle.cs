using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Restaurant.Events;

namespace Restaurant.Core
{
    public class QueuedHandle<T> : IHandler<T>, IStartable where T : Message
    {
        private readonly IHandler<T> _handle;
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(true);
        private int _processedMessages;

        public QueuedHandle(string queueName, IHandler<T> handle)
        {
            Name = queueName;
            _handle = handle;
        }

        public void Handle(T message)
        {
            _queue.Enqueue(message);
            Interlocked.Increment(ref _processedMessages);
            _resetEvent.Set();
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (_queue.TryDequeue(out var message))
                    try
                    {
                        _handle.Handle(message);
                    }
                    catch
                    {
                        _queue.Enqueue(message);
                    }

                    _resetEvent.WaitOne();
                }
            });
        }

        public string Name { get; }

        public int QueueDepth => _queue.Count;

        public int ProcessedMessageCount => _processedMessages;
    }
}