using System;
class Program
{
    static void Main(string[] args)
    {
        Player player1 = new Player("Andi", LudoColor.Red);
        Player player2 = new Player("Bobi", LudoColor.Yellow);
        Player player3 = new Player("Cica", LudoColor.Green);
        Player player4 = new Player("Duda", LudoColor.Blue);

        IDice dice = new Dice();
        IBoard board = new Board();

        GameController controller = new GameController(player1, player2, player3, player4, dice, board);
        controller.OnGameStart += () => Console.WriteLine("Selamat bermain!!!");

        controller.StartGame();




    }
}
public class Board : IBoard
{
    public int[,] Grid { get; }
    public Board()
    {
        Grid = new int[15, 15];

    }

}
public class Dice : IDice
{
    private Random _random;
    public Dice()
    {
        _random = new Random();
    }
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
    public int StepIndex { get; set; }
    public int BaseIndex { get; set; }

    public Piece(IPlayer ownerPlayer, LudoColor pieceColor)
    {
        PieceColor = pieceColor;
        PlayerOwner = ownerPlayer;
        State = PieceState.AtBase;
        StepIndex = 0;
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
public interface IBoard
{
    int[,] Grid { get; }
}
public interface IDice
{
    public int Roll();
}
public interface IPiece
{
    LudoColor PieceColor { get; }
    IPlayer PlayerOwner { get; }
    PieceState State { get; set; }
    int StepIndex { get; set; }
    public int BaseIndex { get; set; }

}
public interface IPlayer
{
    public string Name { get; set; }
    public LudoColor Color { get; set; }
}
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
    BlockedPath,
    Empty
}
public class GameController
{
    private Dictionary<IPlayer, List<IPiece>> _playerPieces;
    private Dictionary<LudoColor, List<Position>> _playerPaths;
    private Dictionary<Position, ZoneType> _zoneMap;
    private Dictionary<LudoColor, List<Position>> _basePositions;
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
        _basePositions = new Dictionary<LudoColor, List<Position>>
        {
            [LudoColor.Red] = new List<Position>
            {
                new Position(1,1), new Position(1,3), new Position(3,1), new Position(3,3)
            },
            [LudoColor.Yellow] = new List<Position>
            {
                new Position(11,1), new Position(11,3), new Position(13,1), new Position(13,3)
            },
            [LudoColor.Green] = new List<Position>
            {
                new Position(11,11), new Position(11,13), new Position(13,11), new Position(13,13)
            },
            [LudoColor.Blue] = new List<Position>
            {
                new Position(1,11), new Position(1,13), new Position(3,11), new Position(3,13)
            }
        };


