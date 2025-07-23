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
// Logika perhitungan skor atau pencarian domino yang bisa dimainkan dipindahkan ke kelas Game.
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

    // Mengembalikan daftar domino di tangan (untuk diinspeksi oleh kelas Game).
    public List<Domino> GetDominos()
    {
        return new List<Domino>(dominoes); // Mengembalikan salinan untuk mencegah modifikasi eksternal.
    }

    // Memeriksa apakah tangan kosong.
    public bool IsEmpty()
    {
        return dominoes.Count == 0;
    }
}

// Kelas Player mewakili seorang pemain dalam permainan.
// Logika bermain atau memeriksa domino yang bisa dimainkan dipindahkan ke kelas Game.
public class Player
{
    private string name;
    private Hand hand;
    private int score;

    public Player(string name)
    {
        this.name = name;
        this.hand = new Hand();
        this.score = 0;
    }

    // Mengembalikan nama pemain.
    public string GetName()
    {
        return name;
    }

    // Mengembalikan tangan pemain (untuk diinspeksi oleh kelas Game).
    public Hand GetHand()
    {
        return hand;
    }

    // Mengembalikan skor pemain.
    public int GetScore()
    {
        return score;
    }

    // Menambahkan poin ke skor pemain.
    public void AddScore(int points)
    {
        score += points;
    }
}

// Kelas Board mewakili area permainan tempat domino diletakkan.
// Logika penempatan domino dipindahkan ke kelas Game.
public class Board
{
    private List<Domino> playedDominos;
    private int leftEnd;
    private int rightEnd;

    public Board()
    {
        playedDominos = new List<Domino>();
        leftEnd = -1; // -1 menunjukkan papan kosong
        rightEnd = -1; // -1 menunjukkan papan kosong
    }

    // Mengembalikan nilai ujung kiri papan.
    public int GetLeftEnd()
    {
        return leftEnd;
    }

    // Mengembalikan nilai ujung kanan papan.
    public int GetRightEnd()
    {
        return rightEnd;
    }

    // Mengembalikan daftar domino yang sudah dimainkan di papan (untuk diinspeksi oleh kelas Game).
    public List<Domino> GetPlayedDominos()
    {
        return new List<Domino>(playedDominos);
    }

    // Memeriksa apakah papan kosong.
    public bool IsEmpty()
    {
        return playedDominos.Count == 0;
    }

    // Mengatur ujung kiri dan kanan papan (digunakan oleh kelas Game).
    public void SetEnds(int left, int right)
    {
        leftEnd = left;
        rightEnd = right;
    }

    // Menambahkan domino ke daftar domino yang dimainkan (digunakan oleh kelas Game).
    public void AddPlayedDomino(Domino domino, bool toLeft)
    {
        if (toLeft)
        {
            playedDominos.Insert(0, domino);
        }
        else
        {
            playedDominos.Add(domino);
        }
    }

    // Mengembalikan representasi string dari papan.
    public override string ToString()
    {
        if (IsEmpty())
        {
            return "Papan: Kosong";
        }
        return "Papan: " + string.Join("-", playedDominos.Select(d => d.ToString()));
    }
}

// Kelas Game mengelola alur permainan secara keseluruhan, menampung semua logika utama.
public class Game
{
    private List<Player> players;
    private Board board;
    private int currentPlayerIndex; // Index of the current player in the players list
    private int turnDirection; // 1 for clockwise, -1 for counter-clockwise
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

        List<Domino> allDominos = createAndShuffleAllDominos();
        dealDominosToPlayers(allDominos, initialTilesPerPlayer);
        determineStartingPlayer();

