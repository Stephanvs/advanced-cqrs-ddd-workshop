using System;
using Restaurant.Models;

namespace Restaurant.Events
{
    public class OrderPlaced : Message
    {

        public OrderPlaced(OrderDocument order, Guid correlationId, Guid causationId, Guid? id = null) : base(order,
            correlationId, causationId, id)
        {
        }
    }
}