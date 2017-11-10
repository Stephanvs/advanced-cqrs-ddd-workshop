using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Restaurant.Commands;
using Restaurant.Core;
using Restaurant.Events;

namespace Restaurant.Actors
{
    public class AlarmClock : IHandler<DelayPublish>, IStartable
    {
        private readonly IBus _bus;
        private int _processedMessagesCount;
        private readonly List<DelayPublish> _list = new List<DelayPublish>();
        private readonly object _lock = new object();

        public int QueueDepth => _list.Count;

        public string Name => "AlarmClock";

        public int ProcessedMessageCount => _processedMessagesCount;

        public AlarmClock(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(DelayPublish message)
        {
            lock (_lock)
            {
                _list.Add(message);
            }
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    lock (_lock)
                    {
                        for (var i = _list.Count - 1; i >= 0; i--)
                        {
                            var message = _list[i];

                            if (message.PublishingOn < DateTime.Now)
                            {
                                _bus.Publish(message.Message);
                                _list.Remove(message);
                                _processedMessagesCount++;
                            }
                        }
                    }

                    Thread.Sleep(200);
                }
            });
        }
    }
}