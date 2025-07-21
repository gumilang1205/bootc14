using System;
namespace EventHandler
{
    public delegate void TickEventHandler(object sender, EventArgs e);
    public class Clock
    {
        public event TickEventHandler Tick;
        public void StartClock()
        {
            Console.WriteLine("clock started. Waiting for 5 seconds...");
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Thick!");
                Tick?.Invoke(this, EventArgs.Empty);
            }
            Console.WriteLine("Clock Stopped.");
        }
    }
    public class DisplayClock
    {
        public void Subscribe(Clock theClock)
        {
            theClock.Tick += HandleClockTick;
            Console.WriteLine("DisplayClock subscribed to Clock's Tick Event.");
        }
        public void Unsubscribed(Clock theClock)
        {
            theClock.Tick -= HandleClockTick;
            Console.WriteLine("DisplayClock unsubcribed from Clock's Tick event.");
        }
        
    }
    private void HandleClockTick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            Console.WriteLine($"[DisplayCLock] 
            Current Time: { now.ToLongTimeString()}
            ");
        }
    class Program
    {
        static void Main(string[] args)
        {
            Clock myClock = new Clock();

        }
    }
}