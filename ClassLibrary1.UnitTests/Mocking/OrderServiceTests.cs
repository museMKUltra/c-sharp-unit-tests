using ClassLibrary1.Mocking;
using Moq;
using NUnit.Framework;

namespace ClassLibrary1.UnitTests.Mocking
{
    [TestFixture]
    public class OrderServiceTests
    {
        [Test]
        public void PlaceOrder_WhenCalled_StoreTheOrder()
        {
            var storage = new Mock<IStorage>();
            var orderService = new OrderService(storage.Object);

            var order = new Order();
            orderService.PlaceOrder(order);
            
            // verify Store had been called after implementation of PlaceOrder 
            storage.Verify(s => s.Store(order));
        }
    }
}