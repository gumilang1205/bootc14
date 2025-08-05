using Ludo.Enum;

namespace Ludo.interfaceX
{
    public interface IBoard
    {
        int[,] Grid { get; }
        ZoneType GetZoneType(int x, int y);
    }
}

