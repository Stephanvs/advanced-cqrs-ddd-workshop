using Restaurant.Models;

namespace Restaurant.Events
{
    public class FoodCooked : Message
    {
        public FoodCooked(OrderDocument order) : base(order)
        {
        }
    }
}