namespace LudoBoard
{
    public class SimulationBoard {
    public static void DisplayBoard() {
        int[,] board = new int[15, 15];
        // Console.ForegroundColor = ConsoleColor.White;

        // for (int i = 0; i < board.GetLength(0); i++) {
        //     for (int j = 0; j < board.GetLength(1); j++) {
        //         Console.Write($" ({i},{j}) ");
        //     }
        //     Console.WriteLine();
        // }

        Console.ResetColor();

        for (int i = 0; i < board.GetLength(0); i++) {
            for (int j = 0; j < board.GetLength(1); j++) {

                Console.ResetColor();

                if ((i == 6 && j == 6) || (i == 6 && j == 8) || (i == 8 && j == 8) || (i == 8 && j == 6) || (i == 7 && j == 7)) {
                    Console.Write(" * ");
                    continue;
                }

                // === SAFE ZONE (Eksplisit) ===
                if (
                    (i == 8 && j == 2) ||
                    (i == 2 && j == 6) ||
                    (i == 6 && j == 12) ||
                    (i == 12 && j == 8)
                ) {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" ★ ");
                    continue;
                }

                // === BASE ZONES ===
                if (i >= 0 && i <= 5 && j >= 0 && j <= 5) {
                    Console.ForegroundColor = ConsoleColor.Red; // Base Merah
                } else if (i >= 0 && i <= 5 && j >= 9 && j <= 14) {
                    Console.ForegroundColor = ConsoleColor.Green; // Base Hijau
                } else if (i >= 9 && i <= 14 && j >= 0 && j <= 5) {
                    Console.ForegroundColor = ConsoleColor.Blue; // Base Biru
                } else if (i >= 9 && i <= 14 && j >= 9 && j <= 14) {
                    Console.ForegroundColor = ConsoleColor.Yellow; // Base Kuning
                }

                  // === HOME POINTS ===
                  else if (i == 7 && j == 6) {
                    Console.ForegroundColor = ConsoleColor.Red; // Home Merah
                } else if (i == 6 && j == 7) {
                    Console.ForegroundColor = ConsoleColor.Green; // Home Hijau
                } else if (i == 7 && j == 8) {
                    Console.ForegroundColor = ConsoleColor.Yellow; // Home Kuning
                } else if (i == 8 && j == 7) {
                    Console.ForegroundColor = ConsoleColor.Blue; // Home Biru
                }

                  // === START POINTS ===
                  else if (i == 6 && j == 1) {
                    Console.ForegroundColor = ConsoleColor.Red; // Start Merah
                } else if (i == 1 && j == 8) {
                    Console.ForegroundColor = ConsoleColor.Green; // Start Hijau
                } else if (i == 8 && j == 13) {
                    Console.ForegroundColor = ConsoleColor.Yellow; // Start Kuning
                } else if (i == 13 && j == 6) {
                    Console.ForegroundColor = ConsoleColor.Blue; // Start Biru
                }

                  // === HOME PATHS ===
                  else if (i == 7 && j >= 1 && j <= 5) {
                    Console.ForegroundColor = ConsoleColor.Red; // Path Merah
                } else if (j == 7 && i >= 1 && i <= 5) {
                    Console.ForegroundColor = ConsoleColor.Green; // Path Hijau
                } else if (i == 7 && j >= 9 && j <= 13) {
                    Console.ForegroundColor = ConsoleColor.Yellow; // Path Kuning
                } else if (j == 7 && i >= 9 && i <= 13) {
                    Console.ForegroundColor = ConsoleColor.Blue; // Path Biru
                } else {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(" ■ ");
                // Console.Write($" ({i},{j}) ");
            }
            Console.WriteLine();
        }

    }
}
}