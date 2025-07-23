using System;
using System.Collections.Generic;
using System.Linq;

// Kelas Domino mewakili satu kartu domino dengan dua ujung angka.
public class Domino
{
    public int End1 { get; private set; }
    public int End2 { get; private set; }

    public Domino(int end1, int end2)
    {
        End1 = end1;
        End2 = end2;
    }

    // Mengembalikan nilai ujung lain dari domino jika salah satu ujung diberikan.
    public int GetOtherEnd(int value)
    {
        if (End1 == value)
        {
            return End2;
        }
        else if (End2 == value)
        {
            return End1;
        }
        throw new ArgumentException("Nilai tidak ditemukan di domino ini.");
    }

    // Memeriksa apakah domino adalah double (kedua ujung memiliki angka yang sama).
    public bool IsDouble()
    {
        return End1 == End2;
    }

    // Memutar domino (misalnya, 3-5 menjadi 5-3).
    public void Rotate()
    {
        int temp = End1;
        End1 = End2;
        End2 = temp;
    }

    // Mengembalikan representasi string dari domino (misalnya, "[3|5]").
    public override string ToString()
    {
        return $"[{End1}|{End2}]";
    }

    // Membandingkan dua objek Domino untuk kesamaan nilai (tidak peduli urutan ujung).
    public override bool Equals(object obj)
    {
        if (obj is Domino other)
        {
            return (End1 == other.End1 && End2 == other.End2) ||
                   (End1 == other.End2 && End2 == other.End1);
        }
        return false;
    }

    public override int GetHashCode()
    {
        // Pastikan hash code konsisten terlepas dari rotasi
        return Math.Min(End1, End2).GetHashCode() ^ Math.Max(End1, End2).GetHashCode();
    }
}

// Kelas Hand mewakili kumpulan domino yang dimiliki oleh seorang pemain.
public class Hand
{
    private List<Domino> dominoes;

    public Hand()
    {
        dominoes = new List<Domino>();
    }

    // Menambahkan domino ke tangan.
    public void AddDomino(Domino domino)
    {
        dominoes.Add(domino);
    }

    // Menghapus domino dari tangan.
    public bool RemoveDomino(Domino domino)
    {
        // Gunakan Equals untuk memastikan domino yang benar dihapus, terlepas dari rotasi.
        return dominoes.RemoveAll(d => d.Equals(domino)) > 0;
    }

    // Mengembalikan daftar domino di tangan.
    public List<Domino> GetDominoes()
    {
        return new List<Domino>(dominoes); // Mengembalikan salinan untuk mencegah modifikasi eksternal.
    }

    // Memeriksa apakah tangan kosong.
    public bool IsEmpty()
    {
        return dominoes.Count == 0;
    }

    // Menghitung total skor domino yang tersisa di tangan.
    public int CalculateScore()
    {
        return dominoes.Sum(d => d.End1 + d.End2);
    }

    // Mencari domino yang dapat dimainkan dari tangan berdasarkan kondisi papan.
    public Domino FindPlayableDomino(Board board)
    {
        if (board.IsEmpty())
        {
            // Jika papan kosong, domino apa pun bisa dimainkan (biasanya double tertinggi).
            // Untuk demo, kita ambil saja yang pertama.
            return dominoes.FirstOrDefault();
        }

        foreach (var domino in dominoes)
        {
            // Coba cocokkan dengan ujung kiri papan
            if (domino.End1 == board.LeftEnd || domino.End2 == board.LeftEnd)
            {
                return domino;
            }
            // Coba cocokkan dengan ujung kanan papan
            if (domino.End1 == board.RightEnd || domino.End2 == board.RightEnd)
            {
                return domino;
            }
        }
        return null; // Tidak ada domino yang bisa dimainkan.
    }
}

// Kelas Player mewakili seorang pemain dalam permainan.
public class Player
{
    public string Name { get; private set; }
    public Hand Hand { get; private set; }
    public int Score { get; private set; }

    public Player(string name)
    {
        Name = name;
        Hand = new Hand();
        Score = 0;
    }

