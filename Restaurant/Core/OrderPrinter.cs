using System;
using Newtonsoft.Json.Linq;
using Restaurant.Commands;

namespace Restaurant.Core
{
    public class OrderPrinter : IHandler<PrintOrder>
    {
        public void Handle(PrintOrder order)
        {
            Console.WriteLine(JObject.FromObject(order));
        }
    }
}