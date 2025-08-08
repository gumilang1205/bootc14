using System;
using Ludo.Enum;
using Ludo.interfaceX;
using Ludo.Game;

namespace Ludo.Game
{
    public class GameController
    {
        private Dictionary<IPlayer, List<IPiece>> _playerPieces;
        private Dictionary<LudoColor, List<Position>> _playerPaths;
        private Dictionary<Position, ZoneType> _zoneMap;
        private Dictionary<LudoColor, List<Position>> _basePositions;
        private Dictionary<LudoColor, Position> _startPoints;
        private Dictionary<LudoColor, Position> _homeEntryPoints;
        private IDice _dice;
        private List<IPlayer> _players;
        private IBoard _board;
        private int _currentTurnIndex;
        public event Action<string> OnLogMessage;


        public GameController(List<IPlayer> players, IDice dice, IBoard board)
        {
            _dice = dice;
            _board = board;
            _players = players;
            _playerPieces = new Dictionary<IPlayer, List<IPiece>>();
            _playerPaths = new Dictionary<LudoColor, List<Position>>();
            _zoneMap = new Dictionary<Position, ZoneType>();
            _basePositions = new Dictionary<LudoColor, List<Position>> 
            {
                [LudoColor.Red] = new List<Position>
                    {
                        new Position(2, 2), new Position(2, 3), new Position(3,2), new Position(3,3)
                    },
                [LudoColor.Yellow] = new List<Position>
                    {
                        new Position(11, 2), new Position(11, 3), new Position(12, 2), new Position(12, 3)
                    },
                [LudoColor.Green] = new List<Position>
                    {
                        new Position(11, 11), new Position(11, 12), new Position(12, 11), new Position(12, 12)
                    },
                [LudoColor.Blue] = new List<Position>
                    {
                        new Position(2, 11), new Position(2, 12), new Position(3, 11), new Position(3, 12)
                    }
            }; 
            _startPoints = new Dictionary<LudoColor, Position> 
            {
                [LudoColor.Red] = new Position(6, 1),
                [LudoColor.Yellow] = new Position(13, 6),
                [LudoColor.Green] = new Position(8, 13),
                [LudoColor.Blue] = new Position(1, 8)
            }; 
            _homeEntryPoints = new Dictionary<LudoColor, Position> 
            {
                [LudoColor.Red] = new Position(7, 0),
                [LudoColor.Yellow] = new Position(14, 7),
                [LudoColor.Green] = new Position(7, 14),
                [LudoColor.Blue] = new Position(0, 7),
            }; 
            _currentTurnIndex = 0;
            InitializePath();
            InitializePiece();
            InitializeZones();

        }
        public void InitializePiece()
        {
            foreach (var player in _players)
            {
                var pieces = new List<IPiece>();
                for (int i = 0; i < 4; i++)
                {
                    var piece = new Piece(player, player.Color);
                    piece.State = PieceState.AtBase;
                    piece.BaseIndex = i;
                    pieces.Add(piece);
                }
                _playerPieces[player] = pieces;
            }
        }

