using Restaurant.Events;

namespace Restaurant.Actors
{
    public interface IMinion
    {
        void Handle(Message message);
    }
}