    // Memainkan domino ke papan.
    public bool Play(Domino domino, Board board, string end)
    {
        // Validasi dasar, logika detail ada di Board.PlaceDomino
        if (Hand.GetDominoes().Contains(domino) && board.PlaceDomino(domino, end))
        {
            Hand.RemoveDomino(domino);
            Console.WriteLine($"{Name} memainkan {domino} di ujung {end}.");
            return true;
        }
        Console.WriteLine($"Gagal memainkan {domino} di ujung {end}.");
        return false;
    }

    // Mengembalikan tangan pemain.
    public Hand GetHand()
    {
        return Hand;
    }

    // Mengembalikan skor pemain.
    public int GetScore()
    {
        return Score;
    }

    // Menambahkan poin ke skor pemain.
    public void AddScore(int points)
    {
        Score += points;
    }

    // Memeriksa apakah pemain memiliki domino yang dapat dimainkan.
    public bool HasPlayableDomino(Board board)
    {
        return Hand.FindPlayableDomino(board) != null;
    }
}

// Kelas Board mewakili area permainan tempat domino diletakkan.
public class Board
{
    private List<Domino> playedDominoes;
    public int LeftEnd { get; private set; }
    public int RightEnd { get; private set; }

    public Board()
    {
        playedDominoes = new List<Domino>();
        LeftEnd = -1; // -1 menunjukkan papan kosong
        RightEnd = -1; // -1 menunjukkan papan kosong
    }

    // Meletakkan domino di papan.
    public bool PlaceDomino(Domino domino, string end)
    {
        if (IsEmpty())
        {
            playedDominoes.Add(domino);
            LeftEnd = domino.End1;
            RightEnd = domino.End2;
            return true;
        }

        bool placed = false;
        if (end.ToLower() == "left")
        {
            if (domino.End1 == LeftEnd)
            {
                // Domino cocok, tambahkan di kiri
                playedDominoes.Insert(0, domino);
                LeftEnd = domino.End2;
                placed = true;
            }
            else if (domino.End2 == LeftEnd)
            {
                // Domino cocok, putar dan tambahkan di kiri
                domino.Rotate();
                playedDominoes.Insert(0, domino);
                LeftEnd = domino.End2;
                placed = true;
            }
        }
        else if (end.ToLower() == "right")
        {
            if (domino.End1 == RightEnd)
            {
                // Domino cocok, putar dan tambahkan di kanan
                domino.Rotate();
                playedDominoes.Add(domino);
                RightEnd = domino.End2;
                placed = true;
            }
            else if (domino.End2 == RightEnd)
            {
                // Domino cocok, tambahkan di kanan
                playedDominoes.Add(domino);
                RightEnd = domino.End1;
                placed = true;
            }
        }

        return placed;
    }

    // Mengembalikan nilai ujung kiri papan.
    public int GetLeftEnd()
    {
        return LeftEnd;
    }

    // Mengembalikan nilai ujung kanan papan.
    public int GetRightEnd()
    {
        return RightEnd;
    }

    // Mengembalikan daftar domino yang sudah dimainkan di papan.
    public List<Domino> GetPlayedDominoes()
    {
        return new List<Domino>(playedDominoes);
    }

    // Memeriksa apakah papan kosong.
    public bool IsEmpty()
    {
        return playedDominoes.Count == 0;
    }

    // Mengembalikan representasi string dari papan.
    public override string ToString()
    {
        if (IsEmpty())
        {
            return "Papan: Kosong";
        }
        return "Papan: " + string.Join("-", playedDominoes.Select(d => d.ToString()));
    }
}

// Kelas Game mengelola alur permainan secara keseluruhan.
public class Game
{
    private List<Player> players;
    private Board board;
    private Player currentPlayer;
    private int turnDirection; // 1 for clockwise, -1 for counter-clockwise
    private Player firstPlayer;
    private Random random;

    public Game(List<string> playerNames)
    {
        players = new List<Player>();
        foreach (var name in playerNames)
        {
            players.Add(new Player(name));
        }
        board = new Board();
        turnDirection = 1; // Default clockwise
        random = new Random();
    }

