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
        IBoard board = new Board(); // Board sekarang memiliki inisialisasi dasar

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
    public int[,] Grid { get; } = new int[15, 15]; // Ukuran standar papan Ludo 15x15
    private Dictionary<Position, ZoneType> _predefinedZones; // Menambahkan ini untuk menyimpan info zona

    public Board()
    {
        // Konstruktor ini sekarang akan menginisialisasi Grid dan zona-zona dasar.
        // Ini adalah tempat yang baik untuk setup visual atau statis dari papan.
        _predefinedZones = new Dictionary<Position, ZoneType>();
        InitializeBoardLayout(); // Panggil metode untuk mengisi layout papan
    }

    // Metode baru untuk menginisialisasi tata letak papan secara dasar
    private void InitializeBoardLayout()
    {
        // Contoh sederhana: menandai area Base dan Home Point
        // Kamu bisa menambahkan lebih banyak detail di sini jika diperlukan
        // (Misalnya, untuk tujuan rendering atau validasi awal)

        // Tandai area Base
        // Red Base (0-5, 0-5)
        for (int x = 0; x <= 5; x++) for (int y = 0; y <= 5; y++) _predefinedZones[new Position(x, y)] = ZoneType.Base;
        // Yellow Base (9-14, 0-5)
        for (int x = 9; x <= 14; x++) for (int y = 0; y <= 5; y++) _predefinedZones[new Position(x, y)] = ZoneType.Base;
        // Green Base (9-14, 9-14)
        for (int x = 9; x <= 14; x++) for (int y = 9; y <= 14; y++) _predefinedZones[new Position(x, y)] = ZoneType.Base;
        // Blue Base (0-5, 9-14)
        for (int x = 0; x <= 5; x++) for (int y = 9; y <= 14; y++) _predefinedZones[new Position(x, y)] = ZoneType.Base;

        // Tandai Home Point (Pusat papan)
        _predefinedZones[new Position(7, 7)] = ZoneType.HomePoint;

        // Kita bisa juga menandai area jalur, meskipun detail pergerakan ada di GameController
        // tapi ini adalah data dasar tentang papan itu sendiri, bukan status game.

        // Tandai semua jalur di sini sebagai CommonPath
        // Jalur vertikal tengah (kolom 6 dan 8)
        for (int y = 0; y < 15; y++)
        {
            if (!_predefinedZones.ContainsKey(new Position(6, y)) || _predefinedZones[new Position(6,y)] == ZoneType.Empty)
                _predefinedZones[new Position(6, y)] = ZoneType.CommonPath;
            if (!_predefinedZones.ContainsKey(new Position(8, y)) || _predefinedZones[new Position(8,y)] == ZoneType.Empty)
                _predefinedZones[new Position(8, y)] = ZoneType.CommonPath;
        }
        // Jalur horizontal tengah (baris 6 dan 8)
        for (int x = 0; x < 15; x++)
        {
            if (!_predefinedZones.ContainsKey(new Position(x, 6)) || _predefinedZones[new Position(x,6)] == ZoneType.Empty)
                _predefinedZones[new Position(x, 6)] = ZoneType.CommonPath;
            if (!_predefinedZones.ContainsKey(new Position(x, 8)) || _predefinedZones[new Position(x,8)] == ZoneType.Empty)
                _predefinedZones[new Position(x, 8)] = ZoneType.CommonPath;
        }

        // Tandai jalur tengah yang mengarah ke home (jalur 7)
        for (int y = 1; y <= 13; y++)
        {
            if (y != 7) // Jangan tiban HomePoint
            {
                if (!_predefinedZones.ContainsKey(new Position(7, y)) || _predefinedZones[new Position(7,y)] == ZoneType.Empty)
                    _predefinedZones[new Position(7, y)] = ZoneType.CommonPath;
            }
        }
        for (int x = 1; x <= 13; x++)
        {
            if (x != 7) // Jangan tiban HomePoint
            {
                if (!_predefinedZones.ContainsKey(new Position(x, 7)) || _predefinedZones[new Position(x,7)] == ZoneType.Empty)
                    _predefinedZones[new Position(x, 7)] = ZoneType.CommonPath;
            }
        }

        // Home Paths (bisa juga didefinisikan di sini sebagai layout dasar papan)
        // Red Home Path (7,1 to 7,6)
        for (int y = 1; y <= 6; y++) _predefinedZones[new Position(7, y)] = ZoneType.HomePath;
        // Yellow Home Path (8,7 to 13,7)
        for (int x = 8; x <= 13; x++) _predefinedZones[new Position(x, 7)] = ZoneType.HomePath;
        // Green Home Path (7,8 to 7,13)
        for (int y = 8; y <= 13; y++) _predefinedZones[new Position(7, y)] = ZoneType.HomePath;
        // Blue Home Path (1,7 to 6,7)
        for (int x = 1; x <= 6; x++) _predefinedZones[new Position(x, 7)] = ZoneType.HomePath;

        // Safe Zones (bintang, override CommonPath/StartPoint jika ada)
        // Safe zones include all start points and other marked safe cells
        _predefinedZones[new Position(6, 1)] = ZoneType.SafeZone;  // Start Red
        _predefinedZones[new Position(13, 6)] = ZoneType.SafeZone; // Start Yellow
        _predefinedZones[new Position(8, 13)] = ZoneType.SafeZone; // Start Green
        _predefinedZones[new Position(1, 8)] = ZoneType.SafeZone;  // Start Blue

        _predefinedZones[new Position(1, 6)] = ZoneType.SafeZone;  // Safe after Red's first turn
        _predefinedZones[new Position(6, 9)] = ZoneType.SafeZone;  // Safe after Blue's first turn
        _predefinedZones[new Position(13, 8)] = ZoneType.SafeZone; // Safe after Green's first turn
        _predefinedZones[new Position(8, 5)] = ZoneType.SafeZone;  // Safe after Yellow's first turn
    }

    // Metode untuk mendapatkan tipe zona dari Board (opsional, jika GameController ingin query Board)
    public ZoneType GetZoneType(int x, int y)
    {
        if (_predefinedZones.TryGetValue(new Position(x, y), out var zone))
            return zone;
        return ZoneType.Empty;
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
    public int BaseIndex { get; set; } // Indeks di base (0-3), hanya relevan jika State == AtBase

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
    // Tambahkan metode ini jika GameController ingin query Board untuk tipe zona
    ZoneType GetZoneType(int x, int y);
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
    int BaseIndex { get; set; } // Untuk melacak posisi di base
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
    Empty
}

