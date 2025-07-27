using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ObjectOrientationProgramming.Inheritance
{
    public class Animal
    {
        public string Name { get; set; }
        public void Eat()
        {
            Console.WriteLine($"{Name} sedang makan");
        }
    }
    public class Dog : Animal
    {
        public void Bark()
        {
            Console.WriteLine($"{Name} menggonggong");
        }
    }
    //inheritance dengan constructor
        public class Vehicle
    {
        public string Brand { get; set; }
        public Vehicle(string brand)
        {
            Brand = brand;
        }
        public void ShowBrand()
        {
            Console.WriteLine($"Brand : {Brand}");
        }
    }
    public class Car : Vehicle
    {
        public string Model { get; set; }
        public Car(string brand, string model) : base(brand)
        {
            Model = model;
        }
        public void ShowCar()
        {
            Console.WriteLine($"Brand : {Brand}, Model : {Model}");
        }
    }
}