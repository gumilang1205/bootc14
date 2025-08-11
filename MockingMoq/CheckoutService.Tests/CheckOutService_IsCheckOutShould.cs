namespace Mock.Test;

using Moq;

[TestFixture]
public class OrderServiceTest
{
    [Test]
    public void ProcessOrder_WhenCalled_ReturnTrue()
    {
        //* Arrage - Menyiapkan Mock Object
        var mockShippingService = new Mock<IShippingService>();

        //* Setup - Atur Mock Behavior : Jika ShipOrder dipanggil dengan orderId=1 dan alamat apa pun maka return true
        mockShippingService.Setup(s => s.ShipOrder(1, It.IsAny<string>())).Returns(true);

        //* Inject - Membuat instance OrderService dengan MockObject
        var orderService = new OrderService(mockShippingService.Object);

        //* Act - Panggil test method
        var result = orderService.ProcessOrder(1, "Salatiga");

        //* Assert - Verifikasi hasil
        Assert.IsTrue(result);

        //* Verify - Memastikan method mock dipanggil
        mockShippingService.Verify(s => s.ShipOrder(1, "Salatiga"), Times.Once);
    }
}

// Create NUnit Project
// Add mock from nuget or "dotnet add package Moq"
// Reference to main project csproj