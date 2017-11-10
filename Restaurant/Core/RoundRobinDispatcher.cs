using System.Collections.Generic;
using Restaurant.Events;

namespace Restaurant.Core
{
    public class RoundRobinDispatcher<T> : IHandler<T> where T: Message
    {
        private readonly Queue<IHandler<T>> _queue = new Queue<IHandler<T>>();

        public RoundRobinDispatcher(IEnumerable<IHandler<T>> handlers)
        {
            foreach (var handler in handlers)
            {
                _queue.Enqueue(handler);
            }
        }

        public void Handle(T message)
        {
            var handler = _queue.Dequeue();

            try
            {
                handler.Handle(message);
            }
            finally
            {
                _queue.Enqueue(handler);
            }
        }
    }
}