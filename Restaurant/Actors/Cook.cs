using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Restaurant.Commands;
using Restaurant.Core;
using Restaurant.Events;
using Restaurant.Models;

namespace Restaurant.Actors
{
    public class Cook : IHandler<CookFood>
    {
        private readonly string _name;
        private readonly int _delayTime;
        private readonly IDictionary<Guid, OrderDocument> _db;
        private readonly IBus _bus;

        private readonly IDictionary<string, IEnumerable<string>> _menu
            = new Dictionary<string, IEnumerable<string>>
            {
                ["Crystal Meth"] = new[] { "drain cleaner", "amphetamine", "lantern fuel", "antifreeze" },
                ["MacBookPro"] = new[] { "aluminum", "lcd", "plastic" }
            };

        public Cook(string name, int delayTime, IDictionary<Guid, OrderDocument> db, IBus bus)
        {
            _name = name;
            _delayTime = delayTime;
            _db = db;
            _bus = bus;
        }

        public void Handle(CookFood message)
        {
            if (_db.ContainsKey(message.CorrelationId))
            {
                return;
            }

            _db.Add(message.CorrelationId, message.Order);

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

            _bus.Publish(new FoodCooked(message.Order, message.CorrelationId, message.Id));
        }
    }
}