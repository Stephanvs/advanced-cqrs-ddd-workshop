using Restaurant.Models;

namespace Restaurant.Events
{
    public class OrderPaid : Message
    {

        public OrderPaid(OrderDocument order) : base (order)
        {
        }
    }
}