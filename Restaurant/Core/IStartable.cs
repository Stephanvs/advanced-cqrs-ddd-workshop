namespace Restaurant.Core
{
    public interface IStartable
    {
        void Start();
        int QueueDepth { get; }
        string Name { get; }

        int ProcessedMessageCount { get; }
    }
}