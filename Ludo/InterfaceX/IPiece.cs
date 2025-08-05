using Ludo.Enum;

namespace Ludo.interfaceX
{
    public interface IPiece
    {
        LudoColor PieceColor { get; }
        IPlayer PlayerOwner { get; }
        PieceState State { get; set; }
        int StepIndex { get; set; }
        int BaseIndex { get; set; }

    }
}