    // Memulai permainan: membuat, mengocok, dan membagikan semua domino.
    public void StartGame(int initialTilesPerPlayer)
    {
        Console.WriteLine("Memulai permainan domino...");

        // 1. Buat semua domino (0-0 hingga 6-6)
        List<Domino> allDominos = new List<Domino>();
        for (int i = 0; i <= 6; i++)
        {
            for (int j = i; j <= 6; j++)
            {
                allDominos.Add(new Domino(i, j));
            }
        }

        // 2. Kocok domino
        allDominos = allDominos.OrderBy(d => random.Next()).ToList();

        // 3. Bagikan domino ke setiap pemain
        int dominoIndex = 0;
        foreach (var player in players)
        {
            for (int i = 0; i < initialTilesPerPlayer; i++)
            {
                if (dominoIndex < allDominos.Count)
                {
                    player.Hand.AddDomino(allDominos[dominoIndex]);
                    dominoIndex++;
                }
                else
                {
                    Console.WriteLine("Tidak cukup domino untuk dibagikan ke semua pemain secara merata.");
                    break;
                }
            }
        }

        // Domino yang tersisa tidak digunakan dalam permainan ini
        if (dominoIndex < allDominos.Count)
        {
            Console.WriteLine($"Ada {allDominos.Count - dominoIndex} domino sisa yang tidak dibagikan.");
        }

        // 4. Tentukan pemain pertama
        firstPlayer = DetermineStartingPlayer();
        currentPlayer = firstPlayer;
        Console.WriteLine($"Pemain pertama adalah: {firstPlayer.Name}");
        Console.WriteLine("------------------------------------------");
    }

    // Menentukan pemain pertama (misalnya, yang memiliki double tertinggi).
    public Player DetermineStartingPlayer()
    {
        Player startingPlayer = null;
        Domino highestDouble = null;

        foreach (var player in players)
        {
            foreach (var domino in player.Hand.GetDominoes())
            {
                if (domino.IsDouble())
                {
                    if (highestDouble == null || domino.End1 > highestDouble.End1)
                    {
                        highestDouble = domino;
                        startingPlayer = player;
                    }
                }
            }
        }

        // Jika tidak ada double, pilih pemain pertama secara acak
        if (startingPlayer == null)
        {
            startingPlayer = players[random.Next(players.Count)];
        }
        return startingPlayer;
    }

    // Mengelola giliran pemain.
    public void NextTurn()
    {
        int currentIndex = players.IndexOf(currentPlayer);
        currentIndex = (currentIndex + turnDirection + players.Count) % players.Count;
        currentPlayer = players[currentIndex];
    }

    // Memainkan domino oleh pemain.
    public bool PlayDomino(Player player, Domino domino, string end)
    {
        return player.Play(domino, board, end);
    }

    // Memeriksa kondisi kemenangan (pemain kehabisan domino).
    public bool CheckWinCondition()
    {
        foreach (var player in players)
        {
            if (player.Hand.IsEmpty())
            {
                Console.WriteLine($"\n!!! {player.Name} kehabisan domino dan memenangkan putaran ini !!!");
                // Hitung skor pemain lain
                foreach (var otherPlayer in players)
                {
                    if (otherPlayer != player)
                    {
                        int score = otherPlayer.Hand.CalculateScore();
                        player.AddScore(score); // Pemenang mendapatkan skor dari tangan pemain lain
                        Console.WriteLine($"{player.Name} mendapatkan {score} poin dari {otherPlayer.Name}.");
                    }
                }
                Console.WriteLine($"Total skor {player.Name} saat ini: {player.GetScore()}");
                return true;
            }
        }
        return false;
    }

    // Mengakhiri permainan (opsional, untuk multi-putaran).
    public void EndGame()
    {
        Console.WriteLine("\nPermainan berakhir.");
        Console.WriteLine("Skor Akhir:");
        foreach (var player in players.OrderByDescending(p => p.GetScore()))
        {
            Console.WriteLine($"- {player.Name}: {player.GetScore()} poin");
        }
    }

