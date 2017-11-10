using Restaurant.Commands;
using Restaurant.Core;
using Restaurant.Events;

namespace Restaurant.Actors
{
    public class DodgyMinion : IHandler<Message>, IMinion
    {
        private readonly IBus _bus;

        public DodgyMinion(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(Message message)
        {
            switch (message)
            {
                case OrderPlaced _:
                    {
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
                        _bus.Publish(new CookFood(message.Order, message.CorrelationId, message.Id));
                        break;
                    }
                case FoodCooked _:
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