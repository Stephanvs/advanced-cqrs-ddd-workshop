using Restaurant.Events;

namespace Restaurant.Core
{
    public interface IHandler<in T>
        where T : Message
    {
        void Handle(T message);
    }
}