    // Metode untuk menjalankan satu putaran permainan.
    public void PlayRound()
    {
        bool roundOver = false;
        int consecutivePasses = 0; // Untuk mendeteksi jika permainan macet

        while (!roundOver)
        {
            Console.WriteLine($"\n--- Giliran {currentPlayer.Name} ---");
            Console.WriteLine(board.ToString());
            Console.WriteLine($"{currentPlayer.Name}, tangan Anda: {string.Join(" ", currentPlayer.Hand.GetDominoes().Select(d => d.ToString()))}");

            if (currentPlayer.HasPlayableDomino(board))
            {
                consecutivePasses = 0; // Reset jika ada yang bisa bermain
                bool validMove = false;
                while (!validMove)
                {
                    Console.Write("Pilih domino yang ingin dimainkan (misal: 3-5): ");
                    string inputDominoStr = Console.ReadLine();
                    Domino chosenDomino = null;
                    try
                    {
                        string[] parts = inputDominoStr.Split('-');
                        int e1 = int.Parse(parts[0]);
                        int e2 = int.Parse(parts[1]);
                        chosenDomino = new Domino(e1, e2); // Buat objek domino sementara untuk perbandingan

                        // Pastikan domino ada di tangan pemain
                        Domino actualDominoInHand = currentPlayer.Hand.GetDominoes()
                            .FirstOrDefault(d => d.Equals(chosenDomino));

                        if (actualDominoInHand == null)
                        {
                            Console.WriteLine("Domino tidak ada di tangan Anda. Coba lagi.");
                            continue;
                        }

                        Console.Write("Mainkan di ujung mana? (left/right): ");
                        string endToPlay = Console.ReadLine();

                        if (board.IsEmpty())
                        {
                            // Jika papan kosong, domino apa pun bisa dimainkan
                            validMove = PlayDomino(currentPlayer, actualDominoInHand, "right"); // Default ke right
                        }
                        else
                        {
                            validMove = PlayDomino(currentPlayer, actualDominoInHand, endToPlay);
                        }

                        if (!validMove)
                        {
                            Console.WriteLine("Gerakan tidak valid. Domino tidak cocok atau ujung salah. Coba lagi.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Input tidak valid: {ex.Message}. Coba lagi.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"{currentPlayer.Name} tidak memiliki domino yang bisa dimainkan. Melewatkan giliran.");
                consecutivePasses++;
            }

            roundOver = CheckWinCondition();

            // Jika semua pemain melewati giliran secara berurutan, permainan macet.
            if (consecutivePasses >= players.Count)
            {
                Console.WriteLine("\nPermainan macet! Tidak ada yang bisa bergerak.");
                roundOver = true;
                // Hitung skor untuk semua pemain
                foreach (var player in players)
                {
                    int score = player.Hand.CalculateScore();
                    player.AddScore(score); // Setiap pemain menambahkan skor tangannya sendiri
                    Console.WriteLine($"{player.Name} mendapatkan {score} poin dari tangannya sendiri.");
                }
            }

            if (!roundOver)
            {
                NextTurn();
            }
        }
    }
}

// Program utama untuk menjalankan game.
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Selamat Datang di Permainan Domino!");

        Console.Write("Masukkan jumlah pemain (2-4): ");
        int numPlayers;
        while (!int.TryParse(Console.ReadLine(), out numPlayers) || numPlayers < 2 || numPlayers > 4)
        {
            Console.WriteLine("Jumlah pemain tidak valid. Masukkan angka antara 2 dan 4.");
            Console.Write("Masukkan jumlah pemain (2-4): ");
        }

        List<string> playerNames = new List<string>();
        for (int i = 0; i < numPlayers; i++)
        {
            Console.Write($"Masukkan nama pemain {i + 1}: ");
            playerNames.Add(Console.ReadLine());
        }

        int initialTilesPerPlayer = 7; // Umumnya 7 domino untuk 2-4 pemain
        if (numPlayers > 2) initialTilesPerPlayer = 5; // Umumnya 5 domino untuk 3-4 pemain

        Game game = new Game(playerNames);
        game.StartGame(initialTilesPerPlayer);

        game.PlayRound(); // Jalankan satu putaran permainan

        game.EndGame(); // Tampilkan skor akhir
        Console.WriteLine("\nTekan tombol apa saja untuk keluar.");
        Console.ReadKey();
    }
}
