using System.Drawing;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;

namespace Ludo.interfaceX;

public class Player : IPlayer
{
    public string Name { get; set; }
    public Color Color { get; set; }
    public Player(string name, Color color)
    {
        Name = name;
        Color = color;
    }
}
