using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Restaurant.Commands;
using Restaurant.Core;
using Restaurant.Events;
using Restaurant.Models;

namespace Restaurant.Actors
{
    public class MidgetHouse
        : IHandler<OrderPlaced>
        , IHandler<OrderCompleted>
        , IHandler<Message>
    {
        private readonly IBus _bus;
        private readonly MidgetFactory _midgetFactory;

        private readonly IDictionary<Guid, IMidget> _midgets = new ConcurrentDictionary<Guid, IMidget>();

        public MidgetHouse(IBus bus)
        {
            _bus = bus;
            _midgetFactory = new MidgetFactory(bus);
        }

        public void Handle(OrderPlaced message)
        {
            var midget = _midgetFactory.Create(message.Order);
            _midgets.Add(message.CorrelationId, midget);
            _bus.Subscribe<Message>(message.CorrelationId, this);
        }

        public void Handle(OrderCompleted message)
        {
            _bus.Unsubscribe(message.CorrelationId);
            _midgets.Remove(message.CorrelationId);
        }

        public void Handle(Message message)
        {
            var midget = _midgets[message.CorrelationId];
            midget.Handle(message);
        }

        public int Count()
        {
            return _midgets.Count;
        }
    }

    public class MidgetFactory
    {
        private readonly IBus _bus;

        public MidgetFactory(IBus bus)
        {
            _bus = bus;
        }

        public IMidget Create(OrderDocument order)
        {
            if (order.IsDodgyCustomer)
            {
                return new DodgyMidget(_bus);
            }
            else
            {
                return new RegularMidget(_bus);
            }
        }
    }

    public interface IMidget
    {
        void Handle(Message message);
    }

    public class RegularMidget : IHandler<Message>, IMidget
    {
        private readonly IBus _bus;
        private bool _foodCooked;

        public RegularMidget(IBus bus)
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

    public class DodgyMidget : IHandler<Message>, IMidget
    {
        private readonly IBus _bus;

        public DodgyMidget(IBus bus)
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