using System;
using System.Runtime.InteropServices;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Restaurant;
using Restaurant.Actors;
using Restaurant.Commands;
using Restaurant.Core;
using Restaurant.Events;
using Restaurant.Models;

namespace Tests
{
    [TestFixture]
    public class When_the_order_is_placed_then_th
    {
        [Test]
        public void Then_a_cook_food_command_should_be_sent()
        {
            var bus = new Mock<IBus>();
            var midger = new RegularMidget(bus.Object);
            var orderDocument = new OrderDocument();
            var correlationId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            midger.Handle(new OrderPlaced(orderDocument, correlationId, Guid.NewGuid(), messageId));

            bus.Verify(f=> f.Publish(It.Is<CookFood>(c=> c.Order == orderDocument &&
            c.CorrelationId == correlationId &&
            c.CausationId == messageId)));
        }
    }
}
