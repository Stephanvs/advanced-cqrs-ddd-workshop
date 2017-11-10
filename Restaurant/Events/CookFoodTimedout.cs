using System;
using Restaurant.Models;

namespace Restaurant.Events
{
    public class CookFoodTimedout : Message
    {
        public CookFoodTimedout(OrderDocument order, Guid correlationId, Guid causationId, Guid? id = null)
            : base(order, correlationId, causationId, id)
        {
        }
    }
}