        public void InitializePath()
        {
            var universalCommonPath = new List<Position>();
            for (int y = 1; y <= 5; y++) universalCommonPath.Add(new Position(6, y));
            for (int x = 5; x >= 0; x--) universalCommonPath.Add(new Position(x, 6));

            universalCommonPath.Add(new Position(0, 7));
            for (int x = 0; x <= 5; x++) universalCommonPath.Add(new Position(x, 8));
            for (int y = 9; y <= 14; y++) universalCommonPath.Add(new Position(6, y));

            universalCommonPath.Add(new Position(7, 14));
            for (int y = 14; y >= 9; y--) universalCommonPath.Add(new Position(8, y));
            for (int x = 9; x <= 14; x++) universalCommonPath.Add(new Position(x, 8));

            universalCommonPath.Add(new Position(14, 7));
            for (int x = 14; x >= 9; x--) universalCommonPath.Add(new Position(x, 6));
            for (int y = 5; y >= 0; y--) universalCommonPath.Add(new Position(8, y));
            universalCommonPath.Add(new Position(7, 0));
            universalCommonPath.Add(new Position(6, 0));

            if (universalCommonPath.Count != 52)
            {
                OnLogMessage?.Invoke($"WARNING: Common path has {universalCommonPath.Count} steps, expected 52!");
            }

            var redHome = new List<Position>();
            for (int y = 1; y <= 6; y++) redHome.Add(new Position(7, y));

            var yellowHome = new List<Position>();
            for (int x = 13; x >= 8; x--) yellowHome.Add(new Position(x, 7));

            var greenHome = new List<Position>();
            for (int y = 13; y >= 8; y--) greenHome.Add(new Position(7, y));

            var blueHome = new List<Position>();
            for (int x = 1; x <= 6; x++) blueHome.Add(new Position(x, 7));

            _playerPaths[LudoColor.Red] = CreatePlayerPath(universalCommonPath, LudoColor.Red, redHome);
            _playerPaths[LudoColor.Green] = CreatePlayerPath(universalCommonPath, LudoColor.Green, greenHome);
            _playerPaths[LudoColor.Yellow] = CreatePlayerPath(universalCommonPath, LudoColor.Yellow, yellowHome);
            _playerPaths[LudoColor.Blue] = CreatePlayerPath(universalCommonPath, LudoColor.Blue, blueHome);
        }
        private List<Position> CreatePlayerPath(List<Position> commonPath, LudoColor color, List<Position> homePath)
        {
            var playerSpecificPath = new List<Position>();
            Position startPoint = _startPoints[color];
            Position homeEntryPointOnCommonPath = _homeEntryPoints[color];

            int startIndex = commonPath.IndexOf(startPoint);
            if (startIndex == -1)
            {
                OnLogMessage?.Invoke($"Error: Start point {startPoint.X},{startPoint.Y} for {color} not found in common path.");
                return new List<Position>();
            }
            int stepsAdded = 0;
            for (int i = 0; i < commonPath.Count; i++)
            {
                int currentIndex = (startIndex + i) % commonPath.Count;
                Position currentPos = commonPath[currentIndex];
                playerSpecificPath.Add(currentPos);
                stepsAdded++;
                if (currentPos.Equals(homeEntryPointOnCommonPath))
                {
                    if (stepsAdded != 51)
                    {
                        OnLogMessage?.Invoke($"WARNING: Player {color} common path before home entry has {stepsAdded} steps, expected 51!");
                    }
                    break;
                }
            }

            playerSpecificPath.AddRange(homePath);

            return playerSpecificPath;
        }
        public void InitializeZones()
        {
            _zoneMap.Clear();
            for (int y = 0; y < 15; y++)
            {
                for (int x = 0; x < 15; x++)
                {
                    Position p = new Position(x, y);
                    _zoneMap[p] = _board.GetZoneType(x, y);
                }
            }
        }
        public ZoneType GetZoneType(int x, int y)
        {
            if (_zoneMap.TryGetValue(new Position(x, y), out var zone))
                return zone;
            return ZoneType.Empty;
        }
        public bool IsSafeZone(int x, int y)
        {
            var zone = GetZoneType(x, y);
            return zone == ZoneType.SafeZone || zone == ZoneType.StartPoint || zone == ZoneType.HomePath; 
        }
        public bool IsBlocked(int x, int y)
        {
            return GetZoneType(x, y) == ZoneType.BlockedPath;
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
        public bool CanMove(IPiece piece, int roll)
        {
            if (piece.State == PieceState.AtBase)
            {
                return roll == 6;
            }
            var path = GetPathForPlayer(piece.PieceColor);
            return piece.StepIndex + roll < path.Count;

        }
        public bool MovePiece(IPiece piece, int roll)
        {
            if (piece.State == PieceState.AtBase)
            {
                if (roll == 6)
                {
                    MovePieceFromBase(piece);
                    return true;
                }
                return false;
            }

            int newStepIndex = piece.StepIndex + roll;
            var path = GetPathForPlayer(piece.PieceColor);

            if (newStepIndex == path.Count - 1)
            {
                piece.StepIndex = newStepIndex;
                piece.State = PieceState.Home;
                OnLogMessage?.Invoke($"Bidak {ColorToString(piece.PieceColor)} mendarat di Home Point!"); 
                return true;
            }
            else if (newStepIndex >= path.Count)
            {
                OnLogMessage?.Invoke($"Langkah {roll} terlalu banyak. Bidak tidak bisa digerakkan.");
                return false;
            }
            else
            {
                piece.StepIndex = newStepIndex;
                OnLogMessage?.Invoke($"Bidak {ColorToString(piece.PieceColor)} pindah ke langkah {piece.StepIndex + 1}.");
                return true;
            }
        }

        public void MovePieceFromBase(IPiece piece)
        {
            var startPoint = _startPoints[piece.PieceColor];
            var path = GetPathForPlayer(piece.PieceColor);

            int startIndex = path.IndexOf(startPoint);
            if (startIndex == -1)
            {
                OnLogMessage?.Invoke("Error: Start point tidak ditemukan di jalur pemain.");
                return;
            }

            piece.State = PieceState.Active;
            piece.StepIndex = startIndex;
            piece.BaseIndex = -1;
        }
        public bool CheckWin(IPlayer player)
        {
            return _playerPieces[player].All(p => p.State == PieceState.Home);
        }
        public bool CaptureIfExists(IPlayer currentPlayer, Position currentPosition)
        {
            bool captured = false;
            ZoneType zoneType = _board.GetZoneType(currentPosition.X, currentPosition.Y);
            if (zoneType == ZoneType.SafeZone)
            {
                return false;
            }

            foreach (var otherPlayer in _players)
            {
                if (otherPlayer != currentPlayer)
                {
                    foreach (var otherPiece in _playerPieces[otherPlayer])
                    {
                        if (otherPiece.State == PieceState.Active)
                        {
                            var otherPath = GetPathForPlayer(otherPiece.PieceColor);
                            if (otherPiece.StepIndex >= 0 && otherPiece.StepIndex < otherPath.Count)
                            {
                                if (otherPath[otherPiece.StepIndex].Equals(currentPosition))
                                {
                                    ReturnPieceToBase(otherPiece);
                                    OnLogMessage?.Invoke($"Bidak {ColorToString(otherPiece.PieceColor)} milik {otherPlayer.Name} kembali ke base!");
                                    captured = true;
                                }
                            }
                        }
                    }
                }
            }
            return captured;
        }
        private void ReturnPieceToBase(IPiece piece)
        {
            piece.State = PieceState.AtBase;
            piece.StepIndex = -1;
            for (int i = 0; i < 4; i++)
            {
                if (!_playerPieces[piece.PlayerOwner].Any(p => p.BaseIndex == i))
                {
                    piece.BaseIndex = i;
                    break;
                }
            }
        }
        public string ColorToString(LudoColor color)
        {
            return color.ToString();
        }

        public char GetPieceChar(LudoColor color)
        {
            return color.ToString()[0];
        }
        public List<Position> GetPathForPlayer(LudoColor color)
        {
            if (_playerPaths.TryGetValue(color, out var path))
            {
                return path;
            }

            return new List<Position>();
        }

        public Dictionary<IPlayer, List<IPiece>> GetPlayerPieces
        {
            get { return _playerPieces; }
        }

        public IBoard GetBoard()
        {
            return _board;
        }

        public Dictionary<LudoColor, List<Position>> GetBasePositions
        {
            get { return _basePositions; }
        }

    }


}

