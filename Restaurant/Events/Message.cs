using System;
using Restaurant.Models;

namespace Restaurant.Events
{
    public abstract class Message
    {
        protected Message(OrderDocument order, Guid correlationId, Guid causationId, Guid? id = null)
        {
            Order = order;
            Id = id ?? Guid.NewGuid();
            CorrelationId = correlationId;
            CausationId = causationId;
        }

        public OrderDocument Order { get; }

        public Guid Id { get; }

        public Guid CorrelationId { get; }

        public Guid CausationId { get; }
    }
}