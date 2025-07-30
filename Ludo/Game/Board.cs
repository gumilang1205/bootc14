using System.Runtime.CompilerServices;
using Ludo.interfaceX;

namespace Ludo.Game;

public class Board : IBoard
{
    public int[,] Grid { get; } = new int[15, 15];
    public Board()
    {

    }
}
