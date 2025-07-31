using System;
using System.Collections.Generic;
namespace LudoBoard
{
    public enum Color
    {
        Red,
        Green,
        Blue,
        Yellow
    }

    public class Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Board
    {
        public const int Size = 15;
        private char[,] grid = new char[Size, Size];

        public Board()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Isi semua dengan spasi
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    grid[i, j] = ' ';

            // Tandai area rumah pemain
            FillArea(0, 0, 6, 6, 'R');      // Red home
            FillArea(0, 9, 6, 15, 'G');     // Green home
            FillArea(9, 0, 15, 6, 'B');     // Blue home
            FillArea(9, 9, 15, 15, 'Y');    // Yellow home

            // Tandai jalur bintang sebagai path
            DrawPath();
        }

        private void FillArea(int startX, int startY, int endX, int endY, char c)
        {
            for (int i = startX; i < endX; i++)
                for (int j = startY; j < endY; j++)
                    grid[i, j] = c;
        }

        private void DrawPath()
        {
            // Jalur horizontal tengah
            for (int i = 6; i < 9; i++)
                for (int j = 0; j < Size; j++)
                    grid[i, j] = '*';

            // Jalur vertikal tengah
            for (int i = 0; i < Size; i++)
                for (int j = 6; j < 9; j++)
                    grid[i, j] = '*';
        }

        public void Display()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Board ludoBoard = new Board();
            ludoBoard.Display();

            Console.WriteLine("\nPapan Ludo berhasil dibuat!");
            Console.ReadLine();
            SimulationBoard.DisplayBoard();
        }
    }
}