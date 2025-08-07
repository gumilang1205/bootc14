
using Ludo.Enum;
using Ludo.interfaceX;
using Ludo.Game;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Selamat datang di permainan Ludo!");
        Console.Write("Masukkan jumlah poemain (2-4) :");
        int playerCount;
        while (!int.TryParse(Console.ReadLine(), out playerCount) || playerCount < 2 || playerCount > 4)
        {
            Console.Write("Jumlah pemain tidak valid. Masukkan angka antara 2-4:");
        }
        List<IPlayer> players = new List<IPlayer>();
        List<LudoColor> availableColors = new List<LudoColor> { LudoColor.Red, LudoColor.Yellow, LudoColor.Green, LudoColor.Blue };
        for (int i = 0; i < playerCount; i++)
        {
            Console.Write($"Masukkan nama pemain {i + 1} :");
            string playerName = Console.ReadLine();
            LudoColor playerColor = availableColors[i];
            players.Add(new Player(playerName, playerColor));
        }

        IDice dice = new Dice();
        IBoard board = new Board();

        GameController controller = new GameController(players, dice, board);
        controller.OnLogMessage += Console.WriteLine;

        Display display = new Display(controller);
        display.StartGame();
    }
}
