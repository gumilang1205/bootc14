using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

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
            if (!_predefinedZones.ContainsKey(new Position(6, y)) || _predefinedZones[new Position(6, y)] == ZoneType.Empty)
                _predefinedZones[new Position(6, y)] = ZoneType.CommonPath;
            if (!_predefinedZones.ContainsKey(new Position(8, y)) || _predefinedZones[new Position(8, y)] == ZoneType.Empty)
                _predefinedZones[new Position(8, y)] = ZoneType.CommonPath;
        }
        // Jalur horizontal tengah (baris 6 dan 8)
        for (int x = 0; x < 15; x++)
        {
            if (!_predefinedZones.ContainsKey(new Position(x, 6)) || _predefinedZones[new Position(x, 6)] == ZoneType.Empty)
                _predefinedZones[new Position(x, 6)] = ZoneType.CommonPath;
            if (!_predefinedZones.ContainsKey(new Position(x, 8)) || _predefinedZones[new Position(x, 8)] == ZoneType.Empty)
                _predefinedZones[new Position(x, 8)] = ZoneType.CommonPath;
        }

        // Tandai jalur tengah yang mengarah ke home (jalur 7)
        for (int y = 1; y <= 13; y++)
        {
            if (y != 7) // Jangan tiban HomePoint
            {
                if (!_predefinedZones.ContainsKey(new Position(7, y)) || _predefinedZones[new Position(7, y)] == ZoneType.Empty)
                    _predefinedZones[new Position(7, y)] = ZoneType.CommonPath;
            }
        }
        for (int x = 1; x <= 13; x++)
        {
            if (x != 7) // Jangan tiban HomePoint
            {
                if (!_predefinedZones.ContainsKey(new Position(x, 7)) || _predefinedZones[new Position(x, 7)] == ZoneType.Empty)
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
        _predefinedZones[new Position(6, 1)] = ZoneType.StartPoint;   // Start Red
        _predefinedZones[new Position(13, 6)] = ZoneType.StartPoint;  // Start Yellow
        _predefinedZones[new Position(8, 13)] = ZoneType.StartPoint;  // Start Green
        _predefinedZones[new Position(1, 8)] = ZoneType.StartPoint;   // Start Blue

        _predefinedZones[new Position(2, 6)] = ZoneType.SafeZone;   // Safe after Red's first turn
        _predefinedZones[new Position(6, 12)] = ZoneType.SafeZone;   // Safe after Blue's first turn
        _predefinedZones[new Position(12, 8)] = ZoneType.SafeZone;  // Safe after Green's first turn
        _predefinedZones[new Position(8, 2)] = ZoneType.SafeZone;   // Safe after Yellow's first turn
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

    // Variabel _consecutiveSixes sudah dihapus

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

        // Titik start untuk setiap warna
        _startPoints = new Dictionary<LudoColor, Position>
        {
            [LudoColor.Red] = new Position(6, 1),      // Merah: Samping kiri jalur vertikal atas
            [LudoColor.Yellow] = new Position(13, 6),  // Kuning: Bawah jalur horizontal kanan
            [LudoColor.Green] = new Position(8, 13),   // Hijau: Samping kanan jalur vertikal bawah
            [LudoColor.Blue] = new Position(1, 8)      // Biru: Atas jalur horizontal kiri
        };

        // Titik masuk ke jalur Home untuk setiap warna (langkah ke-51 di jalur umum)
        // Ini adalah titik di common path sebelum memasuki home path mereka
        _homeEntryPoints = new Dictionary<LudoColor, Position>
        {
            [LudoColor.Red] = new Position(7, 0),    // Sebelum jalur rumah Red
            [LudoColor.Yellow] = new Position(14, 7), // Sebelum jalur rumah Yellow
            [LudoColor.Green] = new Position(7, 14),  // Sebelum jalur rumah Green
            [LudoColor.Blue] = new Position(0, 7)    // Sebelum jalur rumah Blue
        };

        _currentTurnIndex = 0;
        // _consecutiveSixes sudah dihapus

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
        for (int x = 5; x >= 0; x--) universalCommonPath.Add(new Position(x, 6));
        // Bagian kiri (arah bawah, di antara base biru dan hijau)
        universalCommonPath.Add(new Position(0, 7)); // Ini adalah Home Entry Point untuk Blue
        for (int x = 0; x <= 5; x++) universalCommonPath.Add(new Position(x, 8));
        for (int y = 9; y <= 14; y++) universalCommonPath.Add(new Position(6, y));
        // Bagian bawah (arah kiri, di antara base hijau dan biru)
        universalCommonPath.Add(new Position(7, 14)); // Ini adalah Home Entry Point untuk Green
        for (int y = 14; y >= 9; y--) universalCommonPath.Add(new Position(8, y));
        for (int x = 9; x <= 14; x++) universalCommonPath.Add(new Position(x, 8));
        // Bagian kanan (arah atas, di antara base kuning dan merah)
        universalCommonPath.Add(new Position(14, 7)); // Ini adalah Home Entry Point untuk Yellow
        for (int x = 14; x >= 9; x--) universalCommonPath.Add(new Position(x, 6));
        for (int y = 5; y >= 0; y--) universalCommonPath.Add(new Position(8, y));
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

        //  Menambahkan langkah-langkah jalur umum dari titik awal pemain hingga titik masuk rumah mereka
        // Jalur Ludo standar memiliki 51 langkah umum sebelum memasuki jalur pulang.
        int stepsAdded = 0;
        for (int i = 0; i < commonPath.Count; i++) // Lakukan pengulangan melalui seluruh jalur umum
        {
            // Hitung indeks saat ini di jalur melingkar
            int currentIndex = (startIndex + i) % commonPath.Count;
            Position currentPos = commonPath[currentIndex];
            playerSpecificPath.Add(currentPos);
            stepsAdded++;

            // Jika kita mencapai titik masuk rumah (yang merupakan langkah ke-51 untuk tampilan jalur pemain ini)
            if (currentPos.Equals(homeEntryPointOnCommonPath))
            {
                // Kita seharusnya menambahkan tepat 51 langkah dari awal hingga entri beranda.
                // Jika tidak, berarti ada masalah dengan commonPath atau homeEntryPoints.
                if (stepsAdded != 51)
                {
                    Console.WriteLine($"WARNING: Player {color} common path before home entry has {stepsAdded} steps, expected 51!");
                }
                break;
            }
        }

        // Menambahkan jalur rumah (6 langkah)
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

    public IPlayer GetCurrentPlayer()
    {
        return _players[_currentTurnIndex];
    }

    private void NextTurn()
    {
        _currentTurnIndex = (_currentTurnIndex + 1) % _players.Count;
    }

    private int RollDice()
    {
        return _dice.Roll();
    }

    // Metode untuk memindahkan bidak
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

        // Cek jika bidak akan mendarat di Home Point
        if (newStepIndex == path.Count - 1)
        {
            piece.StepIndex = newStepIndex;
            piece.State = PieceState.Home;
            Console.WriteLine($"Bidak {ColorToString(piece.PieceColor)} mendarat di Home Point!");
            return true;
        }
        // Cek jika langkah melebihi jalur
        else if (newStepIndex >= path.Count)
        {
            Console.WriteLine($"Langkah {roll} terlalu banyak. Bidak tidak bisa digerakkan.");
            return false;
        }
        // Pindahkan bidak
        else
        {
            piece.StepIndex = newStepIndex;
            Console.WriteLine($"Bidak {ColorToString(piece.PieceColor)} pindah ke langkah {piece.StepIndex + 1}.");
            return true;
        }
    }

    // Memindahkan bidak dari base ke titik start
    private void MovePieceFromBase(IPiece piece)
    {
        var startPoint = _startPoints[piece.PieceColor];
        var path = GetPathForPlayer(piece.PieceColor);

        // Temukan index start point di path pemain
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

    private bool CheckAndCaptureOpponentPieces(IPlayer currentPlayer, Position currentPosition)
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

    private bool CanMove(IPiece piece, int roll)
    {
        if (piece.State == PieceState.AtBase)
        {
            return roll == 6;
        }
        // Pastikan tidak melebihi jalur
        var path = GetPathForPlayer(piece.PieceColor);
        return piece.StepIndex + roll < path.Count;
    }

    private bool CheckWin(IPlayer player)
    {
        return _playerPieces[player].All(p => p.State == PieceState.Home);
    }

    private string ColorToString(LudoColor color)
    {
        return color.ToString();
    }

    private char GetPieceChar(LudoColor color)
    {
        return color.ToString()[0];
    }

    // Metode helper untuk mendapatkan jalur berdasarkan warna
    private List<Position> GetPathForPlayer(LudoColor color)
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