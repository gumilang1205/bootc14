using System.Drawing;

namespace Ludo.interfaceX;

public interface IPlayer
{
    public string Name { get; set; }
    public Color Color { get; set; }
}