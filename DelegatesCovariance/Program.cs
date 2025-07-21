using System;
namespace DelegatesCovariane
{
    class Food { }
    class Fruit : Food { }
    class Apple : Fruit { }
    class Program
    {
        // delegate yang mengembalikan tipe Food
        delegate Food GetFoodDelegate();
        //Metode yang mengembalikan tipe yang lebih spesifik
        //(Apple adalah Food)
        static Apple GetAnApple()
        {
            return new Apple();
        }

        static void Main(string[] args)
        {
            GetFoodDelegate foodProducer = GetAnApple;
            Food myFood = foodProducer();
            Console.WriteLine($"Produced : {myFood.GetType().Name}");

            Func<Food> funcFoodProducer = GetAnApple;
            Food anotherFood = funcFoodProducer();
            Console.WriteLine($"Produced via Func: {anotherFood.GetType().Name}");

        }
    }
    
}