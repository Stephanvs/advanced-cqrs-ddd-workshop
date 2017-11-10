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

            _bus.Publish(new OrderPlaced(order));
        }
    }
}