// --- Kelas GameController ---

public class GameController
{
    private Dictionary<IPlayer, List<IPiece>> _playerPieces;
    private Dictionary<LudoColor, List<Position>> _playerPaths;
    private Dictionary<Position, ZoneType> _zoneMap; // Ini akan diinisialisasi dari Board
    private Dictionary<LudoColor, List<Position>> _basePositions;
    private Dictionary<LudoColor, Position> _startPoints;
    private Dictionary<LudoColor, Position> _homeEntryPoints; // Titik masuk ke jalur Home
    private IDice _dice;
    private List<IPlayer> _players;
    private IBoard _board; // Sekarang bisa digunakan untuk mendapatkan info zona

    private int _currentTurnIndex;
    private int _consecutiveSixes; // Untuk aturan 3x dadu 6

    public event Action OnGameStart;

    public GameController(IPlayer player1, IPlayer player2, IPlayer player3, IPlayer player4, IDice dice, IBoard board)
    {
        _players = new List<IPlayer> { player1, player2, player3, player4 };
        _dice = dice;
        _board = board; // Board diinjeksi ke GameController

        _playerPieces = new Dictionary<IPlayer, List<IPiece>>();
        _playerPaths = new Dictionary<LudoColor, List<Position>>();
        _zoneMap = new Dictionary<Position, ZoneType>(); // Akan diisi dari _board

        // Posisi base untuk setiap warna (Contoh: sudut 4x4)
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
            [LudoColor.Red] = new Position(6, 1),    // Merah: Samping kiri jalur vertikal atas
            [LudoColor.Yellow] = new Position(13, 6), // Kuning: Bawah jalur horizontal kanan
            [LudoColor.Green] = new Position(8, 13),  // Hijau: Samping kanan jalur vertikal bawah
            [LudoColor.Blue] = new Position(1, 8)     // Biru: Atas jalur horizontal kiri
        };

        // Titik masuk ke jalur Home untuk setiap warna (langkah ke-51 di jalur umum)
        // Ini adalah titik di common path sebelum memasuki home path mereka
        _homeEntryPoints = new Dictionary<LudoColor, Position>
        {
            [LudoColor.Red] = new Position(7, 0),    // Sebelum jalur rumah Red
            [LudoColor.Yellow] = new Position(14, 7), // Sebelum jalur rumah Yellow
            [LudoColor.Green] = new Position(7, 14),  // Sebelum jalur rumah Green
            [LudoColor.Blue] = new Position(0, 7)     // Sebelum jalur rumah Blue
        };

