using System;
using Restaurant.Models;

namespace Restaurant.Events
{
    public class OrderPaid : Message
    {

        public OrderPaid(OrderDocument order, Guid correlationId, Guid causationId, Guid? id = null) : base (order, correlationId, causationId, id)
        {
        }
    }
}