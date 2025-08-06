
using Ludo.Enum;
using Ludo.interfaceX;
using Ludo.Game;

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

        controller.OnGameStart += () => Console.WriteLine("Selamat datang di permainan Ludo!");
        controller.StartGame();
    }
}