        _currentTurnIndex = 0;
        _consecutiveSixes = 0;

        InitializePaths(); // Panggil ini sebelum InitializePieces karena Pieces butuh info path
        InitializePieces();
        InitializeZonesFromBoard(); // Metode baru untuk menyalin/menggunakan data zona dari IBoard
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
                    var activePiecess = _playerPieces[currentPlayer].Where(p => p.State == PieceState.Active).ToList();
                    if (activePiecess.Any())
                    {
                        // Kembalikan bidak yang paling baru digerakkan.
                        // Jika tidak ada pelacakan "terbaru digerakkan", kita ambil sembarang bidak aktif.
                        // Untuk kesederhanaan, kita ambil bidak aktif pertama dalam list.
                        ReturnPieceToBase(activePiecess.First());
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

            // Jika dadu 6, prioritaskan mengeluarkan bidak dari base jika ada
            if (roll == 6 && atBasePieces.Any())
            {
                // Beri opsi untuk mengeluarkan bidak atau memindahkan bidak aktif
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
                        if (input == 1) // Memilih untuk mengeluarkan bidak dari base
                        {
                            var chosenPiece = atBasePieces.First();
                            MovePieceFromBase(chosenPiece);
                            Console.WriteLine($"Bidak {ColorToString(chosenPiece.PieceColor)} keluar dari base ke titik start.");
                            movedPiece = true;
                            gotBonusTurn = true; // Dadu 6 selalu bonus giliran
                            break;
                        }
                        else if (input > 1 && input <= movablePieces.Count + 1) // Memilih bidak aktif
                        {
                            var chosenPiece = movablePieces[input - 2]; // Index disesuaikan karena opsi 1 sudah dipakai
                            if (MovePiece(chosenPiece, roll))
                            {
                                movedPiece = true;
                                if (chosenPiece.State == PieceState.Active)
                                {
                                    var currentPos = GetPathForPlayer(chosenPiece.PieceColor)[chosenPiece.StepIndex];
                                    gotBonusTurn = CheckAndCaptureOpponentPieces(currentPlayer, currentPos);
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

    /// <summary>
    /// Menginisialisasi jalur untuk setiap warna, memastikan bidak bergerak searah jarum jam.
    /// Total 52 langkah di common path, diikuti 6 langkah di home path.
    /// </summary>
    public void InitializePaths()
    {
        // Mendefinisikan satu common path universal (52 langkah)
        var universalCommonPath = new List<Position>();

        // Bagian atas (arah kanan, di antara base merah dan kuning)
        for (int y = 1; y <= 5; y++) universalCommonPath.Add(new Position(6, y));
        universalCommonPath.Add(new Position(5, 6));
        universalCommonPath.Add(new Position(4, 6));
        universalCommonPath.Add(new Position(3, 6));
        universalCommonPath.Add(new Position(2, 6));
        universalCommonPath.Add(new Position(1, 6));
        universalCommonPath.Add(new Position(0, 6));

        // Bagian kiri (arah bawah, di antara base biru dan hijau)
        universalCommonPath.Add(new Position(0, 7)); // Ini adalah Home Entry Point untuk Blue
        universalCommonPath.Add(new Position(0, 8));
        universalCommonPath.Add(new Position(1, 8));
        universalCommonPath.Add(new Position(2, 8));
        universalCommonPath.Add(new Position(3, 8));
        universalCommonPath.Add(new Position(4, 8));
        universalCommonPath.Add(new Position(5, 8));
        universalCommonPath.Add(new Position(6, 9));
        universalCommonPath.Add(new Position(6, 10));
        universalCommonPath.Add(new Position(6, 11));
        universalCommonPath.Add(new Position(6, 12));
        universalCommonPath.Add(new Position(6, 13));
        universalCommonPath.Add(new Position(6, 14));

        // Bagian bawah (arah kiri, di antara base hijau dan biru)
        universalCommonPath.Add(new Position(7, 14)); // Ini adalah Home Entry Point untuk Green
        universalCommonPath.Add(new Position(8, 14));
        universalCommonPath.Add(new Position(8, 13));
        universalCommonPath.Add(new Position(8, 12));
        universalCommonPath.Add(new Position(8, 11));
        universalCommonPath.Add(new Position(8, 10));
        universalCommonPath.Add(new Position(8, 9));
        universalCommonPath.Add(new Position(9, 8));
        universalCommonPath.Add(new Position(10, 8));
        universalCommonPath.Add(new Position(11, 8));
        universalCommonPath.Add(new Position(12, 8));
        universalCommonPath.Add(new Position(13, 8));
        universalCommonPath.Add(new Position(14, 8));

        // Bagian kanan (arah atas, di antara base kuning dan merah)
        universalCommonPath.Add(new Position(14, 7)); // Ini adalah Home Entry Point untuk Yellow
        universalCommonPath.Add(new Position(14, 6));
        universalCommonPath.Add(new Position(13, 6));
        universalCommonPath.Add(new Position(12, 6));
        universalCommonPath.Add(new Position(11, 6));
        universalCommonPath.Add(new Position(10, 6));
        universalCommonPath.Add(new Position(9, 6));
        universalCommonPath.Add(new Position(8, 5));
        universalCommonPath.Add(new Position(8, 4));
        universalCommonPath.Add(new Position(8, 3));
        universalCommonPath.Add(new Position(8, 2));
        universalCommonPath.Add(new Position(8, 1));
        universalCommonPath.Add(new Position(8, 0));

        universalCommonPath.Add(new Position(7, 0)); // Ini adalah Home Entry Point untuk Red
        universalCommonPath.Add(new Position(6, 0)); // Langkah ke-52, tepat sebelum start point merah.

        // Memastikan common path memiliki 52 langkah
        if (universalCommonPath.Count != 52)
        {
            Console.WriteLine($"WARNING: Common path has {universalCommonPath.Count} steps, expected 52!");
        }

        // Definisi jalur Home untuk setiap warna (6 langkah)
        var redHome = new List<Position>(); // (7,1) -> (7,6)
        for (int y = 1; y <= 6; y++) redHome.Add(new Position(7, y));

        var yellowHome = new List<Position>(); // (13,7) -> (8,7)
        for (int x = 13; x >= 8; x--) yellowHome.Add(new Position(x, 7));

        var greenHome = new List<Position>(); // (7,13) -> (7,8)
        for (int y = 13; y >= 8; y--) greenHome.Add(new Position(7, y));

        var blueHome = new List<Position>(); // (1,7) -> (6,7)
        for (int x = 1; x <= 6; x++) blueHome.Add(new Position(x, 7));


        // Menggabungkan jalur umum dan jalur home dengan rotasi yang benar untuk setiap warna
        _playerPaths[LudoColor.Red] = CreatePlayerPath(universalCommonPath, LudoColor.Red, redHome);
        _playerPaths[LudoColor.Yellow] = CreatePlayerPath(universalCommonPath, LudoColor.Yellow, yellowHome);
        _playerPaths[LudoColor.Green] = CreatePlayerPath(universalCommonPath, LudoColor.Green, greenHome);
        _playerPaths[LudoColor.Blue] = CreatePlayerPath(universalCommonPath, LudoColor.Blue, blueHome);
    }

    /// <summary>
    /// Fungsi helper untuk membuat jalur pemain (51 common + 6 home).
    /// </summary>
    private List<Position> CreatePlayerPath(List<Position> commonPath, LudoColor color, List<Position> homePath)
    {
        var playerSpecificPath = new List<Position>();
        Position startPoint = _startPoints[color];
        Position homeEntryPointOnCommonPath = _homeEntryPoints[color]; // The 51st step for this player before home lane

        int startIndex = commonPath.IndexOf(startPoint);
        if (startIndex == -1)
        {
            Console.WriteLine($"Error: Start point {startPoint.X},{startPoint.Y} for {color} not found in common path.");
            return new List<Position>();
        }

        // Add common path steps from player's start point until their home entry point
        // A standard Ludo path has 51 common steps before entering the home lane.
        int stepsAdded = 0;
        for (int i = 0; i < commonPath.Count; i++) // Iterate through the whole common path
        {
            // Calculate current index in the circular path
            int currentIndex = (startIndex + i) % commonPath.Count;
            Position currentPos = commonPath[currentIndex];
            playerSpecificPath.Add(currentPos);
            stepsAdded++;

            // If we reached the home entry point (which is the 51st step for this player's path view)
            if (currentPos.Equals(homeEntryPointOnCommonPath))
            {
                // We should have added exactly 51 steps from the start to the home entry.
                // If not, there's an an issue with the commonPath or homeEntryPoints.
                if (stepsAdded != 51)
                {
                    Console.WriteLine($"WARNING: Player {color} common path before home entry has {stepsAdded} steps, expected 51!");
                }
                break;
            }
        }
        
        // Add the home path (6 steps)
        playerSpecificPath.AddRange(homePath);

        return playerSpecificPath;
    }

    // Menginisialisasi tipe zona pada papan (sekarang mengambil dari IBoard)
    public void InitializeZonesFromBoard()
    {
        // Mengosongkan _zoneMap GameController terlebih dahulu
        _zoneMap.Clear();

        // Iterasi seluruh grid dan dapatkan tipe zona dari Board
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 15; x++)
            {
                Position p = new Position(x, y);
                _zoneMap[p] = _board.GetZoneType(x, y);
            }
        }
    }


    public void DrawBoard()
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("              Papan Ludo                 ");
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
                        case ZoneType.StartPoint: Console.ForegroundColor = ConsoleColor.White; break;
                        case ZoneType.SafeZone: Console.ForegroundColor = ConsoleColor.Cyan; break;
                        case ZoneType.HomePath: Console.ForegroundColor = ConsoleColor.Magenta; break;
                        case ZoneType.HomePoint: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                        case ZoneType.CommonPath: Console.ForegroundColor = ConsoleColor.Gray; break;
                        default: Console.ForegroundColor = ConsoleColor.DarkGray; break; // Area kosong
                    }
                }
                Console.Write(charToDisplay + " "); // Cetak karakter dengan spasi
                Console.ForegroundColor = originalColor; // Kembalikan warna
            }
            Console.WriteLine(); // Baris baru setelah setiap baris X selesai
        }
        Console.ResetColor(); // Reset warna konsol ke default
        Console.WriteLine("------------------------------------------");
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

    // Metode ini sekarang akan memanggil GetZoneType dari objek _board
    public ZoneType GetZoneType(int x, int y)
    {
        return _board.GetZoneType(x, y);
    }

    // Metode ini juga akan memanggil IsSafeZone dari objek _board
    public bool IsSafeZone(Position pos)
    {
        return _board.GetZoneType(pos.X, pos.Y) == ZoneType.SafeZone || _board.GetZoneType(pos.X, pos.Y) == ZoneType.StartPoint;
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
            // Tidak boleh melebihi total panjang jalur (termasuk home path)
            return targetStepIndex < path.Count;
        }
        return false;
    }

    // Memindahkan bidak dari base ke start point
    public void MovePieceFromBase(IPiece piece)
    {
        if (piece.State == PieceState.AtBase)
        {
            piece.State = PieceState.Active;
            piece.StepIndex = _playerPaths[piece.PieceColor].IndexOf(_startPoints[piece.PieceColor]); // Set ke indeks awal jalur (start point)
            piece.BaseIndex = -1; // Tidak lagi di base
            Console.WriteLine($"Bidak {ColorToString(piece.PieceColor)} milik {piece.PlayerOwner.Name} keluar dari base!");

            // Cek apakah ada bidak lawan di start point, dan tangkap jika ada (start point juga safe zone, tapi bisa capture jika keluar base)
            var startPos = GetPathForPlayer(piece.PieceColor)[piece.StepIndex];
            CheckAndCaptureOpponentPieces(piece.PlayerOwner, startPos, true); // True = ignore safe zone for capture on start
        }
    }


    // Memindahkan bidak yang aktif
    public bool MovePiece(IPiece piece, int steps)
    {
        if (piece.State != PieceState.Active) return false;

        var path = GetPathForPlayer(piece.PieceColor);
        int newStepIndex = piece.StepIndex + steps;

        // Cek jika bidak mencapai atau melewati home point
        if (newStepIndex >= path.Count) // path.Count adalah total langkah termasuk Home Point
        {
            // Jika persis mencapai home point (indeks terakhir home path)
            if (newStepIndex == path.Count -1) // Indeks terakhir home path adalah home point
            {
                piece.StepIndex = newStepIndex; 
                piece.State = PieceState.Home;
                Console.WriteLine($"{piece.PlayerOwner.Name}'s {ColorToString(piece.PieceColor)} mencapai Home!");
                return true;
            }
            else // Jika melebihi home point, bidak tidak bisa bergerak (overshoot)
            {
                Console.WriteLine($"Bidak {ColorToString(piece.PieceColor)} milik {piece.PlayerOwner.Name} tidak bisa bergerak {steps} langkah karena akan overshoot Home.");
                return false;
            }
        }
        else
        {
            piece.StepIndex = newStepIndex;
            Console.WriteLine($"{piece.PlayerOwner.Name}'s {ColorToString(piece.PieceColor)} bergerak ke langkah {newStepIndex + 1}.");
            return true;
        }
    }

    // Mengembalikan bidak ke base
    public void ReturnPieceToBase(IPiece piece)
    {
        piece.State = PieceState.AtBase;
        piece.StepIndex = -1;
        // Temukan posisi base yang kosong dan tetapkan BaseIndex
        for (int i = 0; i < 4; i++)
        {
            if (!_playerPieces[piece.PlayerOwner].Any(p => p.State == PieceState.AtBase && p.BaseIndex == i))
            {
                piece.BaseIndex = i;
                break;
            }
        }
        Console.WriteLine($"Bidak {ColorToString(piece.PieceColor)} milik {piece.PlayerOwner.Name} dikembalikan ke base!");
    }

    /// <summary>
    /// Mengecek dan menangkap bidak lawan di posisi tertentu.
    /// </summary>
    /// <param name="currentPlayer">Pemain yang sedang bergerak.</param>
    /// <param name="currentPos">Posisi bidak saat ini.</param>
    /// <param name="ignoreSafeZone">True jika capture boleh dilakukan meskipun di safe zone (misal: saat keluar dari base ke start point).</param>
    /// <returns>True jika ada bidak lawan yang berhasil ditangkap, False jika tidak.</returns>
    public bool CheckAndCaptureOpponentPieces(IPlayer currentPlayer, Position currentPos, bool ignoreSafeZone = false)
    {
        bool captured = false;
        // Jika posisi adalah safe zone dan kita tidak mengabaikan safe zone, maka tidak ada capture.
        // Pengecualian: Saat bidak keluar dari base, ia bisa menangkap bidak lain di start point-nya,
        // meskipun start point adalah safe zone untuk pemiliknya.
        if (!ignoreSafeZone && IsSafeZone(currentPos)) // Menggunakan IsSafeZone dari GameController (yang query _board)
        {
            return false;
        }

        foreach (var playerKvp in _playerPieces)
        {
            IPlayer opponentPlayer = playerKvp.Key;
            if (opponentPlayer == currentPlayer) continue; // Jangan cek bidak sendiri

            foreach (var opponentPiece in playerKvp.Value)
            {
                if (opponentPiece.State == PieceState.Active)
                {
                    var opponentPath = GetPathForPlayer(opponentPiece.PieceColor);
                    if (opponentPiece.StepIndex >= 0 && opponentPiece.StepIndex < opponentPath.Count)
                    {
                        var opponentPos = opponentPath[opponentPiece.StepIndex];

                        if (opponentPos.Equals(currentPos))
                        {
                            // Jika bidak lawan berada di posisi yang sama dan bukan di safe zone
                            // atau jika ignoreSafeZone adalah true (kasus keluar dari base)
                            if (!IsSafeZone(opponentPos) || ignoreSafeZone) // Menggunakan IsSafeZone dari GameController
                            {
                                Console.WriteLine($"Bidak {ColorToString(opponentPiece.PieceColor)} milik {opponentPlayer.Name} ditangkap oleh {currentPlayer.Name}!");
                                ReturnPieceToBase(opponentPiece);
                                captured = true;
                            }
                            else
                            {
                                Console.WriteLine($"Bidak {ColorToString(opponentPiece.PieceColor)} milik {opponentPlayer.Name} berada di Safe Zone, tidak bisa ditangkap.");
                            }
                        }
                    }
                }
            }
        }
        return captured;
    }


    // Mengecek kemenangan
    public bool CheckWin(IPlayer player)
    {
        return _playerPieces[player].All(p => p.State == PieceState.Home);
    }

    // Helper untuk mendapatkan jalur spesifik pemain berdasarkan warna
    private List<Position> GetPathForPlayer(LudoColor color)
    {
        return _playerPaths[color];
    }

    // Helper untuk mengubah LudoColor menjadi string
    private string ColorToString(LudoColor color)
    {
        return color.ToString();
    }
}