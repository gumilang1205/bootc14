// Menggunakan Pola Desain Observer

// Antarmuka Penerbit (Observable)
public interface IProductObservable
{
    void Attach(IProductObserver observer);
    void Detach(IProductObserver observer);
    void NotifyObservers();
}

// Antarmuka Pelanggan (Observer)
public interface IProductObserver
{
    void Update(string productName);
}

// Penerbit Konkret (Concrete Observable)
public class Store : IProductObservable
{
    private List<IProductObserver> _observers = new List<IProductObserver>();
    private string _latestProduct;

    public void Attach(IProductObserver observer)
    {
        _observers.Add(observer);
        Console.WriteLine("Observer attached.");
    }

    public void Detach(IProductObserver observer)
    {
        _observers.Remove(observer);
        Console.WriteLine("Observer detached.");
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_latestProduct);
        }
    }

    public void AddNewProduct(string productName)
    {
        _latestProduct = productName;
        Console.WriteLine($"A new product has been added: {_latestProduct}");
        NotifyObservers();
    }
}

// Pelanggan Konkret (Concrete Observer)
public class EmailService : IProductObserver
{
    public void Update(string productName)
    {
        Console.WriteLine($"Email service notified: New product available: {productName}");
    }
}

public class NotificationService : IProductObserver
{
    public void Update(string productName)
    {
        Console.WriteLine($"Notification service notified: New product available: {productName}");
    }
}

// Contoh penggunaan
public class Program
{
    public static void Main(string[] args)
    {
        Store store = new Store();
        
        EmailService emailService = new EmailService();
        NotificationService notificationService = new NotificationService();

        store.Attach(emailService);
        store.Attach(notificationService);

        Console.WriteLine();
        store.AddNewProduct("Laptop Dell");
        Console.WriteLine();

        store.Detach(emailService);
        
        Console.WriteLine();
        store.AddNewProduct("Keyboard Logitech");
    }
}

/*
Output:
Observer attached.
Observer attached.

A new product has been added: Laptop Dell
Email service notified: New product available: Laptop Dell
Notification service notified: New product available: Laptop Dell

Observer detached.

A new product has been added: Keyboard Logitech
Notification service notified: New product available: Keyboard Logitech
*/