using Restaurant.Models;

namespace Restaurant.Events
{
    public class OrderPlaced : Message
    {

        public OrderPlaced(OrderDocument order):base(order)
        {
        }
    }
}