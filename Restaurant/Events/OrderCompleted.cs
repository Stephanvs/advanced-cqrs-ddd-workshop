using System;
using Restaurant.Models;

namespace Restaurant.Events
{
    public class OrderCompleted : Message
    {
        public OrderCompleted(OrderDocument order, Guid correlationId, Guid causationId, Guid? id = null) : base(order, correlationId, causationId, id)
        {
        }
    }
}