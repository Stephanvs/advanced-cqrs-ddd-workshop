using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Commands;
using Restaurant.Core;
using Restaurant.Events;
using Restaurant.Models;

namespace Restaurant.Actors
{
    public class Cashier : IHandler<PayOrder>
    {
        private readonly IBus _bus;
        private readonly ConcurrentDictionary<Guid, Message> _messages = new ConcurrentDictionary<Guid, Message>();

        public Cashier(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(PayOrder message)
        {
            _messages.TryAdd(message.Order.OrderNumber, message);
        }

        public void Pay(Guid orderId)
        {
            if (!_messages.ContainsKey(orderId))
            {
                return;
            }

            var message = _messages[orderId];

            message.Order.Paid = true;

            _bus.Publish(new OrderPaid(message.Order, message.CorrelationId, message.Id));
        }

        public IEnumerable<OrderDocument> GetOutstandingOrders()
        {
            return _messages
                .Where(o => !o.Value.Order.Paid)
                .Select(o => o.Value.Order);
        }
    }
}