using System.Data;
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

            InitializeZones();

        }
        public void InitializePiece()
        {
            //untuk setiap pemain di _players
            foreach (var player in _players)
            {
                //buat list bidak untuk pemain
                var pieces = new List<IPiece>();
                // setiap pemain memiliki 4 bidak
                for (int i = 0; i < 4; i++)
                {
                    var piece = new Piece(player, player.Color);
                    piece.State = PieceState.AtBase; //posisi awal di Base
                    piece.StepIndex = 0;
                    pieces.Add(piece);
                }
                //masukan list bidak ke dictionary _playerPieces
                _playerPieces[player] = pieces;

            }
        }

        public void InitializePath()
        {
            //CommonPath = 52 langkah keliling papan
            var commonPath = new List<Position>();
            for (int i = 0; i < 52; i++)
            {
                commonPath.Add(new Position(i % 13, i / 13)); // simulasi koordinat
            }

            // HomePath = 6 langkah khusus untuk tiap warna
            var redHome = new List<Position>();
            var greenHome = new List<Position>();
            var yellowHome = new List<Position>();
            var blueHome = new List<Position>();

            for (int i = 0; i < 6; i++)
            {
                redHome.Add(new Position(6 + i, 7));
                greenHome.Add(new Position(7, 6 - i));
                yellowHome.Add(new Position(8 - i, 7));
                blueHome.Add(new Position(7, 8 + i));
            }

            // Gabungkan CommonPath + HomePath untuk tiap warna
            _playerPaths[LudoColor.Red] = new List<Position>(commonPath.Concat(redHome));
            _playerPaths[LudoColor.Green] = new List<Position>(commonPath.Concat(greenHome));
            _playerPaths[LudoColor.Yellow] = new List<Position>(commonPath.Concat(yellowHome));
            _playerPaths[LudoColor.Blue] = new List<Position>(commonPath.Concat(blueHome));


        }
        public void InitializeZones()
        {
            // === BASE ZONES ===
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
                    _zoneMap[new Position(i, j)] = ZoneType.Base; // Red Base

                for (int j = 9; j <= 14; j++)
                    _zoneMap[new Position(i, j)] = ZoneType.Base; // Green Base
            }

            for (int i = 9; i <= 14; i++)
            {
                for (int j = 0; j <= 5; j++)
                    _zoneMap[new Position(i, j)] = ZoneType.Base;    // Blue Base

                for (int j = 9; j <= 14; j++)
                    _zoneMap[new Position(i, j)] = ZoneType.Base; // Yellow Base
            }

            _zoneMap[new Position(6, 1)] = ZoneType.StartPoint;
            _zoneMap[new Position(1, 8)] = ZoneType.StartPoint;
            _zoneMap[new Position(8, 13)] = ZoneType.StartPoint;
            _zoneMap[new Position(13, 6)] = ZoneType.StartPoint;

            // === HOME POINTS ===
            _zoneMap[new Position(7, 6)] = ZoneType.HomePoint;
            _zoneMap[new Position(6, 7)] = ZoneType.HomePoint;
            _zoneMap[new Position(7, 8)] = ZoneType.HomePoint;
            _zoneMap[new Position(8, 7)] = ZoneType.HomePoint;

            // === BLOCKED PATH ===
            _zoneMap[new Position(7, 7)] = ZoneType.BlockedPath;
            _zoneMap[new Position(6, 6)] = ZoneType.BlockedPath;
            _zoneMap[new Position(6, 8)] = ZoneType.BlockedPath;
            _zoneMap[new Position(8, 6)] = ZoneType.BlockedPath;
            _zoneMap[new Position(8, 8)] = ZoneType.BlockedPath;

            // === SAFE ZONE (ekspisit) ===
            _zoneMap[new Position(8, 2)] = ZoneType.SafeZone;
            _zoneMap[new Position(2, 6)] = ZoneType.SafeZone;
            _zoneMap[new Position(6, 12)] = ZoneType.SafeZone;
            _zoneMap[new Position(12, 8)] = ZoneType.SafeZone;

            // === HOME PATH ===
            for (int j = 1; j <= 5; j++) _zoneMap[new Position(7, j)] = ZoneType.HomePath;
            for (int i = 1; i <= 5; i++) _zoneMap[new Position(i, 7)] = ZoneType.HomePath;
            for (int j = 9; j <= 13; j++) _zoneMap[new Position(7, j)] = ZoneType.HomePath;
            for (int i = 9; i <= 13; i++) _zoneMap[new Position(i, 7)] = ZoneType.HomePath;
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


        public void StartGame()
        {
            OnGameStart?.Invoke();
            Console.WriteLine("Game dimulai!");
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
        public bool MovePiece(IPiece piece, int steps)
        {
            if (!CanMove(piece, steps))
                return false;
            var path = GetPathForPlayer(piece.PieceColor);
            piece.StepIndex += steps;
            //Cek apakah sudah sampai HomePoint
            if (piece.StepIndex >= path.Count - 1)
            {
                piece.StepIndex = path.Count - 1;
                piece.State = PieceState.Home;
                Console.WriteLine($"{piece.PlayerOwner.Name} bidaknya masuk Home!");
            }
            else if (piece.StepIndex >= 52 && piece.State == PieceState.Active)
            {
                Console.WriteLine($"{piece.PlayerOwner.Name} bidaknya memasuki HomePath");
            }
            return true;
        }
        public bool CheckWin(IPlayer player)
        {
            return _playerPieces[player].All(p => p.State == PieceState.Home);
        }
        public void CaptureIfExists()
        {
            // Ambil pemain yang sedang jalan
            var currentPlayer = GetCurrentPlayer();
            var currentPieces = _playerPieces[currentPlayer];

            // Loop semua bidak pemain sekarang
            foreach (var myPiece in currentPieces)
            {
                // Jika bidak belum aktif atau sudah di Home, lewati
                if (myPiece.State == PieceState.AtBase || myPiece.State == PieceState.Home)
                    continue;

                // Dapatkan posisi jalur bidak saat ini
                var myPath = GetPathForPlayer(myPiece.PieceColor);
                var myPos = myPath[myPiece.StepIndex];

                // Cek apakah posisi ini termasuk SafeZone atau HomePath (tidak bisa tangkap)
                if (IsSafeZone(myPos.X, myPos.Y))
                    continue;

                // Loop semua pemain lawan
                foreach (var opponent in _players)
                {
                    if (opponent == currentPlayer) continue; // skip diri sendiri

                    var opponentPieces = _playerPieces[opponent];

                    foreach (var enemyPiece in opponentPieces)
                    {
                        // Jika bidak lawan juga aktif
                        if (enemyPiece.State == PieceState.Active)
                        {
                            var enemyPath = GetPathForPlayer(enemyPiece.PieceColor);
                            var enemyPos = enemyPath[enemyPiece.StepIndex];

                            // Jika posisi sama -> tangkap
                            if (enemyPos.Equals(myPos))
                            {
                                // Reset bidak lawan ke Base
                                enemyPiece.State = PieceState.AtBase;
                                enemyPiece.StepIndex = 0;

                                Console.WriteLine(
                                    $"{currentPlayer.Name} menangkap bidak {opponent.Name}! Bidak lawan kembali ke Base."
                                );
                            }
                        }
                    }
                }
            }
        }
        public bool CanEnterFromBase(IPiece piece, int roll)
        {
            return roll == 6 && piece.State == PieceState.AtBase;
        }
        public List<Position> GetPathForPlayer(LudoColor color)
        {
            return _playerPaths[color];
        }
        public int GetPiecePathIndex(IPiece piece)
        {
            return piece.StepIndex;
        }

    }
}

