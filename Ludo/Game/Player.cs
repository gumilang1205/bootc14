
using Ludo.Enum;

namespace Ludo.interfaceX
{
    public class Player : IPlayer
    {
        public string Name { get; set; }
        public LudoColor Color { get; set; }
        public Player(string name, LudoColor color)
        {
            Name = name;
            Color = color;
        }
    }

}

