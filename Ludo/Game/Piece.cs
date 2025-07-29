using Ludo.Enum;
using Ludo.interfaceX;
namespace Ludo.Game;

public class Piece :

{
    public Color PieceColor { get; }

    public IPlayer PlayerOwner { get; }

    public PieceState State { get; set; }
    public int StepIndex { get; set; }
    public Piece(IPlayer ownerPlayer, Color pieceColor)
    {
        PieceColor = pieceColor;
        PlayerOwner = PlayerOwner;
    }
}
