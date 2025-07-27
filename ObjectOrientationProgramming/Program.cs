using ObjectOrientationProgramming.Abstraction;
using ObjectOrientationProgramming.AbstractionInterface;
using ObjectOrientationProgramming.Encapsulation;
using ObjectOrientationProgramming.Inheritance;
using ObjectOrientationProgramming.Polymorphism.Overriding;
class Program
{
    static void Main(string[] args)
    {
        var setor = new BankAccount(2000);
        setor.Deposit(1000);
        Console.WriteLine($"Total Saldo {setor.GetSaldo()}");
        //Inheritance
        Dog dog = new Dog();
        dog.Name = "Aiko";
        dog.Eat();
        dog.Bark();

        Car car = new Car("Toyota", "Sedan");
        car.ShowBrand();
        car.ShowCar();
        //Polymorphism
        Hewan hewan = new Hewan();
        hewan.Suara();
        Kucing kucing = new Kucing();
        kucing.Suara();
        Anjing anjing = new Anjing();
        anjing.Suara();
        //overloading
        Person person = new Person();
        Person person1 = new Person("Bambang");
        Person person2 = new Person("Kento", 90);

        person.Info();
        person1.Info();
        person2.Info();
        //abstraction
        Sapi sapi = new Sapi();
        sapi.MakeSound();
        sapi.Sleep();
        Burung burung = new Burung();
        burung.MakeSound();
        sapi.Sleep();
        //abstractionInterface\
        Circle cirlce = new Circle(9);
        Square square = new Square(7);

        Console.WriteLine($"hasil luas lingkaran adalah {cirlce.GetArea()}");
        Console.WriteLine($"hasil luas kotak adalah {square.GetArea()}");
    }
}
