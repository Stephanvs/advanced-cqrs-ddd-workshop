using System;
using Restaurant.Commands;
using Restaurant.Core;
using Restaurant.Events;

namespace Restaurant.Actors
{
    public class RegularMinion : IHandler<Message>, IMinion
    {
        private readonly IBus _bus;
        private bool _foodCooked;

        public RegularMinion(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(Message message)
        {
            switch (message)
            {
                case OrderPlaced _:
                {
                    _bus.Publish(new CookFood(message.Order, message.CorrelationId, message.Id));
                    _bus.Publish(new DelayPublish(DateTime.Now.AddSeconds(10), new CookFoodTimedout(message.Order, message.CorrelationId, message.Id)));
                    break;
                }
                case CookFoodTimedout _:
                {
                    if (!_foodCooked)
                    {
                        _bus.Publish(new CookFood(message.Order, message.CorrelationId, message.Id));
                        _bus.Publish(new DelayPublish(DateTime.Now.AddSeconds(10),
                            new CookFoodTimedout(message.Order, message.CorrelationId, message.Id)));
                    }

                    break;
                }
                case FoodCooked _:
                {
                    _foodCooked = true;
                    _bus.Publish(new PriceOrder(message.Order, message.CorrelationId, message.Id));
                    break;
                }
                case OrderPriced _:
                {
                    _bus.Publish(new PayOrder(message.Order, message.CorrelationId, message.Id));
                    break;
                }
                case OrderPaid _:
                {
                    _bus.Publish(new PrintOrder(message.Order, message.CorrelationId, message.Id));
                    _bus.Publish(new OrderCompleted(message.Order, message.CorrelationId, message.Id));
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }
}