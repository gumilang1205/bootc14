namespace ObjectOrientationProgramming.Polymorphism.Overriding
{
    public class Hewan
    {
        public virtual void Suara()
        {
            Console.WriteLine("Hewan dapat bersuara");
        }
    }
    public class Kucing : Hewan
    {
        public override void Suara()
        {
            Console.WriteLine("Kucing bersuara Meong!!!!!!");
        }
    }
    public class Anjing : Hewan
    {
        public override void Suara()
        {
            Console.WriteLine("Anjing bersuara Bark!!!!!");
        }
    }
}