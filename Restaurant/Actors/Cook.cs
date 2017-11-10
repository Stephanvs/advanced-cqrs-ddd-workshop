using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Restaurant.Core;
using Restaurant.Events;

namespace Restaurant.Actors
{
    public class Cook : IHandler<OrderPlaced>
    {
        private readonly string _name;
        private readonly int _delayTime;
        private readonly IBus _bus;

        private readonly IDictionary<string, IEnumerable<string>> _menu
            = new Dictionary<string, IEnumerable<string>>
            {
                ["Crystal Meth"] = new[] { "drain cleaner", "amphetamine", "lantern fuel", "antifreeze" },
                ["MacBookPro"] = new[] { "aluminum", "lcd", "plastic" }
            };

        public Cook(string name, int delayTime, IBus bus)
        {
            _name = name;
            _delayTime = delayTime;
            _bus = bus;
        }

        public void Handle(OrderPlaced message)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Thread.Sleep(_delayTime);

            message.Order.CookingMinutes = stopwatch.Elapsed.TotalSeconds;
            message.Order.CookName = _name;

            stopwatch.Stop();

            foreach (var item in message.Order.LineItems)
            {
                message.Order.Ingredients.AddRange(_menu[item.Name]);
                //order.AddIngredient(string.Join(", ", _menu[item.Name]));
            }

            _bus.Publish(new FoodCooked(message.Order));
        }

        //public void Handle(Order order)
        //{
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();

        //    Thread.Sleep(1000);

        //    order.CookingMinutes = stopwatch.Elapsed.TotalSeconds;
        //    order.CookName = _name;

        //    stopwatch.Stop();

        //    foreach (var item in order.LineItems)
        //    {
        //        order.AddIngredient(string.Join(", ", _menu[item.Name]));
        //    }

        //    _orderHandler.Handle(order);
        //}
    }
}