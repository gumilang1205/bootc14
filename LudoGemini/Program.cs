using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Inisialisasi pemain
        Player player1 = new Player("Andi", LudoColor.Red);
        Player player2 = new Player("Bobi", LudoColor.Yellow);
        Player player3 = new Player("Cica", LudoColor.Green);
        Player player4 = new Player("Duda", LudoColor.Blue);

        // Inisialisasi dadu dan papan
        IDice dice = new Dice();
        IBoard board = new Board();

        // Inisialisasi game controller
        GameController controller = new GameController(player1, player2, player3, player4, dice, board);

        // Event handler untuk game start
        controller.OnGameStart += () => Console.WriteLine("Selamat datang di permainan Ludo!");

        // Memulai permainan
        controller.StartGame();
    }
}

// --- Kelas Model ---

public class Board : IBoard
{
    // Grid tidak lagi perlu menyimpan int[,] secara langsung, cukup sebagai representasi visual
    // Logika Board akan dipegang oleh GameController
    public int[,] Grid { get; } = new int[15, 15]; // Ukuran standar papan Ludo 15x15

    public Board()
    {
        // Konstruktor kosong karena detail grid ditangani di GameController
    }
}

public class Dice : IDice
{
    private Random _random;

    public Dice()
    {
        _random = new Random();
    }

    // Mengembalikan angka acak antara 1 dan 6
    public int Roll()
    {
        return _random.Next(1, 7);
    }
}

public class Piece : IPiece
{
    public LudoColor PieceColor { get; }
    public IPlayer PlayerOwner { get; }
    public PieceState State { get; set; }
    public int StepIndex { get; set; } // Indeks di jalur
    public int BaseIndex { get; set; } // Indeks di base (0-3)

    public Piece(IPlayer ownerPlayer, LudoColor pieceColor)
    {
        PieceColor = pieceColor;
        PlayerOwner = ownerPlayer;
        State = PieceState.AtBase;
        StepIndex = -1; // -1 menunjukkan belum di jalur utama
        BaseIndex = -1; // Akan diatur saat inisialisasi bidak di base
    }
}

public class Player : IPlayer
{
    public string Name { get; set; }
    public LudoColor Color { get; set; }
    public Player(string name, LudoColor color)
    {
        Name = name;
        Color = color;
    }
}

public record struct Position
{
    public int X { get; set; }
    public int Y { get; set; }
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

// --- Antarmuka (Interfaces) ---

public interface IBoard
{
    int[,] Grid { get; }
}

public interface IDice
{
    int Roll();
}

public interface IPiece
{
    LudoColor PieceColor { get; }
    IPlayer PlayerOwner { get; }
    PieceState State { get; set; }
    int StepIndex { get; set; }
    int BaseIndex { get; set; }
}

public interface IPlayer
{
    string Name { get; set; }
    LudoColor Color { get; set; }
}

// --- Enums ---

public enum PieceState
{
    AtBase,
    Active,
    Home
}

public enum LudoColor
{
    Red,
    Yellow,
    Green,
    Blue
}

public enum ZoneType
{
    Base,
    StartPoint,
    CommonPath,
    HomePath,
    HomePoint,
    SafeZone,
    BlockedPath, // Tidak banyak digunakan dalam ludo standar, bisa dihapus atau diabaikan
    Empty
}

// --- Kelas GameController ---

public class GameController
{
    private Dictionary<IPlayer, List<IPiece>> _playerPieces;
    private Dictionary<LudoColor, List<Position>> _playerPaths;
    private Dictionary<Position, ZoneType> _zoneMap;
    private Dictionary<LudoColor, List<Position>> _basePositions;
    private Dictionary<LudoColor, Position> _startPoints;
    private IDice _dice;
    private List<IPlayer> _players;
    private IBoard _board; // Tidak terlalu banyak dipakai selain untuk representasi grid
    private int _currentTurnIndex;
    private int _consecutiveSixes; // Untuk aturan 3x dadu 6