        Console.WriteLine("------------------------------------------");
    }

    // Metode internal: Membuat dan mengocok semua domino (0-0 hingga 6-6).
    private List<Domino> createAndShuffleAllDominos()
    {
        List<Domino> allDominos = new List<Domino>();
        for (int i = 0; i <= 6; i++)
        {
            for (int j = i; j <= 6; j++)
            {
                allDominos.Add(new Domino(i, j));
            }
        }
        return allDominos.OrderBy(d => random.Next()).ToList();
    }

    // Metode internal: Membagikan domino ke setiap pemain.
    private void dealDominosToPlayers(List<Domino> allDominos, int initialTilesPerPlayer)
    {
        int dominoIndex = 0;
        foreach (var player in players)
        {
            for (int i = 0; i < initialTilesPerPlayer; i++)
            {
                if (dominoIndex < allDominos.Count)
                {
                    player.GetHand().AddDomino(allDominos[dominoIndex]);
                    dominoIndex++;
                }
                else
                {
                    Console.WriteLine("Tidak cukup domino untuk dibagikan ke semua pemain secara merata.");
                    break;
                }
            }
        }

        if (dominoIndex < allDominos.Count)
        {
            Console.WriteLine($"Ada {allDominos.Count - dominoIndex} domino sisa yang tidak dibagikan.");
        }
    }

    // Metode internal: Menentukan pemain pertama (misalnya, yang memiliki double tertinggi).
    private void determineStartingPlayer()
    {
        int startingPlayerIdx = 0;
        Domino highestDouble = null;

        for (int i = 0; i < players.Count; i++)
        {
            foreach (var domino in players[i].GetHand().GetDominos())
            {
                if (domino.IsDouble())
                {
                    if (highestDouble == null || domino.End1 > highestDouble.End1)
                    {
                        highestDouble = domino;
                        startingPlayerIdx = i;
                    }
                }
            }
        }

        // Jika tidak ada double, pilih pemain pertama secara acak
        if (highestDouble == null)
        {
            startingPlayerIdx = random.Next(players.Count);
        }
        currentPlayerIndex = startingPlayerIdx;
        Console.WriteLine($"Pemain pertama adalah: {players[currentPlayerIndex].GetName()}");
    }

    // Metode internal: Menampilkan status papan.
    private void displayBoardState()
    {
        Console.WriteLine(board.ToString());
        if (!board.IsEmpty())
        {
            Console.WriteLine($"Ujung Papan: Kiri={board.GetLeftEnd()}, Kanan={board.GetRightEnd()}");
        }
    }

    // Metode internal: Menampilkan tangan pemain.
    private void displayPlayerHand(Player player)
    {
        Console.WriteLine($"{player.GetName()}, tangan Anda: {string.Join(" ", player.GetHand().GetDominos().Select(d => d.ToString()))}");
    }

    // Metode internal: Mencari domino yang dapat dimainkan dari tangan pemain berdasarkan kondisi papan.
    private Domino findPlayableDominoForPlayer(Player player)
    {
        if (board.IsEmpty())
        {
            // Jika papan kosong, domino apa pun bisa dimainkan (biasanya double tertinggi).
            // Untuk demo, kita ambil saja yang pertama di tangan.
            return player.GetHand().GetDominos().FirstOrDefault();
        }

        foreach (var domino in player.GetHand().GetDominos())
        {
            if (domino.End1 == board.GetLeftEnd() || domino.End2 == board.GetLeftEnd() ||
                domino.End1 == board.GetRightEnd() || domino.End2 == board.GetRightEnd())
            {
                return domino;
            }
        }
        return null; // Tidak ada domino yang bisa dimainkan.
    }

    // Metode internal: Mencoba menempatkan domino di papan.
    private bool attemptToPlaceDomino(Player player, Domino dominoToPlay, string end)
    {
        if (board.IsEmpty())
        {
            // Jika papan kosong, domino pertama bisa dimainkan di mana saja.
            board.AddPlayedDomino(dominoToPlay, false); // Tambahkan ke kanan secara default
            board.SetEnds(dominoToPlay.End1, dominoToPlay.End2);
            Console.WriteLine($"{player.GetName()} memainkan {dominoToPlay} di papan kosong.");
            return true;
        }

        bool placed = false;
        if (end.ToLower() == "left")
        {
            if (dominoToPlay.End1 == board.GetLeftEnd())
            {
                dominoToPlay.Rotate(); // Putar agar cocok
                board.AddPlayedDomino(dominoToPlay, true);
                board.SetEnds(dominoToPlay.End2, board.GetRightEnd());
                placed = true;
            }
            else if (dominoToPlay.End2 == board.GetLeftEnd())
            {
                board.AddPlayedDomino(dominoToPlay, true);
                board.SetEnds(dominoToPlay.End1, board.GetRightEnd());
                placed = true;
            }
        }
        else if (end.ToLower() == "right")
        {
            if (dominoToPlay.End1 == board.GetRightEnd())
            {
                board.AddPlayedDomino(dominoToPlay, false);
                board.SetEnds(board.GetLeftEnd(), dominoToPlay.End2);
                placed = true;
            }
            else if (dominoToPlay.End2 == board.GetRightEnd())
            {
                dominoToPlay.Rotate(); // Putar agar cocok
                board.AddPlayedDomino(dominoToPlay, false);
                board.SetEnds(board.GetLeftEnd(), dominoToPlay.End1);
                placed = true;
            }
        }

        if (placed)
        {
            player.GetHand().RemoveDomino(dominoToPlay);
            Console.WriteLine($"{player.GetName()} memainkan {dominoToPlay} di ujung {end}.");
        }
        else
        {
            Console.WriteLine($"Gerakan tidak valid. Domino tidak cocok atau ujung salah.");
        }
        return placed;
    }

    // Metode internal: Mengubah giliran ke pemain berikutnya.
    private void moveNextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + turnDirection + players.Count) % players.Count;
    }

    // Metode internal: Memeriksa kondisi kemenangan (pemain kehabisan domino).
    private bool checkWinCondition()
    {
        foreach (var player in players)
        {
            if (player.GetHand().IsEmpty())
            {
                Console.WriteLine($"\n!!! {player.GetName()} kehabisan domino dan memenangkan putaran ini !!!");
                // Hitung skor pemain lain dan tambahkan ke pemenang
                foreach (var otherPlayer in players)
                {
                    if (otherPlayer != player)
                    {
                        int score = calculatePlayerScore(otherPlayer);
                        player.AddScore(score);
                        Console.WriteLine($"{player.GetName()} mendapatkan {score} poin dari {otherPlayer.GetName()}.");
                    }
                }
                Console.WriteLine($"Total skor {player.GetName()} saat ini: {player.GetScore()}");
                return true;
            }
        }
        return false;
    }

    // Metode internal: Mengelola jika permainan macet (tidak ada yang bisa bermain).
    private void handleGameStalemate()
    {
        Console.WriteLine("\nPermainan macet! Tidak ada yang bisa bergerak.");
        // Setiap pemain menambahkan skor tangannya sendiri
        foreach (var player in players)
        {
            int score = calculatePlayerScore(player);
            player.AddScore(score);
            Console.WriteLine($"{player.GetName()} mendapatkan {score} poin dari tangannya sendiri.");
        }
    }

    // Metode internal: Menghitung skor domino yang tersisa di tangan pemain.
    private int calculatePlayerScore(Player player)
    {
        return player.GetHand().GetDominos().Sum(d => d.End1 + d.End2);
    }

    // Metode utama untuk menjalankan satu putaran permainan.
    public void PlayRound()
    {
        bool roundOver = false;
        int consecutivePasses = 0; // Untuk mendeteksi jika permainan macet

        while (!roundOver)
        {
            Player currentPlayer = players[currentPlayerIndex];
            Console.WriteLine($"\n--- Giliran {currentPlayer.GetName()} ---");
            displayBoardState();
            displayPlayerHand(currentPlayer);

            Domino playableDomino = findPlayableDominoForPlayer(currentPlayer);

            if (playableDomino != null)
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
                        Domino actualDominoInHand = currentPlayer.GetHand().GetDominos()
                            .FirstOrDefault(d => d.Equals(chosenDomino));

                        if (actualDominoInHand == null)
                        {
                            Console.WriteLine("Domino tidak ada di tangan Anda. Coba lagi.");
                            continue;
                        }

                        string endToPlay = "";
                        if (!board.IsEmpty())
                        {
                            Console.Write("Mainkan di ujung mana? (left/right): ");
                            endToPlay = Console.ReadLine();
                        } else {
                            endToPlay = "right"; // Default untuk domino pertama
                        }

                        validMove = attemptToPlaceDomino(currentPlayer, actualDominoInHand, endToPlay);

                        if (!validMove)
                        {
                            Console.WriteLine("Gerakan tidak valid. Coba lagi.");
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
                Console.WriteLine($"{currentPlayer.GetName()} tidak memiliki domino yang bisa dimainkan. Melewatkan giliran.");
                consecutivePasses++;
            }

            roundOver = checkWinCondition();

            // Jika semua pemain melewati giliran secara berurutan, permainan macet.
            if (!roundOver && consecutivePasses >= players.Count)
            {
                handleGameStalemate();
                roundOver = true;
            }

            if (!roundOver)
            {
                moveNextTurn();
            }
        }
    }

    // Mengakhiri permainan (opsional, untuk multi-putaran).
    public void EndGame()
    {
        Console.WriteLine("\nPermainan berakhir.");
        Console.WriteLine("Skor Akhir:");
        foreach (var player in players.OrderByDescending(p => p.GetScore()))
        {
            Console.WriteLine($"- {player.GetName()}: {player.GetScore()} poin");
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
