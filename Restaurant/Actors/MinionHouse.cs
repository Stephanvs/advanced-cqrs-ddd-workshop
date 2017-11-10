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

        private readonly IDictionary<Guid, IMinion> _minions = new ConcurrentDictionary<Guid, IMinion>();

        public MinionHouse(IBus bus)
        {
            _bus = bus;
            _minionFactory = new MinionFactory(bus);
        }

        public void Handle(OrderPlaced message)
        {
            var minion = _minionFactory.Create(message.Order);
            _minions.Add(message.CorrelationId, minion);
            _bus.Subscribe<Message>(message.CorrelationId, this);
        }

        public void Handle(OrderCompleted message)
        {
            _bus.Unsubscribe(message.CorrelationId);
            _minions.Remove(message.CorrelationId);
        }

        public void Handle(Message message)
        {
            var minion = _minions[message.CorrelationId];
            minion.Handle(message);
        }

        public int Count()
        {
            return _minions.Count;
        }
    }
}