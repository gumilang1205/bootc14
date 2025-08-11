public interface IShippingService
{
    // Metode ini akan meniru proses pengiriman
    bool ShipOrder(int orderId, string shippingAddress);
}