using Restaurant.Models;

namespace Restaurant.Events
{
    public class OrderPriced : Message
    {

        public OrderPriced(OrderDocument order):base(order)
        {
        }
    }
}