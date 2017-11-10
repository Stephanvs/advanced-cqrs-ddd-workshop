using System;
using Newtonsoft.Json.Linq;
using Restaurant.Events;

namespace Restaurant.Core
{
    public class OrderPrinter : IHandler<OrderPaid>
    {
        public void Handle(OrderPaid order)
        {
            Console.WriteLine(JObject.FromObject(order));
        }
    }
}