using System;
using Restaurant.Core;
using Restaurant.Events;

namespace Restaurant.Actors
{
    public class AssistantManager : IHandler<FoodCooked>
    {
        private readonly IBus _bus;
        private static readonly Random Random = new Random(1);
        private const int MaxPrice = 999999999;

        public AssistantManager(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(FoodCooked message)
        {
            foreach (var item in message.Order.LineItems)
            {
                item.Price = Random.Next(MaxPrice);
            }

            _bus.Publish(new OrderPriced(message.Order));
        }
    }
}