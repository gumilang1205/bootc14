using Ludo.Game;
using Ludo.interfaceX;
using NUnit.Framework;

namespace Ludo.Tests;

[TestFixture]
public class Ludo_IsLudoShould
{
    private GameController _game;
    private IBoard _board;
    private IDice _dice;
    [SetUp]
    public void Setup()
    {
        _board = new Board();
        _dice = new Dice();
        var player1 = new Player("a", Enum.LudoColor.Blue);
        var player2 = new Player("b", Enum.LudoColor.Red);
        List<IPlayer> players = [player1, player2];
        _game = new GameController(players, _dice, _board);
    }

    [Test]
    public void IsSafeZone_Input_ReturnTrue()
    {
        var result = _game.IsSafeZone(1, 2);
        Assert.That(result, Is.False);
    }
    [Test]
    public void IsBlocked_Input_ReturnFalse()
    {
        var result1 = _game.IsBlocked(1, 2);
        Assert.That(result1, Is.False);
    }
}