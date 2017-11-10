using Restaurant.Core;
using Restaurant.Models;

namespace Restaurant.Actors
{
    public class MinionFactory
    {
        private readonly IBus _bus;

        public MinionFactory(IBus bus)
        {
            _bus = bus;
        }

        public IMinion Create(OrderDocument order)
        {
            if (order.IsDodgyCustomer)
            {
                return new DodgyMinion(_bus);
            }
            else
            {
                return new RegularMinion(_bus);
            }
        }
    }
}