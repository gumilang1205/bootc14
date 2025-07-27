namespace Example.InterfaceX
{
    public interface IBayar
    {
        public void Bayar();
        public void Nama(string name);
    }
    public class KartuDebit : IBayar
    {
        public void Bayar()
        {
            Console.WriteLine("Bayar");
        }

        public void Nama(string name)
        {
            Console.WriteLine($"Nama : {name}");
        }
    }
}
