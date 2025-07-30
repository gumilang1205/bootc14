using Ludo.Enum;
using Ludo.interfaceX;
namespace Ludo.Game;

public class Piece : IPiece

{
    public Color PieceColor { get; }

    public IPlayer PlayerOwner { get; }

    public PieceState State { get; set; }
    public int StepIndex { get; set; }

    Color IPiece.PieceColor => throw new NotImplementedException();

    public Piece(IPlayer ownerPlayer, Color pieceColor)
    {
        PieceColor = pieceColor;
        PlayerOwner = ownerPlayer;
    }
}
