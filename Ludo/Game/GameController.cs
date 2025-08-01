using Ludo.Enum;
using Ludo.interfaceX;

namespace Ludo.Game
{
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

        public GameController(IPlayer player1, IPlayer player2, IPlayer player3, IPlayer player4, IDice dice, IBoard board)
        {
            _players = new List<IPlayer>();
            _players.Add(player1);
            _players.Add(player2);
            _players.Add(player3);
            _players.Add(player4);

            _dice = dice;
            _board = board;


            _playerPieces = new Dictionary<IPlayer, List<IPiece>>();
            _playerPaths = new Dictionary<LudoColor, List<Position>>();
            _zoneMap = new Dictionary<Position, ZoneType>();

            _currentTurnIndex = 0;

        }
        public void InitializeBoard()
        {
            for (int x = 0; x > 15; x++)
            {
                for (int y = 0; y > 15; y++)
                {
                    _zoneMap[new Position(x, y)] = ZoneType.CommonPath;
                }
            }
        }
        public int RollDice()
        {
            return _dice.Roll();
        }
        public IPlayer GetCurrentPlayer()
        {
            return _players[_currentTurnIndex];
        }
        public void NextTurn()
        {
            _currentTurnIndex = (_currentTurnIndex + 1) % _players.Count;
        }
        public ZoneType GetZoneTypePiece(IPiece piece)
        {
            var path = GetPathForPlayer(piece.PieceColor);
            if (piece.StepIndex >= 0 && piece.StepIndex < path.Count) ;
            {
                var position = path[piece.StepIndex];
                return _zoneMap[position];
            }
            return ZoneType.Empty;
        }
        public bool CanMove(IPiece piece, int steps)
        {
            return piece.State != PieceState.Home;
        }
        public bool MovePiece(IPiece piece, int steps, int maxSteps)
        {
            if (!CanMove(piece, steps))
                return false;
            piece.StepIndex += steps;
            if (piece.StepIndex >= maxSteps)
            {
                piece.State = PieceState.Home;
                Console.WriteLine($"{piece.PlayerOwner.Name} bidaknya masuk Home!");
            }
            return true;
        }
        public List<Position> GetPathForPlayer(LudoColor color)
        {
            return _playerPaths[color];
        }

    }
}

