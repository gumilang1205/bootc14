using Ludo.Enum;
using Ludo.interfaceX;

namespace Ludo.Game;

public class GameController
{
    private Dictionary<IPlayer, List<IPiece>> _playerPieces;
    private Dictionary<Color, List<Position>> _playerPaths;
    private Dictionary<Position, ZoneType> _zoneMap;
    private IDice _dice;
    private List<IPlayer> _players;
    private IBoard _board;
    private int _currentTurnIndex;
    public event Action OnGameStart;

    public GameController()
    {

    }
}