using Ludo.Enum;
using Ludo.interfaceX;
namespace Ludo.Game;

public class Piece : IPiece

{
    public LudoColor PieceColor { get; }
<<<<<<< HEAD
=======

>>>>>>> a3ae4f7bbf94ed3deb635d66bd4ea83147229243
    public IPlayer PlayerOwner { get; }
    public PieceState State { get; set; }
    public int StepIndex { get; set; }

<<<<<<< HEAD
    public Piece(LudoColor pieceColor,IPlayer ownerPlayer)
=======
    LudoColor IPiece.PieceColor => throw new NotImplementedException();

    public Piece(IPlayer ownerPlayer, LudoColor pieceColor)
>>>>>>> a3ae4f7bbf94ed3deb635d66bd4ea83147229243
    {
        PieceColor = pieceColor;
        PlayerOwner = ownerPlayer;
        State = PieceState.AtBase;
        StepIndex = 0;
    }
}
