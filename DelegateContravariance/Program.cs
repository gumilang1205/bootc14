using System;
namespace DelegateContravarianc
{
    class Animal { }
    class Dog : Animal { }
    class GoldenRetriever : Dog { }
    class Program
    {
        //delegate yg menerima input tipe dog
        delegate void TrainDogDelegate(Dog dog);
        static void TrainAnimal(Animal animal)
        {
            Console.WriteLine($"Training a {animal.GetType().Name}");
        }
        static void Main(string[] args)
        {
            TrainDogDelegate dogTrainer = TrainAnimal;
            dogTrainer(new Dog());
            dogTrainer(new GoldenRetriever());

            Action<Dog> actionDogTrainer = TrainAnimal;
            actionDogTrainer(new Dog());
            actionDogTrainer(new GoldenRetriever());
        }
    }
}