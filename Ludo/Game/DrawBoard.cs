using Ludo.Enum;

public class DrawBoard
{
    public void DrawBoard()
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("             Papan Ludo                 ");
        Console.WriteLine("------------------------------------------");

        char[,] display = new char[15, 15];

        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 15; x++)
            {
                ZoneType zone = _board.GetZoneType(x, y);
                switch (zone)
                {
                    case ZoneType.Base: display[x, y] = '#'; break;
                    case ZoneType.StartPoint: display[x, y] = 'S'; break;
                    case ZoneType.SafeZone: display[x, y] = '*'; break;
                    case ZoneType.HomePath: display[x, y] = '='; break;
                    case ZoneType.HomePoint: display[x, y] = 'H'; break;
                    case ZoneType.CommonPath: display[x, y] = '-'; break;
                    default: display[x, y] = 'X'; break;
                }
            }
        }
        foreach (var kvp in _playerPieces)
        {
            var pieces = kvp.Value;

            foreach (var piece in pieces)
            {
                if (piece.State == PieceState.Active)
                {
                    var path = GetPathForPlayer(piece.PieceColor);
                    if (piece.StepIndex >= 0 && piece.StepIndex < path.Count)
                    {
                        var pos = path[piece.StepIndex];
                        display[pos.X, pos.Y] = GetPieceChar(piece.PieceColor);
                    }
                }
                else if (piece.State == PieceState.AtBase)
                {
                    if (piece.BaseIndex != -1 && piece.BaseIndex < _basePositions[piece.PieceColor].Count)
                    {
                        var pos = _basePositions[piece.PieceColor][piece.BaseIndex];
                        display[pos.X, pos.Y] = GetPieceChar(piece.PieceColor);
                    }
                }
            }
        }
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 15; x++)
            {
                ConsoleColor originalColor = Console.ForegroundColor;
                char charToDisplay = display[x, y];

                if (charToDisplay == 'R') Console.ForegroundColor = ConsoleColor.Red;
                else if (charToDisplay == 'Y') Console.ForegroundColor = ConsoleColor.Yellow;
                else if (charToDisplay == 'G') Console.ForegroundColor = ConsoleColor.Green;
                else if (charToDisplay == 'B') Console.ForegroundColor = ConsoleColor.Blue;
                else
                {
                    ZoneType zone = _board.GetZoneType(x, y);
                    switch (zone)
                    {
                        case ZoneType.Base: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                        case ZoneType.StartPoint: Console.ForegroundColor = ConsoleColor.DarkBlue; break;
                        case ZoneType.SafeZone: Console.ForegroundColor = ConsoleColor.Cyan; break;
                        case ZoneType.HomePath: Console.ForegroundColor = ConsoleColor.Magenta; break;
                        case ZoneType.HomePoint: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                        case ZoneType.CommonPath: Console.ForegroundColor = ConsoleColor.Gray; break;
                        default: Console.ForegroundColor = ConsoleColor.White; break;
                    }
                }
                Console.Write(charToDisplay + " ");
                Console.ForegroundColor = originalColor;
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nKeterangan:");
        Console.ForegroundColor = ConsoleColor.Red; Console.Write("R ");
        Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("Y ");
        Console.ForegroundColor = ConsoleColor.Green; Console.Write("G ");
        Console.ForegroundColor = ConsoleColor.Blue; Console.Write("B ");
        Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Bidak pemain");
        Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("# ");
        Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Base");
        Console.ForegroundColor = ConsoleColor.DarkCyan; Console.Write("S ");
        Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Start Point");
        Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("* ");
        Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Safe Zone");
        Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("= ");
        Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Home Path");
        Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write("H ");
        Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Home Point (Tujuan)");
        Console.ForegroundColor = ConsoleColor.Gray; Console.Write("- ");
        Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("= Common Path");
    }

}
