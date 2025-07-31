using Ludo.Enum;
using Ludo.interfaceX;
namespace Ludo.Game;

public class Piece : IPiece

{
    public LudoColor PieceColor { get; }

    public IPlayer PlayerOwner { get; }

    public PieceState State { get; set; }
    public int StepIndex { get; set; }

    LudoColor IPiece.PieceColor => throw new NotImplementedException();

    public Piece(IPlayer ownerPlayer, LudoColor pieceColor)
    {
        PieceColor = pieceColor;
        PlayerOwner = ownerPlayer;
    }
}
