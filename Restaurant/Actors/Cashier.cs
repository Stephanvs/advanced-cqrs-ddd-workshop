using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Core;
using Restaurant.Events;
using Restaurant.Models;

namespace Restaurant.Actors
{
    public class Cashier : IHandler<OrderPriced>
    {
        private readonly IHandler<OrderPaid> _orderHandler;
        private readonly ConcurrentDictionary<Guid, OrderDocument> _orders = new ConcurrentDictionary<Guid, OrderDocument>();

        public Cashier(IHandler<OrderPaid> orderHandler)
        {
            _orderHandler = orderHandler;
        }

        public void Handle(OrderPriced message)
        {
            _orders.TryAdd(message.Order.OrderNumber, message.Order);
        }

        public void Pay(Guid orderId)
        {
            if (!_orders.ContainsKey(orderId))
            {
                return;
            }

            var order = _orders[orderId];

            order.Paid = true;

            _orderHandler.Handle(new OrderPaid(order));
        }

        public IEnumerable<OrderDocument> GetOutstandingOrders()
        {
            return _orders
                .Where(o => !o.Value.Paid)
                .Select(o => o.Value);
        }
    }
}