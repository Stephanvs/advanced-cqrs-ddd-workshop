using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Restaurant.Actors;
using Restaurant.Commands;
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

            var cashier = new Cashier(bus);
            var assitantManager = new QueuedHandle<PriceOrder>("AssistantManager", new AssistantManager(bus));
            var db = new ConcurrentDictionary<Guid, OrderDocument>();
            var jesse = new QueuedHandle<CookFood>("Cook:Jesse", new Cook("Jesse", 600, db, bus));
            var walt = new QueuedHandle<CookFood>("Cook:Walt", new Cook("Walt", 700, db, bus));
            var gus = new QueuedHandle<CookFood>("Cook:Gus", new Cook("Gus", 1500, db, bus));

            var dispatcher = new MoreFairDispatcher<CookFood>("FairDispatcher", new[] {jesse, walt, gus});

            var chaosMonkey = new ChaosMonkey<CookFood>(25, 25, dispatcher);
            var alarmClock = new AlarmClock(bus);

            var startables = new List<IStartable> {assitantManager, jesse, walt, gus, dispatcher, alarmClock };
            var waiter = new Waiter("Heisenberg", bus);

            var house = new MinionHouse(bus);

            bus.Subscribe(chaosMonkey);
            bus.Subscribe(assitantManager);
            bus.Subscribe(cashier);
            bus.Subscribe<OrderPlaced>(house);
            bus.Subscribe<OrderCompleted>(house);
            bus.Subscribe(orderPrinter);
            bus.Subscribe(alarmClock);

            startables.ForEach(x => x.Start());

            for (var i = 0; i < 50; i++)
            {
                waiter.PlaceOrder(new LineItem("Crystal Meth", 3));
                Console.WriteLine("Order placed");
            }

            Console.WriteLine("Getting outstanding orders");

            Task.Run(() => GetOutstandingOrdersAndPay(cashier));

            Task.Run(() => StartMonitoringQueueDepthsAsync(startables));
            StartMonitoringMinionHouseAsync(house).Wait();
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

        private static async Task StartMonitoringMinionHouseAsync(MinionHouse minionHouse)
        {
            while (true)
            {
             
                Console.WriteLine($"[Stats] MinionHouse: {minionHouse.Count()}");
             
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
