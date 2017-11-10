using Restaurant.Models;

namespace Restaurant.Events
{
    public class OrderReady : Message
    {
        public OrderReady(OrderDocument order) : base(order)
        {
        }
    }
}