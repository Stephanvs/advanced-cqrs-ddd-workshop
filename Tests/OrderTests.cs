using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restaurant;
using Restaurant.Actors;

namespace Tests
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void X()
        {
            var printer = A.Fake<IHandleOrder>();
            var waiter = new Waiter("Walt", printer);
            waiter.PlaceOrder(new LineItem("foo", 2));

            A.CallTo(() => printer.Handle(A<OrderDocument>._)).MustHaveHappened();
        }
    }
}