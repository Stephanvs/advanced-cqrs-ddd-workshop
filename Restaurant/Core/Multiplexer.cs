using System.Collections.Generic;
using Restaurant.Events;

namespace Restaurant.Core
{
    public class Multiplexer<T> : IHandler<T> where T: Message
    {
        private readonly IEnumerable<IHandler<T>> _handlers;

        public Multiplexer(IEnumerable<IHandler<T>> handlers)
        {
            _handlers = handlers;
        }

        public void Handle(T message)
        {
            foreach (var handler in _handlers)
            {
                handler.Handle(message);
            }
        }
    }
}