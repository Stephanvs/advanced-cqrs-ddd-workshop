using System;
using Restaurant.Core;
using Restaurant.Events;
using Restaurant.Models;

namespace Restaurant.Actors
{
    public class Waiter
    {
        private readonly string _name;
        private readonly IBus _bus;
        private static readonly Random Random = new Random();

        public Waiter(string name, IBus bus)
        {
            _name = name;
            _bus = bus;
        }

        public void PlaceOrder(LineItem item)
        {
            var order = new OrderDocument
            {
                OrderNumber = Guid.NewGuid(),
                Waiter = _name
            };
            //order.AddLineItem(item);
            order.LineItems.Add(item);

            order.IsDodgyCustomer = DateTime.UtcNow.Ticks % 2 == 0;

            _bus.Publish(new OrderPlaced(order, Guid.NewGuid(), Guid.Empty));
        }
    }
}