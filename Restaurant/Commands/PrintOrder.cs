using System;
using Restaurant.Events;
using Restaurant.Models;

namespace Restaurant.Commands
{
    public class PrintOrder : Message
    {
        public PrintOrder(OrderDocument order, Guid correlationId, Guid causationId, Guid? id = null)
            : base(order, correlationId, causationId, id)
        {
        }
    }
}