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
        private Dictionary<LudoColor, List<Position>> _basePositions;
        private Dictionary<LudoColor, Position> _startPoints;
        private Dictionary<LudoColor, Position> _homeEntryPoints;
        private IDice _dice;
        private List<IPlayer> _players;
        private IBoard _board;
        private int _currentTurnIndex;
        public event Action OnGameStart;

        public GameController(IPlayer player1, IPlayer player2, IPlayer player3, IPlayer player4, IDice dice, IBoard board)
        {
            _players = new List<IPlayer> { player1, player2, player3, player4 };
            _dice = dice;
            _board = board;

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
                [LudoColor.Red] = new Position(14, 7),
                [LudoColor.Red] = new Position(7, 14),
                [LudoColor.Red] = new Position(0, 7),
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
            //CommonPath = 52 langkah keliling papan
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
                Console.WriteLine($"WARNING: Common path has {universalCommonPath.Count} steps, expected 52!");
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
            _playerPaths[LudoColor.Green] = CreatePlayerPath(universalCommonPath, LudoColor.Red, greenHome);
            _playerPaths[LudoColor.Yellow] = CreatePlayerPath(universalCommonPath, LudoColor.Red, yellowHome);
            _playerPaths[LudoColor.Blue] = CreatePlayerPath(universalCommonPath, LudoColor.Red, blueHome);
        }
        private List<Position> CreatePlayerPath(List<Position> commonPath, LudoColor color, List<Position> homePath)
        {
            var playerSpecificPath = new List<Position>();
            Position startPoint = _startPoints[color];
            Position homeEntryPointOnCommonPath = _homeEntryPoints[color];

            int startIndex = commonPath.IndexOf(startPoint);
            if (startIndex == -1)
            {
                Console.WriteLine($"Error: Start point {startPoint.X},{startPoint.Y} for {color} not found in common path.");
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
                        Console.WriteLine($"WARNING: Player {color} common path before home entry has {stepsAdded} steps, expected 51!");
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
        public void DrawBoard()
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("             Papan Ludo                 ");
            Console.WriteLine("------------------------------------------");

            char[,] display = new char[15, 15];

            // Inisialisasi papan dengan karakter default atau berdasarkan zona dari _board
            for (int y = 0; y < 15; y++)
            {
                for (int x = 0; x < 15; x++)
                {
                    // Ambil tipe zona dari objek _board yang diinjeksi
                    ZoneType zone = _board.GetZoneType(x, y);
                    switch (zone)
                    {
                        case ZoneType.Base: display[x, y] = '#'; break;
                        case ZoneType.StartPoint: display[x, y] = 'S'; break;
                        case ZoneType.SafeZone: display[x, y] = '*'; break;
                        case ZoneType.HomePath: display[x, y] = '='; break;
                        case ZoneType.HomePoint: display[x, y] = 'H'; break;
                        case ZoneType.CommonPath: display[x, y] = '-'; break;
                        default: display[x, y] = 'X'; break; // Kosong jika tidak ada zona atau bidak
                    }
                }
            }

            // Tampilkan bidak di jalur dan di base
            foreach (var kvp in _playerPieces)
            {
                var pieces = kvp.Value;

                foreach (var piece in pieces)
                {
                    if (piece.State == PieceState.Active)
                    {
                        var path = GetPathForPlayer(piece.PieceColor);
                        if (piece.StepIndex >= 0 && piece.StepIndex < path.Count)
                        {
                            var pos = path[piece.StepIndex];
                            // Prioritaskan bidak daripada simbol zona
                            display[pos.X, pos.Y] = GetPieceChar(piece.PieceColor);
                        }
                    }
                    else if (piece.State == PieceState.AtBase)
                    {
                        if (piece.BaseIndex != -1 && piece.BaseIndex < _basePositions[piece.PieceColor].Count)
                        {
                            var pos = _basePositions[piece.PieceColor][piece.BaseIndex];
                            display[pos.X, pos.Y] = GetPieceChar(piece.PieceColor);
                        }
                    }
                    // Bidak di HomeState tidak perlu ditampilkan di papan utama secara individual
                }
            }

            // Cetak grid dengan warna
            for (int y = 0; y < 15; y++) // Iterasi Y (baris)
            {
                for (int x = 0; x < 15; x++) // Iterasi X (kolom)
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    char charToDisplay = display[x, y]; // Ambil karakter dari display array

                    // Atur warna berdasarkan warna bidak
                    if (charToDisplay == 'R') Console.ForegroundColor = ConsoleColor.Red;
                    else if (charToDisplay == 'Y') Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (charToDisplay == 'G') Console.ForegroundColor = ConsoleColor.Green;
                    else if (charToDisplay == 'B') Console.ForegroundColor = ConsoleColor.Blue;
                    else
                    {
                        // Atur warna berdasarkan zona jika bukan bidak
                        // Sekarang ambil dari _board juga untuk konsistensi
                        ZoneType zone = _board.GetZoneType(x, y);
                        switch (zone)
                        {
                            case ZoneType.Base: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                            case ZoneType.StartPoint: Console.ForegroundColor = ConsoleColor.DarkBlue; break;
                            case ZoneType.SafeZone: Console.ForegroundColor = ConsoleColor.Cyan; break;
                            case ZoneType.HomePath: Console.ForegroundColor = ConsoleColor.Magenta; break;
                            case ZoneType.HomePoint: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                            case ZoneType.CommonPath: Console.ForegroundColor = ConsoleColor.Gray; break;
                            default: Console.ForegroundColor = ConsoleColor.White; break;
                        }
                    }
                    Console.Write(charToDisplay + " ");
                    Console.ForegroundColor = originalColor;
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nKeterangan:");
            Console.ForegroundColor = ConsoleColor.Red; Console.Write("R ");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("Y ");
            Console.ForegroundColor = ConsoleColor.Green; Console.Write("G ");
            Console.ForegroundColor = ConsoleColor.Blue; Console.Write("B ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Bidak pemain");
            Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("# ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Base");
            Console.ForegroundColor = ConsoleColor.DarkCyan; Console.Write("S ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Start Point");
            Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("* ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Safe Zone");
            Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("= ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Home Path");
            Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write("H ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Home Point (Tujuan)");
            Console.ForegroundColor = ConsoleColor.Gray; Console.Write("- ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Common Path");
        }

        public bool IsBlocked(int x, int y)
        {
            return GetZoneType(x, y) == ZoneType.BlockedPath;
        }

        public void StartGame()
        {
            OnGameStart?.Invoke();

            bool gameOver = false;
            while (!gameOver)
            {
                Console.Clear();
                DrawBoard();

                var currentPlayer = GetCurrentPlayer();
                Console.WriteLine($"\nGiliran: {currentPlayer.Name} ({ColorToString(currentPlayer.Color)})");

                Console.WriteLine("Tekan ENTER untuk melempar dadu...");
                Console.ReadLine();
                int roll = RollDice();
                Console.WriteLine($"Kamu melempar: {roll}");

                bool movedPiece = false;
                bool gotBonusTurn = false;

                var pieces = _playerPieces[currentPlayer];
                var activePieces = pieces.Where(p => p.State == PieceState.Active).ToList();
                var atBasePieces = pieces.Where(p => p.State == PieceState.AtBase).ToList();

                List<IPiece> movablePieces = new List<IPiece>();

                if (roll == 6 && atBasePieces.Any())
                {
                    Console.WriteLine("Pilihan pergerakan:");
                    Console.WriteLine($"1. Keluarkan bidak dari Base (Bidak {ColorToString(atBasePieces.First().PieceColor)} pertama)");

                    int optionCounter = 2;
                    foreach (var piece in activePieces)
                    {
                        if (CanMove(piece, roll))
                        {
                            movablePieces.Add(piece);
                            Console.WriteLine($"{optionCounter}. Pindahkan bidak {ColorToString(piece.PieceColor)} di langkah {piece.StepIndex + 1}");
                            optionCounter++;
                        }
                    }

                    int choice = -1;
                    while (choice == -1)
                    {
                        Console.Write("Masukkan nomor pilihan: ");
                        if (int.TryParse(Console.ReadLine(), out int input))
                        {
                            if (input == 1)
                            {
                                var chosenPiece = atBasePieces.First();
                                MovePieceFromBase(chosenPiece);
                                Console.WriteLine($"Bidak {ColorToString(chosenPiece.PieceColor)} keluar dari base ke titik start.");
                                movedPiece = true;
                                gotBonusTurn = true;
                                break;
                            }
                            else if (input > 1 && input <= movablePieces.Count + 1)
                            {
                                var chosenPiece = movablePieces[input - 2];
                                if (MovePiece(chosenPiece, roll))
                                {
                                    movedPiece = true;
                                    if (chosenPiece.State == PieceState.Active)
                                    {
                                        var currentPos = GetPathForPlayer(chosenPiece.PieceColor)[chosenPiece.StepIndex];
                                        gotBonusTurn = CaptureIfExists(currentPlayer, currentPos);
                                    }
                                }
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Masukan tidak valid. Harap masukkan angka.");
                        }
                    }
                }
                else // Jika dadu bukan 6 atau tidak ada bidak di base
                {
                    movablePieces.AddRange(activePieces.Where(p => CanMove(p, roll)));

                    if (!movablePieces.Any())
                    {
                        Console.WriteLine("Tidak ada bidak yang bisa digerakkan. Giliran dilewati.");
                        NextTurn();
                        Console.WriteLine("\nTekan ENTER untuk lanjut...");
                        Console.ReadLine();
                        continue;
                    }

                    Console.WriteLine("Pilih bidak yang akan digerakkan:");
                    for (int i = 0; i < movablePieces.Count; i++)
                    {
                        string pieceStatus = movablePieces[i].State == PieceState.AtBase ? "di Base" : $"di langkah {movablePieces[i].StepIndex + 1}";
                        Console.WriteLine($"{i + 1}. Bidak {ColorToString(movablePieces[i].PieceColor)} {pieceStatus}");
                    }

                    int choiceIndex = -1;
                    while (choiceIndex == -1)
                    {
                        Console.Write("Masukkan nomor bidak: ");
                        if (int.TryParse(Console.ReadLine(), out int input) && input >= 1 && input <= movablePieces.Count)
                        {
                            choiceIndex = input - 1;
                        }
                        else
                        {
                            Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.");
                        }
                    }

                    var chosenPiece = movablePieces[choiceIndex];

                    if (MovePiece(chosenPiece, roll))
                    {
                        movedPiece = true;
                        if (chosenPiece.State == PieceState.Active)
                        {
                            var currentPos = GetPathForPlayer(chosenPiece.PieceColor)[chosenPiece.StepIndex];
                            gotBonusTurn = CaptureIfExists(currentPlayer, currentPos);
                        }
                    }
                }


                // Cek kemenangan
                if (CheckWin(currentPlayer))
                {
                    Console.Clear();
                    DrawBoard();
                    Console.WriteLine($"\n=== SELAMAT! {currentPlayer.Name} MENANG!!! ===");
                    gameOver = true;
                    break;
                }

                // Aturan bonus giliran
                if (roll == 6 || gotBonusTurn)
                {
                    Console.WriteLine("Kamu dapat bonus giliran!");
                }
                else
                {
                    NextTurn();
                }

                Console.WriteLine("\nTekan ENTER untuk lanjut...");
                Console.ReadLine();
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
        public bool CanMove(IPiece piece, int roll)
        {
            if (piece.State == PieceState.AtBase)
            {
                return roll == 6;
            }
            var path = GetPathForPlayer(piece.PieceColor);
            return piece.StepIndex + roll < path.Count;

        }
        private bool MovePiece(IPiece piece, int roll)
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
                Console.WriteLine($"Bidak {ColorToString(piece.PieceColor)} mendarat di Home Point!");
                return true;
            }
            else if (newStepIndex >= path.Count)
            {
                Console.WriteLine($"Langkah {roll} terlalu banyak. Bidak tidak bisa digerakkan.");
                return false;
            }
            else
            {
                piece.StepIndex = newStepIndex;
                Console.WriteLine($"Bidak {ColorToString(piece.PieceColor)} pindah ke langkah {piece.StepIndex + 1}.");
                return true;
            }
        }

        private void MovePieceFromBase(IPiece piece)
        {
            var startPoint = _startPoints[piece.PieceColor];
            var path = GetPathForPlayer(piece.PieceColor);

            int startIndex = path.IndexOf(startPoint);
            if (startIndex == -1)
            {
                Console.WriteLine("Error: Start point tidak ditemukan di jalur pemain.");
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
            // Hanya cek jika posisi tidak berada di Safe Zone
            ZoneType zoneType = _board.GetZoneType(currentPosition.X, currentPosition.Y);
            if (zoneType == ZoneType.SafeZone)
            {
                return false; // Tidak bisa menangkap di safe zone
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
                                    // Kumpulkan bidak lawan kembali ke base
                                    ReturnPieceToBase(otherPiece);
                                    Console.WriteLine($"Bidak {ColorToString(otherPiece.PieceColor)} milik {otherPlayer.Name} kembali ke base!");
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
            // Cari slot base yang kosong
            for (int i = 0; i < 4; i++)
            {
                if (!_playerPieces[piece.PlayerOwner].Any(p => p.BaseIndex == i))
                {
                    piece.BaseIndex = i;
                    break;
                }
            }
        }
        public bool CanEnterFromBase(IPiece piece, int roll)
        {
            return roll == 6 && piece.State == PieceState.AtBase;
        }
        private string ColorToString(LudoColor color)
        {
            return color.ToString();
        }

        private char GetPieceChar(LudoColor color)
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
        public int GetPiecePathIndex(IPiece piece)
        {
            return piece.StepIndex;
        }

    }
}

