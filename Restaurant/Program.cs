using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Restaurant.Actors;
using Restaurant.Core;
using Restaurant.Events;
using Restaurant.Models;

namespace Restaurant
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = new TopicBasedPubSub();
            var orderPrinter = new OrderPrinter();

            var cashier = new Cashier(orderPrinter);
            var assitantManager = new QueuedHandle<FoodCooked>("AssistantManager", new AssistantManager(bus));
            var jesse = new QueuedHandle<OrderPlaced>("Cook:Jesse", new Cook("Jesse", 600, bus));
            var walt = new QueuedHandle<OrderPlaced>("Cook:Walt", new Cook("Walt", 700, bus));
            var gus = new QueuedHandle<OrderPlaced>("Cook:Gus", new Cook("Gus", 1500, bus));

            var dispatcher = new MoreFairDispatcher<OrderPlaced>("FairDispatcher", new[] {jesse, walt, gus});

            var startables = new List<IStartable> {assitantManager, jesse, walt, gus, dispatcher };
            var waiter = new Waiter("Heisenberg", bus);

            bus.Subscribe(dispatcher);
            bus.Subscribe(assitantManager);
            bus.Subscribe(cashier);

            startables.ForEach(x => x.Start());

            for (var i = 0; i < 200; i++)
            {
                waiter.PlaceOrder(new LineItem("Crystal Meth", 3));
                Console.WriteLine("Order placed");
            }

            Console.WriteLine("Getting outstanding orders");

            Task.Run(() => GetOutstandingOrdersAndPay(cashier));

            StartMonitoringQueueDepthsAsync(startables).GetAwaiter().GetResult();
        }

        private static async Task StartMonitoringQueueDepthsAsync(IList<IStartable> queues)
        {
            while (true)
            {
                foreach (var queue in queues)
                {
                    Console.WriteLine($"[Stats] Queue: {queue.Name} \r\n\tQueueDepth: {queue.QueueDepth} \r\n\tProcessedMessages: {queue.ProcessedMessageCount}");
                }

                await Task.Delay(600).ConfigureAwait(false);
            }
        }

        private static void GetOutstandingOrdersAndPay(Cashier cashier)
        {
            while (true)
            {
                var order = cashier.GetOutstandingOrders().FirstOrDefault();

                if (order == null)
                {
                    continue;
                }

                cashier.Pay(order.OrderNumber);
                Console.WriteLine("Order payed");
            }
        }
    }
}
