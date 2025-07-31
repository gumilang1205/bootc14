using Ludo.Enum;
using Ludo.interfaceX;

namespace Ludo.Game;

public class GameController
{
    private Dictionary<IPlayer, List<IPiece>> _playerPieces;
    private Dictionary<LudoColor, List<Position>> _playerPaths;
    private Dictionary<Position, ZoneType> _zoneMap;
    private IDice _dice;
    private List<IPlayer> _players;
    private IBoard _board;
    private int _currentTurnIndex;
    public event Action OnGameStart;

    public GameController(IPlayer p1, IPlayer p2, IPlayer p3, IPlayer p4)
    {
        _players.Add(p1);
        _players.Add(p2);
        _players.Add(p3);
        _players.Add(p4);
    }
}