        _currentTurnIndex = 0;
        InitializePiece();
        InitializePath();
        InitializeZones();
        DrawBoard();
    }
    public void DrawBoard()
    {
        char[,] display = new char[15, 15];

        // isi default
        for (int y = 0; y < 15; y++)
            for (int x = 0; x < 15; x++)
                display[x, y] = '.';

        // Tampilkan bidak di jalur
        foreach (var kvp in _playerPieces)
        {
            var player = kvp.Key;
            var pieces = kvp.Value;

            foreach (var piece in pieces)
            {
                if (piece.State == PieceState.Active || piece.State == PieceState.Home)
                {
                    var path = GetPathForPlayer(piece.PieceColor);
                    if (piece.StepIndex >= 0 && piece.StepIndex < path.Count)
                    {
                        var pos = path[piece.StepIndex];
                        display[pos.X, pos.Y] = player.Name[0];
                    }
                }
            }
        }

        // Tampilkan bidak di Base
        foreach (var kvp in _playerPieces)
        {
            var player = kvp.Key;
            var pieces = kvp.Value.Where(p => p.State == PieceState.AtBase);

            foreach (var piece in pieces)
            {
                var baseList = _basePositions[piece.PieceColor];
                if (piece.BaseIndex >= 0 && piece.BaseIndex < baseList.Count)
                {
                    var pos = baseList[piece.BaseIndex];
                    display[pos.X, pos.Y] = player.Name[0];
                }
            }
        }

        // Cetak grid
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 15; x++)
            {
                var zone = GetZoneType(x, y);
                switch (zone)
                {
                    case ZoneType.Base: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                    case ZoneType.StartPoint: Console.ForegroundColor = ConsoleColor.White; break;
                    case ZoneType.SafeZone: Console.ForegroundColor = ConsoleColor.Cyan; break;
                    case ZoneType.HomePath: Console.ForegroundColor = ConsoleColor.Magenta; break;
                    case ZoneType.HomePoint: Console.ForegroundColor = ConsoleColor.Yellow; break;
                    default: Console.ForegroundColor = ConsoleColor.Gray; break;
                }

                Console.Write(display[x, y] + " ");
            }
            Console.WriteLine();
        }
        Console.ResetColor();
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
                piece.BaseIndex = i;
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
        InitializePiece();
        InitializePath();

        OnGameStart?.Invoke();

        bool gameOver = false;
        bool bonusTurn = false;

        while (!gameOver)
        {
            if (!bonusTurn)
            {
                Console.Clear();
                DrawBoard();
            }

            var currentPlayer = GetCurrentPlayer();
            Console.WriteLine($"\nGiliran: {currentPlayer.Name} ({currentPlayer.Color})");

            // Lempar dadu
            int roll = RollDice();
            Console.WriteLine($"Lempar dadu: {roll}");

            var pieces = _playerPieces[currentPlayer];
            var activePieces = pieces.Where(p => p.State == PieceState.Active).ToList();
            var atBasePieces = pieces.Where(p => p.State == PieceState.AtBase).ToList();

            bool moved = false;
            bonusTurn = false;

            // === ATURAN EKSTRA: Semua bidak di Base dan bukan 6 ===
            if (!activePieces.Any() && atBasePieces.Any() && roll != 6)
            {
                Console.WriteLine("Semua bidak di Base dan kamu tidak dapat 6. Giliran dilewati!");
                NextTurn();
                Console.WriteLine("\nTekan ENTER untuk lanjut...");
                Console.ReadLine();
                continue;
            }

            // === KELUAR DARI BASE JIKA 6 ===
            if (roll == 6 && atBasePieces.Any())
            {
                Console.WriteLine("Kamu punya bidak di Base. Mau keluar? (y/n)");
                var choice = Console.ReadLine();
                if (choice?.ToLower() == "y")
                {
                    var piece = atBasePieces.First();
                    piece.State = PieceState.Active;
                    piece.StepIndex = 0;
                    piece.BaseIndex = -1;
                    moved = true;
                }
            }

            // === GERAK BIDAK AKTIF ===
            if (activePieces.Any())
            {
                Console.WriteLine("Bidak aktif:");
                for (int i = 0; i < activePieces.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Bidak ke-{i + 1} di langkah {activePieces[i].StepIndex}");
                }
                Console.Write("Pilih bidak (nomor): ");
                if (int.TryParse(Console.ReadLine(), out int index) &&
                    index >= 1 && index <= activePieces.Count)
                {
                    var chosenPiece = activePieces[index - 1];
                    MovePiece(chosenPiece, roll);
                    moved = true;
                }
                else
                {
                    Console.WriteLine("Input salah, bidak tidak bergerak.");
                }
            }
            else if (!moved)
            {
                Console.WriteLine("Tidak ada bidak untuk digerakkan.");
            }

            // === CEK CAPTURE DAN BONUS TURN ===
            if (moved)
            {
                bool captureHappened = CaptureIfExists();
                if (captureHappened)
                {
                    bonusTurn = true;
                    Console.WriteLine("Kamu dapat bonus giliran karena menangkap lawan!");
                }

                // CEK MENANG
                if (CheckWin(currentPlayer))
                {
                    Console.Clear();
                    DrawBoard();
                    Console.WriteLine($"\n=== {currentPlayer.Name} MENANG!!! ===");
                    gameOver = true;
                    break;
                }
            }

            // === BONUS TURN JIKA DAPAT 6 ===
            if (roll == 6)
            {
                Console.WriteLine("Kamu dapat 6! Bonus giliran!");
                bonusTurn = true;
            }

            // === GILIRAN SELANJUTNYA ===
            if (!bonusTurn)
                NextTurn();

            Console.WriteLine("\nTekan ENTER untuk lanjut giliran berikutnya...");
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
        if (piece.StepIndex >= 0 && piece.StepIndex < path.Count)
        {
            var position = path[piece.StepIndex];
            return _zoneMap.ContainsKey(position) ? _zoneMap[position] : ZoneType.Empty;
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
    public bool CaptureIfExists()
    {
        var currentPlayer = GetCurrentPlayer();
        var currentPieces = _playerPieces[currentPlayer];
        bool captured = false;

        foreach (var myPiece in currentPieces)
        {
            if (myPiece.State == PieceState.AtBase || myPiece.State == PieceState.Home)
                continue;

            var myPath = GetPathForPlayer(myPiece.PieceColor);
            var myPos = myPath[myPiece.StepIndex];

            if (IsSafeZone(myPos.X, myPos.Y))
                continue;

            foreach (var opponent in _players)
            {
                if (opponent == currentPlayer) continue;

                var opponentPieces = _playerPieces[opponent];
                foreach (var enemyPiece in opponentPieces)
                {
                    if (enemyPiece.State == PieceState.Active)
                    {
                        var enemyPath = GetPathForPlayer(enemyPiece.PieceColor);
                        var enemyPos = enemyPath[enemyPiece.StepIndex];

                        if (enemyPos.Equals(myPos))
                        {
                            enemyPiece.State = PieceState.AtBase;
                            enemyPiece.StepIndex = 0;
                            Console.WriteLine($"{currentPlayer.Name} menangkap bidak {opponent.Name}!");
                            captured = true;
                        }
                    }
                }
            }
        }
        return captured;
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