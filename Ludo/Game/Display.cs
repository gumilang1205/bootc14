using Ludo.Game;
using Ludo.interfaceX;
using Ludo.Enum;
namespace Ludo.Game
{
    public class Display
    {
        private GameController _gameController;

        public Display(GameController gameController)
        {
            _gameController = gameController;
        }
        public void StartGame()
        {
            bool gameOver = false;
            while (!gameOver)
            {
                Console.Clear();
                DrawBoard();

                var currentPlayer = _gameController.GetCurrentPlayer();
                Console.WriteLine($"\nGiliran: {currentPlayer.Name} ({_gameController.ColorToString(currentPlayer.Color)})");

                Console.WriteLine("Tekan ENTER untuk melempar dadu...");
                Console.ReadLine();
                int roll = _gameController.RollDice();
                Console.WriteLine($"Kamu melempar: {roll}");

                bool movedPiece = false;
                bool gotBonusTurn = false;

                var pieces = _gameController.GetPlayerPieces[currentPlayer];
                var activePieces = pieces.Where(p => p.State == PieceState.Active).ToList();
                var atBasePieces = pieces.Where(p => p.State == PieceState.AtBase).ToList();

                List<IPiece> movablePieces = new List<IPiece>();

                if (roll == 6 && atBasePieces.Any()) 
                {
                    Console.WriteLine("Pilihan pergerakan:");
                    Console.WriteLine($"1. Keluarkan bidak dari Base (Bidak {_gameController.ColorToString(atBasePieces.First().PieceColor)} pertama)");

                    int optionCounter = 2;
                    foreach (var piece in activePieces)
                    {
                        if (_gameController.CanMove(piece, roll))
                        {
                            movablePieces.Add(piece);
                            Console.WriteLine($"{optionCounter}. Pindahkan bidak {_gameController.ColorToString(piece.PieceColor)} di langkah {piece.StepIndex + 1}");
                            optionCounter++;
                        }
                    }

                    int choice = -1; 
                    while (choice == -1) 
                    {
                        Console.Write("Masukkan nomor pilihan: ");
                        if (int.TryParse(Console.ReadLine(), out int input))
                        {
                            if (input == 1)
                            {
                                var chosenPiece = atBasePieces.First();
                                _gameController.MovePieceFromBase(chosenPiece);
                                Console.WriteLine($"Bidak {_gameController.ColorToString(chosenPiece.PieceColor)} keluar dari base ke titik start.");
                                movedPiece = true;
                                gotBonusTurn = true;
                                break;
                            }
                            else if (input > 1 && input <= movablePieces.Count + 1)
                            {
                                var chosenPiece = movablePieces[input - 2];
                                if (_gameController.MovePiece(chosenPiece, roll))
                                {
                                    movedPiece = true;
                                    if (chosenPiece.State == PieceState.Active)
                                    {
                                        var currentPos = _gameController.GetPathForPlayer(chosenPiece.PieceColor)[chosenPiece.StepIndex];
                                        gotBonusTurn = _gameController.CaptureIfExists(currentPlayer, currentPos);
                                    }
                                }
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Masukan tidak valid. Harap masukkan angka.");
                        }
                    }
                }
                else 
                {
                    movablePieces.AddRange(activePieces.Where(p => _gameController.CanMove(p, roll)));

                    if (!movablePieces.Any())
                    {
                        Console.WriteLine("Tidak ada bidak yang bisa digerakkan. Giliran dilewati.");
                        _gameController.NextTurn();
                        Console.WriteLine("\nTekan ENTER untuk lanjut...");
                        Console.ReadLine();
                        continue;
                    }

                    Console.WriteLine("Pilih bidak yang akan digerakkan:");
                    for (int i = 0; i < movablePieces.Count; i++)
                    {
                        string pieceStatus = movablePieces[i].State == PieceState.AtBase ? "di Base" : $"di langkah {movablePieces[i].StepIndex + 1}";
                        Console.WriteLine($"{i + 1}. Bidak {_gameController.ColorToString(movablePieces[i].PieceColor)} {pieceStatus}");
                    }

                    int choiceIndex = -1;
                    while (choiceIndex == -1)
                    {
                        Console.Write("Masukkan nomor bidak: ");
                        if (int.TryParse(Console.ReadLine(), out int input) && input >= 1 && input <= movablePieces.Count)
                        {
                            choiceIndex = input - 1;
                        }
                        else
                        {
                            Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.");
                        }
                    }

                    var chosenPiece = movablePieces[choiceIndex];

                    if (_gameController.MovePiece(chosenPiece, roll))
                    {
                        movedPiece = true;
                        if (chosenPiece.State == PieceState.Active)
                        {
                            var currentPos = _gameController.GetPathForPlayer(chosenPiece.PieceColor)[chosenPiece.StepIndex];
                            gotBonusTurn = _gameController.CaptureIfExists(currentPlayer, currentPos);
                        }
                    }
                }

                if (_gameController.CheckWin(currentPlayer))
                {
                    Console.Clear();
                    DrawBoard();
                    Console.WriteLine($"\n=== SELAMAT! {currentPlayer.Name} MENANG!!! ===");
                    gameOver = true;
                    break;
                }

                if (roll == 6 || gotBonusTurn)
                {
                    Console.WriteLine("Kamu dapat bonus giliran!");
                }
                else
                {
                    _gameController.NextTurn();
                }

                Console.WriteLine("\nTekan ENTER untuk lanjut...");
                Console.ReadLine();
            }
        }
        public void DrawBoard()
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("                Papan Ludo                ");
            Console.WriteLine("------------------------------------------");

            char[,] display = new char[15, 15];

            for (int y = 0; y < 15; y++)
            {
                for (int x = 0; x < 15; x++)
                {
                    ZoneType zone = _gameController.GetBoard().GetZoneType(x, y);
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
            foreach (var kvp in _gameController.GetPlayerPieces) 
            {
                var pieces = kvp.Value;

                foreach (var piece in pieces)
                {
                    if (piece.State == PieceState.Active)
                    {
                        var path = _gameController.GetPathForPlayer(piece.PieceColor);
                        if (piece.StepIndex >= 0 && piece.StepIndex < path.Count)
                        {
                            var pos = path[piece.StepIndex];
                            display[pos.X, pos.Y] = _gameController.GetPieceChar(piece.PieceColor);
                        }
                    }
                    else if (piece.State == PieceState.AtBase)
                    {
                        if (piece.BaseIndex != -1 && piece.BaseIndex < _gameController.GetBasePositions[piece.PieceColor].Count)
                        {
                            var pos = _gameController.GetBasePositions[piece.PieceColor][piece.BaseIndex];
                            display[pos.X, pos.Y] = _gameController.GetPieceChar(piece.PieceColor);
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
                        ZoneType zone = _gameController.GetBoard().GetZoneType(x, y);
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
}
