namespace ObjectOrientationProgramming.Abstraction
{
    public abstract class Animal
    {
        public abstract void MakeSound();
        public void Sleep()
        {
            Console.WriteLine("Hewan tidur....");
        }
    }
    public class Sapi : Animal
    {
        public override void MakeSound()
        {
            Console.WriteLine("Sapi bersuara : Moowwwww");
        }
    }
    public class Burung : Animal
    {
        public override void MakeSound()
        {
            Console.WriteLine("Burung bersuara : cicitcuitcuit");
        }
    }
}