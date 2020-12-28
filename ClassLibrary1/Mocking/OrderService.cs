namespace ClassLibrary1.Mocking
{
    public class OrderService
    {
        private readonly IStorage _storage;

        public OrderService(IStorage storage)
        {
            _storage = storage;
        }

        public int PlaceOrder(Order order)
        {
            var orderId = _storage.Store(order);

            // some other work

            return orderId;
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
    }

    public interface IStorage
    {
        int Store(Order order);
    }
}