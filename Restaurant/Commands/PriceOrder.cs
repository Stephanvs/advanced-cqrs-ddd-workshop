using System;
using Restaurant.Events;
using Restaurant.Models;

namespace Restaurant.Commands
{
    public class PriceOrder : Message
    {
        public PriceOrder(OrderDocument order, Guid correlationId, Guid causationId, Guid? id = null) : base(order, correlationId, causationId, id)
        {
        }
    }
}