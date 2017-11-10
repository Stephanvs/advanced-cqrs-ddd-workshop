using System;
using System.Collections.Generic;
using Restaurant.Events;

namespace Restaurant.Core
{
    public class TopicBasedPubSub : IBus
    {
        private readonly IDictionary<string, IEnumerable<dynamic>> _subscribers
            = new Dictionary<string, IEnumerable<dynamic>>();

        private readonly object _lockObj = new object();

        public void Publish<T>(string topic, T message) where T: Message
        {
            PublishTopic(topic, message);
            PublishCorrelationId(message.CorrelationId, message);
        }

        public void Publish<T>(T message) where T : Message
        {
            Publish(typeof(T).Name, message);
        }

        private void PublishCorrelationId(Guid correlationId, Message message)
        {
            if (_subscribers.ContainsKey(message.CorrelationId.ToString()))
            {
                foreach (var sub in _subscribers[correlationId.ToString()])
                {
                    sub.Handle(message);
                }
            }
        }

        private void PublishTopic<T>(string topic, T message) where T : Message
        {
            if (_subscribers.ContainsKey(topic))
            {
                foreach (var sub in _subscribers[topic])
                {
                    sub.Handle(message);
                }
            }
        }

        public void Subscribe<T>(string topic, IHandler<T> handler) where T : Message
        {
            // To be lock-free in Publish, copy the entire list of subscribers for a key, and replace the instance with a new one.
            lock (_lockObj)
            {
                if (_subscribers.ContainsKey(topic))
                {
                    var subscribers = _subscribers[topic];
                    _subscribers[topic] = new List<dynamic>(subscribers)
                    {
                        handler
                    };
                }
                else
                {
                    var subscribers = new List<dynamic> { handler };
                    _subscribers.Add(topic, subscribers);
                }
            }
        }

        public void Subscribe<T>(IHandler<T> handler) where T : Message
        {
            Subscribe(typeof(T).Name, handler);
        }

        public void Subscribe<T>(Guid correlationId, IHandler<T> handler) where T : Message
        {
            Subscribe(correlationId.ToString(), handler);
        }

        public void Unsubscribe(string topic)
        {
            _subscribers.Remove(topic);
        }

        public void Unsubscribe(Guid correlationId)
        {
            Unsubscribe(correlationId.ToString());
        }
    }

    public interface IBus
    {
        void Publish<T>(string topic, T message) where T : Message;

        void Publish<T>(T message) where T : Message;

        void Subscribe<T>(string topic, IHandler<T> handler) where T : Message;

        void Subscribe<T>(Guid correlationId, IHandler<T> handler) where T : Message;

        void Subscribe<T>(IHandler<T> handler) where T : Message;

        void Unsubscribe(string topic);

        void Unsubscribe(Guid correlationId);
    }
}