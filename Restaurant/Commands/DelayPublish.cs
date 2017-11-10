using System;
using Restaurant.Events;
using Restaurant.Models;

namespace Restaurant.Commands
{
    public class DelayPublish : Message
    {
        public DateTime PublishingOn { get; }
        public Message Message { get; set; }

        public DelayPublish(DateTime publishingOn, Message message)
            : base(message.Order, message.CorrelationId, message.Id, Guid.NewGuid())
        {
            PublishingOn = publishingOn;
            Message = message;
        }
    }
}