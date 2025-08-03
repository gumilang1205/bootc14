using Ludo.interfaceX;

namespace Ludo.Game
{
    public class Dice : IDice
    {
        private Random _random;
        public Dice()
        {
            _random = new Random();
        }
        public int Roll()
        {
            return _random.Next(1, 7);
        }
    }
}


