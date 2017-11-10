using Restaurant.Models;

namespace Restaurant.Events
{
    public abstract class Message
    {
        protected Message(OrderDocument order)
        {
            Order = order;
        }

        public OrderDocument Order { get; }
    }
}