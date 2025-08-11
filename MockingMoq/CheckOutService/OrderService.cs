public class OrderService
{
    private readonly IShippingService _shippingService;

    // Dependensi disuntikkan melalui constructor
    public OrderService(IShippingService shippingService)
    {
        _shippingService = shippingService;
    }

    public bool ProcessOrder(int orderId, string shippingAddress)
    {
        // ... logika pemrosesan pesanan lainnya ...

        // Memanggil metode dari dependensi
        bool shippingResult = _shippingService.ShipOrder(orderId, shippingAddress);

        return shippingResult;
    }
}