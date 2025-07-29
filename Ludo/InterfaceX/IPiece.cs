using System.Drawing;
using System.Security;
using Ludo.Enum;

namespace Ludo.interfaceX;

public interface IPiece
{
    Color PieceColor { get; }
    IPlayer PlayerOwner { get; }
    PieceState State { get; set; }
    int StepIndex { get; set; }

}