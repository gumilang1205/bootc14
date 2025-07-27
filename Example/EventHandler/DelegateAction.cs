namespace Example.EventHandler
{
    public class Tombol
    {
        public event Action? Press;
        public void Tekan()
        {
            Press?.Invoke();
        }
    }

    public class Lampu
    {
        public void LampuMobil()
        {
            Console.WriteLine("Lampu Mobil Menyala");
        }
        public void LampuMotor()
        {
            Console.WriteLine("Lampu Motor Menyala");
        }
    }
}