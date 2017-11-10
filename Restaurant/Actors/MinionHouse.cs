using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Restaurant.Core;
using Restaurant.Events;

namespace Restaurant.Actors
{
    public class MinionHouse
        : IHandler<OrderPlaced>
            , IHandler<OrderCompleted>
            , IHandler<Message>
    {
        private readonly IBus _bus;
        private readonly MinionFactory _minionFactory;

        private readonly IDictionary<Guid, IMinion> _midgets = new ConcurrentDictionary<Guid, IMinion>();

        public MinionHouse(IBus bus)
        {
            _bus = bus;
            _minionFactory = new MinionFactory(bus);
        }

        public void Handle(OrderPlaced message)
        {
            var midget = _minionFactory.Create(message.Order);
            _midgets.Add(message.CorrelationId, midget);
            _bus.Subscribe<Message>(message.CorrelationId, this);
        }

        public void Handle(OrderCompleted message)
        {
            _bus.Unsubscribe(message.CorrelationId);
            _midgets.Remove(message.CorrelationId);
        }

        public void Handle(Message message)
        {
            var midget = _midgets[message.CorrelationId];
            midget.Handle(message);
        }

        public int Count()
        {
            return _midgets.Count;
        }
    }
}