    public event Action OnGameStart;

    public GameController(IPlayer player1, IPlayer player2, IPlayer player3, IPlayer player4, IDice dice, IBoard board)
    {
        _players = new List<IPlayer> { player1, player2, player3, player4 };
        _dice = dice;
        _board = board;

        _playerPieces = new Dictionary<IPlayer, List<IPiece>>();
        _playerPaths = new Dictionary<LudoColor, List<Position>>();
        _zoneMap = new Dictionary<Position, ZoneType>();

        // Posisi base untuk setiap warna
        _basePositions = new Dictionary<LudoColor, List<Position>>
        {
            [LudoColor.Red] = new List<Position>
            {
                new Position(1, 1), new Position(1, 2), new Position(2, 1), new Position(2, 2)
            },
            [LudoColor.Yellow] = new List<Position>
            {
                new Position(12, 1), new Position(12, 2), new Position(13, 1), new Position(13, 2)
            },
            [LudoColor.Green] = new List<Position>
            {
                new Position(12, 12), new Position(12, 13), new Position(13, 12), new Position(13, 13)
            },
            [LudoColor.Blue] = new List<Position>
            {
                new Position(1, 12), new Position(1, 13), new Position(2, 12), new Position(2, 13)
            }
        };

        // Titik start untuk setiap warna
        _startPoints = new Dictionary<LudoColor, Position>
        {
            [LudoColor.Red] = new Position(6, 1),
            [LudoColor.Yellow] = new Position(13, 6),
            [LudoColor.Green] = new Position(8, 13),
            [LudoColor.Blue] = new Position(1, 8)
        };

        _currentTurnIndex = 0;
        _consecutiveSixes = 0;

        InitializePieces();
        InitializePaths();
        InitializeZones();
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

            // Aturan 3x dadu 6 berturut-turut
            if (roll == 6)
            {
                _consecutiveSixes++;
                if (_consecutiveSixes == 3)
                {
                    Console.WriteLine("Dapat 6 tiga kali berturut-turut! Bidak terakhirmu kembali ke base!");
                    var lastMovedPiece = GetLastMovedPiece(currentPlayer); // Implementasikan ini jika perlu melacak bidak terakhir
                    if (lastMovedPiece != null && lastMovedPiece.State == PieceState.Active)
                    {
                        ReturnPieceToBase(lastMovedPiece);
                    }
                    _consecutiveSixes = 0; // Reset
                    NextTurn();
                    Console.WriteLine("\nTekan ENTER untuk lanjut...");
                    Console.ReadLine();
                    continue; // Lewati giliran
                }
            }
            else
            {
                _consecutiveSixes = 0; // Reset jika tidak 6
            }

            var pieces = _playerPieces[currentPlayer];
            var activePieces = pieces.Where(p => p.State == PieceState.Active).ToList();
            var atBasePieces = pieces.Where(p => p.State == PieceState.AtBase).ToList();

            // Pilihan untuk pemain
            List<IPiece> movablePieces = new List<IPiece>();
            if (roll == 6 && atBasePieces.Any())
            {
                // Prioritaskan mengeluarkan bidak dari base jika ada dan dadu 6
                movablePieces.Add(atBasePieces.First()); // Anggap saja yang pertama bisa dikeluarkan
                Console.WriteLine("Kamu dapat 6! Bidak dari Base bisa keluar (Pilihan 1).");
            }
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
                string pieceStatus = movablePieces[i].State == PieceState.AtBase ? "di Base" : $"di langkah {movablePieces[i].StepIndex}";
                Console.WriteLine($"{i + 1}. Bidak {movablePieces[i].PieceColor} {pieceStatus}");
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

            // Pindahkan bidak
            if (chosenPiece.State == PieceState.AtBase)
            {
                MovePieceFromBase(chosenPiece);
                Console.WriteLine($"Bidak {chosenPiece.PieceColor} keluar dari base.");
                movedPiece = true;
            }
            else
            {
                // Simpan posisi lama untuk pengecekan capture
                int oldStepIndex = chosenPiece.StepIndex;
                if (MovePiece(chosenPiece, roll))
                {
                    movedPiece = true;
                    // Cek capture hanya jika bidak bergerak
                    if (chosenPiece.State == PieceState.Active) // Pastikan bidak masih aktif (belum masuk home)
                    {
                        var currentPos = GetPathForPlayer(chosenPiece.PieceColor)[chosenPiece.StepIndex];
                        gotBonusTurn = CheckAndCaptureOpponentPieces(currentPlayer, currentPos);
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

    // Menginisialisasi 4 bidak untuk setiap pemain dan menempatkannya di base
    public void InitializePieces()
    {
        foreach (var player in _players)
        {
            var pieces = new List<IPiece>();
            for (int i = 0; i < 4; i++)
            {
                var piece = new Piece(player, player.Color);
                piece.State = PieceState.AtBase;
                piece.BaseIndex = i; // Menetapkan indeks base
                pieces.Add(piece);
            }
            _playerPieces[player] = pieces;
        }
    }

    // Menginisialisasi jalur untuk setiap warna
    public void InitializePaths()
    {
        // Jalur umum Ludo (52 langkah)
        var commonPath = new List<Position>();

        // Bagian atas (kiri ke kanan)
        for (int x = 6; x <= 13; x++) commonPath.Add(new Position(x, 0));
        for (int y = 1; y <= 5; y++) commonPath.Add(new Position(14, y));
        // Bagian kanan (atas ke bawah)
        for (int x = 13; x >= 9; x--) commonPath.Add(new Position(x, 6));
        for (int x = 8; x >= 0; x--) commonPath.Add(new Position(x, 7)); // Jalur tengah (horizontal)
        // Bagian bawah (kanan ke kiri)
        for (int x = 0; x <= 5; x++) commonPath.Add(new Position(x, 14));
        for (int y = 13; y >= 9; y--) commonPath.Add(new Position(0, y));
        // Bagian kiri (bawah ke atas)
        for (int x = 1; x <= 5; x++) commonPath.Add(new Position(x, 8)); // Jalur tengah (vertikal)
        for (int x = 6; x <= 13; x++) commonPath.Add(new Position(x, 14));


        // Posisi Jalur Umum
        // Jalur sisi atas
        for (int x = 6; x < 9; x++) commonPath.Add(new Position(x, 0)); // Red Start (6,0)
        for (int y = 1; y < 6; y++) commonPath.Add(new Position(8, y)); // Red Path
        // Jalur sisi kanan
        for (int y = 6; y < 9; y++) commonPath.Add(new Position(14, y)); // Yellow Start (14,6)
        for (int x = 13; x > 8; x--) commonPath.Add(new Position(x, 8)); // Yellow Path
        // Jalur sisi bawah
        for (int x = 8; x > 5; x--) commonPath.Add(new Position(x, 14)); // Green Start (8,14)
        for (int y = 13; y > 8; y--) commonPath.Add(new Position(6, y)); // Green Path
        // Jalur sisi kiri
        for (int y = 8; y > 5; y--) commonPath.Add(new Position(0, y)); // Blue Start (0,8)
        for (int x = 1; x < 6; x++) commonPath.Add(new Position(x, 6)); // Blue Path

        // Home Paths (6 langkah)
        var redHome = new List<Position>(); // (7,1) -> (7,6)
        for (int y = 1; y <= 6; y++) redHome.Add(new Position(7, y));

        var yellowHome = new List<Position>(); // (13,7) -> (8,7)
        for (int x = 13; x >= 8; x--) yellowHome.Add(new Position(x, 7));

        var greenHome = new List<Position>(); // (7,13) -> (7,8)
        for (int y = 13; y >= 8; y--) greenHome.Add(new Position(7, y));

        var blueHome = new List<Position>(); // (1,7) -> (6,7)
        for (int x = 1; x <= 6; x++) blueHome.Add(new Position(x, 7));

        // Menggabungkan jalur umum dan jalur home dengan rotasi yang benar untuk setiap warna
        // Perlu penyesuaian untuk memastikan start point yang benar
        List<Position> fullCommonPath = GenerateCommonPath();

        // Path untuk setiap warna
        // Merah: Start di (6,1)
        _playerPaths[LudoColor.Red] = RotatePath(fullCommonPath, _startPoints[LudoColor.Red])
                                    .Take(51) // Ambil 51 langkah common path
                                    .Concat(redHome)
                                    .ToList();

        // Kuning: Start di (13,6)
        _playerPaths[LudoColor.Yellow] = RotatePath(fullCommonPath, _startPoints[LudoColor.Yellow])
                                        .Take(51)
                                        .Concat(yellowHome)
                                        .ToList();

        // Hijau: Start di (8,13)
        _playerPaths[LudoColor.Green] = RotatePath(fullCommonPath, _startPoints[LudoColor.Green])
                                        .Take(51)
                                        .Concat(greenHome)
                                        .ToList();

        // Biru: Start di (1,8)
        _playerPaths[LudoColor.Blue] = RotatePath(fullCommonPath, _startPoints[LudoColor.Blue])
                                    .Take(51)
                                    .Concat(blueHome)
                                    .ToList();
    }

    // Fungsi helper untuk menghasilkan jalur umum (total 52 langkah)
    private List<Position> GenerateCommonPath()
    {
        var path = new List<Position>();
        // Kanan dari Start Red (6,1)
        for (int y = 1; y <= 5; y++) path.Add(new Position(6, y)); // Up
        for (int x = 7; x <= 13; x++) path.Add(new Position(x, 5)); // Right
        path.Add(new Position(14, 5)); // Corner

        // Turun ke Start Yellow (13,6)
        for (int y = 6; y <= 8; y++) path.Add(new Position(14, y));
        for (int x = 13; x >= 9; x--) path.Add(new Position(x, 8)); // Down
        path.Add(new Position(8, 9)); // Corner

        // Kiri ke Start Green (8,13)
        for (int y = 9; y <= 13; y++) path.Add(new Position(8, y));
        for (int x = 7; x >= 1; x--) path.Add(new Position(x, 14)); // Left
        path.Add(new Position(0, 14)); // Corner

        // Naik ke Start Blue (1,8)
        for (int y = 13; y >= 9; y--) path.Add(new Position(0, y));
        for (int x = 1; x <= 5; x++) path.Add(new Position(x, 8)); // Up
        path.Add(new Position(5, 7)); // Corner

        // Kanan ke Start Red (6,1)
        for (int y = 6; y >= 1; y--) path.Add(new Position(6, y));

        return path;
    }

    // Fungsi untuk merotasi jalur sehingga dimulai dari start point yang benar
    private List<Position> RotatePath(List<Position> path, Position startPoint)
    {
        int startIndex = path.IndexOf(startPoint);
        if (startIndex == -1) return path; // Should not happen with correct path and start points

        var rotatedPath = new List<Position>();
        for (int i = 0; i < path.Count; i++)
        {
            rotatedPath.Add(path[(startIndex + i) % path.Count]);
        }
        return rotatedPath;
    }


    // Menginisialisasi tipe zona pada papan
    public void InitializeZones()
    {
        // === BASE ZONES ===
        // Red Base
        for (int x = 0; x <= 5; x++)
            for (int y = 0; y <= 5; y++)
                _zoneMap[new Position(x, y)] = ZoneType.Base;
        // Blue Base
        for (int x = 0; x <= 5; x++)
            for (int y = 9; y <= 14; y++)
                _zoneMap[new Position(x, y)] = ZoneType.Base;
        // Green Base
        for (int x = 9; x <= 14; x++)
            for (int y = 9; y <= 14; y++)
                _zoneMap[new Position(x, y)] = ZoneType.Base;
        // Yellow Base
        for (int x = 9; x <= 14; x++)
            for (int y = 0; y <= 5; y++)
                _zoneMap[new Position(x, y)] = ZoneType.Base;

        // === START POINTS ===
        _zoneMap[_startPoints[LudoColor.Red]] = ZoneType.StartPoint;
        _zoneMap[_startPoints[LudoColor.Yellow]] = ZoneType.StartPoint;
        _zoneMap[_startPoints[LudoColor.Green]] = ZoneType.StartPoint;
        _zoneMap[_startPoints[LudoColor.Blue]] = ZoneType.StartPoint;

        // === HOME PATHS ===
        // Red Home Path
        for (int y = 1; y <= 6; y++) _zoneMap[new Position(7, y)] = ZoneType.HomePath;
        // Yellow Home Path
        for (int x = 8; x <= 13; x++) _zoneMap[new Position(x, 7)] = ZoneType.HomePath;
        // Green Home Path
        for (int y = 8; y <= 13; y++) _zoneMap[new Position(7, y)] = ZoneType.HomePath;
        // Blue Home Path
        for (int x = 1; x <= 6; x++) _zoneMap[new Position(x, 7)] = ZoneType.HomePath;

        // === HOME POINT (Pusat papan) ===
        _zoneMap[new Position(7, 7)] = ZoneType.HomePoint;

        // === SAFE ZONES (bintang) ===
        _zoneMap[new Position(6, 1)] = ZoneType.SafeZone; // Start Red
        _zoneMap[new Position(1, 8)] = ZoneType.SafeZone; // Start Blue
        _zoneMap[new Position(8, 13)] = ZoneType.SafeZone; // Start Green
        _zoneMap[new Position(13, 6)] = ZoneType.SafeZone; // Start Yellow

        _zoneMap[new Position(1, 6)] = ZoneType.SafeZone;
        _zoneMap[new Position(2, 8)] = ZoneType.SafeZone;
        _zoneMap[new Position(6, 13)] = ZoneType.SafeZone;
        _zoneMap[new Position(8, 2)] = ZoneType.SafeZone;
        _zoneMap[new Position(13, 8)] = ZoneType.SafeZone;
        _zoneMap[new Position(12, 6)] = ZoneType.SafeZone;
    }

    public void DrawBoard()
    {
        char[,] display = new char[15, 15];

        // Inisialisasi papan dengan karakter default atau berdasarkan zona
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 15; x++)
            {
                ZoneType zone = GetZoneType(x, y);
                switch (zone)
                {
                    case ZoneType.Base: display[x, y] = '#'; break;
                    case ZoneType.StartPoint: display[x, y] = 'S'; break;
                    case ZoneType.SafeZone: display[x, y] = '*'; break;
                    case ZoneType.HomePath: display[x, y] = '='; break;
                    case ZoneType.HomePoint: display[x, y] = 'H'; break;
                    case ZoneType.CommonPath: display[x, y] = '-'; break;
                    default: display[x, y] = '.'; break;
                }
            }
        }

        // Tampilkan bidak di jalur
        foreach (var kvp in _playerPieces)
        {
            var player = kvp.Key;
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
                    // Tampilkan bidak di base
                    if (piece.BaseIndex != -1 && piece.BaseIndex < _basePositions[piece.PieceColor].Count)
                    {
                        var pos = _basePositions[piece.PieceColor][piece.BaseIndex];
                        display[pos.X, pos.Y] = GetPieceChar(piece.PieceColor);
                    }
                }
            }
        }

        // Cetak grid dengan warna
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 15; x++)
            {
                ConsoleColor originalColor = Console.ForegroundColor;
                char charToDisplay = display[x, y];

                // Atur warna berdasarkan warna bidak
                if (charToDisplay == 'R') Console.ForegroundColor = ConsoleColor.Red;
                else if (charToDisplay == 'Y') Console.ForegroundColor = ConsoleColor.Yellow;
                else if (charToDisplay == 'G') Console.ForegroundColor = ConsoleColor.Green;
                else if (charToDisplay == 'B') Console.ForegroundColor = ConsoleColor.Blue;
                else
                {
                    // Atur warna berdasarkan zona
                    ZoneType zone = GetZoneType(x, y);
                    switch (zone)
                    {
                        case ZoneType.Base: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                        case ZoneType.StartPoint: Console.ForegroundColor = ConsoleColor.White; break;
                        case ZoneType.SafeZone: Console.ForegroundColor = ConsoleColor.Cyan; break;
                        case ZoneType.HomePath: Console.ForegroundColor = ConsoleColor.Magenta; break;
                        case ZoneType.HomePoint: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                        case ZoneType.CommonPath: Console.ForegroundColor = ConsoleColor.Gray; break;
                        default: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                    }
                }
                Console.Write(charToDisplay + " ");
                Console.ForegroundColor = originalColor; // Kembalikan warna
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    // Helper untuk mendapatkan karakter bidak berdasarkan warna
    private char GetPieceChar(LudoColor color)
    {
        return color.ToString()[0];
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
    public ZoneType GetZoneType(int x, int y)
    {
        if (_zoneMap.TryGetValue(new Position(x, y), out var zone))
            return zone;
        // Default untuk area kosong atau area jalur umum yang tidak spesifik
        // Penting: Area jalur umum harus ditetapkan dalam InitializeZones
        // Untuk saat ini, asumsikan jalur kosong di luar zona spesifik adalah CommonPath
        return ZoneType.Empty; // Atau CommonPath, tergantung desain
    }
    public bool IsSafeZone(int x, int y)
    {
        var zone = GetZoneType(x, y);
        return zone == ZoneType.SafeZone || zone == ZoneType.StartPoint || zone == ZoneType.HomePath;
    }

    // Mengecek apakah bidak bisa bergerak
    public bool CanMove(IPiece piece, int steps)
    {
        if (piece.State == PieceState.Home) return false;

        var path = GetPathForPlayer(piece.PieceColor);
        int targetStepIndex = piece.StepIndex + steps;

        // Jika bidak masih di base, hanya bisa bergerak jika dadu 6
        if (piece.State == PieceState.AtBase)
        {
            return steps == 6;
        }
        // Jika bidak aktif, cek apakah langkah tidak melebihi home point
        else if (piece.State == PieceState.Active)
        {
            // Tidak boleh melebihi total panjang jalur + home path
            return targetStepIndex < path.Count;
        }
        return false;
    }

    // Memindahkan bidak
    public bool MovePiece(IPiece piece, int steps)
    {
        if (!CanMove(piece, steps) && piece.State != PieceState.AtBase)
        {
            Console.WriteLine("Bidak tidak bisa digerakkan sejauh itu.");
            return false;
        }

        var path = GetPathForPlayer(piece.PieceColor);
        int oldStepIndex = piece.StepIndex;
        piece.StepIndex += steps;

        // Cek jika bidak mencapai atau melewati home point
        if (piece.StepIndex >= path.Count - 1)
        {
            piece.StepIndex = path.Count - 1; // Pastikan berhenti di home point
            piece.State = PieceState.Home;
            Console.WriteLine($"{piece.PlayerOwner.Name}'s {ColorToString(piece.PieceColor)} piece entered Home!");
            return true;
        }
        else if (piece.StepIndex >= 52 && piece.StepIndex < path.Count) // Masuk HomePath (52 adalah indeks awal HomePath)
        {
            Console.WriteLine($"{piece.PlayerOwner.Name}'s {ColorToString(piece.PieceColor)} piece entered its Home Path.");
            // Tidak perlu mengubah state menjadi Active lagi, sudah Active
        }
        return true;
    }

    // Memindahkan bidak dari base ke start point
    public void MovePieceFromBase(IPiece piece)
    {
        if (piece.State == PieceState.AtBase)
        {
            piece.State = PieceState.Active;
            piece.StepIndex = 0; // Set ke indeks pertama jalur (start point)
            piece.BaseIndex = -1; // Tidak lagi di base
            // Atur ulang posisi base agar tidak ada "lubang" kosong
            RearrangeBasePieces(piece.PlayerOwner);

            // Cek apakah ada bidak lain di start point yang bisa ditangkap
            var startPos = GetPathForPlayer(piece.PieceColor)[0];
            CheckAndCaptureOpponentPieces(piece.PlayerOwner, startPos);
        }
    }

    // Memastikan bidak di base mengisi posisi dengan benar
    private void RearrangeBasePieces(IPlayer player)
    {
        var piecesInBase = _playerPieces[player].Where(p => p.State == PieceState.AtBase).ToList();
        for (int i = 0; i < piecesInBase.Count; i++)
        {
            piecesInBase[i].BaseIndex = i;
        }
    }

    // Mengecek apakah pemain menang (semua bidak di Home)
    public bool CheckWin(IPlayer player)
    {
        return _playerPieces[player].All(p => p.State == PieceState.Home);
    }

    // Mengecek dan melakukan penangkapan bidak lawan
    public bool CheckAndCaptureOpponentPieces(IPlayer currentPlayer, Position currentPosition)
    {
        bool captured = false;
        if (IsSafeZone(currentPosition.X, currentPosition.Y))
        {
            return false; // Bidak tidak bisa ditangkap di safe zone
        }

        foreach (var opponent in _players)
        {
            if (opponent == currentPlayer) continue; // Jangan cek bidak sendiri

            var opponentPieces = _playerPieces[opponent];
            foreach (var enemyPiece in opponentPieces)
            {
                if (enemyPiece.State == PieceState.Active)
                {
                    var enemyPath = GetPathForPlayer(enemyPiece.PieceColor);
                    if (enemyPiece.StepIndex >= 0 && enemyPiece.StepIndex < enemyPath.Count)
                    {
                        var enemyPos = enemyPath[enemyPiece.StepIndex];

                        if (enemyPos.Equals(currentPosition))
                        {
                            // Tangkap bidak lawan, kembalikan ke base
                            ReturnPieceToBase(enemyPiece);
                            Console.WriteLine($"{currentPlayer.Name} menangkap bidak {opponent.Name}'s {ColorToString(enemyPiece.PieceColor)}!");
                            captured = true;
                        }
                    }
                }
            }
        }
        return captured;
    }

    // Mengembalikan bidak ke base
    private void ReturnPieceToBase(IPiece piece)
    {
        piece.State = PieceState.AtBase;
        piece.StepIndex = -1; // Tidak lagi di jalur
        // Temukan posisi base yang kosong
        var playerBasePieces = _playerPieces[piece.PlayerOwner].Where(p => p.State == PieceState.AtBase).ToList();
        int nextBaseIndex = 0;
        while (playerBasePieces.Any(p => p.BaseIndex == nextBaseIndex))
        {
            nextBaseIndex++;
        }
        piece.BaseIndex = nextBaseIndex;
    }

    // Mendapatkan bidak terakhir yang digerakkan (perlu melacaknya)
    // Ini adalah implementasi placeholder, dalam game sungguhan Anda akan melacak ini per giliran.
    private IPiece GetLastMovedPiece(IPlayer player)
    {
        // Untuk simulasi, ambil saja bidak pertama yang aktif, atau bidak yang baru saja bergerak
        // Implementasi lebih kompleks mungkin melibatkan menyimpan referensi ke bidak yang baru digerakkan.
        return _playerPieces[player].FirstOrDefault(p => p.State == PieceState.Active);
    }

    public List<Position> GetPathForPlayer(LudoColor color)
    {
        return _playerPaths[color];
    }

    private string ColorToString(LudoColor color)
    {
        return color.ToString();
    }
}