using System;
using System.Collections.Generic;
using System.Linq;

namespace LudoGame
{
    // --- Supporting Types ---

    /// <summary>
    /// Represents a position on the game board.
    /// </summary>
    public record struct Position(int X, int Y);

    /// <summary>
    /// Represents the state of a game piece.
    /// </summary>
    public enum PieceState
    {
        AtBase,
        Active,
        Home
    }

    /// <summary>
    /// Represents different types of zones on the game board.
    /// </summary>
    public enum ZoneType
    {
        Base,
        StartPoint,
        CommonPath,
        HomePath,
        HomePoint,
        SafeZone,
        BlockedPath, // Not explicitly used in logic but good to have
        Empty
    }

    /// <summary>
    /// Represents the colors for players and pieces.
    /// </summary>
    public enum Color
    {
        BLUE,
        RED,
        GREEN,
        YELLOW
    }

    // --- IPlayer & Player ---

    /// <summary>
    /// Interface for a game player.
    /// </summary>
    public interface IPlayer
    {
        string Name { get; }
        Color Color { get; }
    }

    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    public class Player : IPlayer
    {
        public string Name { get; }
        public Color Color { get; }

        public Player(string name, Color color)
        {
            Name = name;
            Color = color;
        }
    }

    // --- IPiece & Piece ---

    /// <summary>
    /// Interface for a game piece.
    /// </summary>
    public interface IPiece
    {
        Color PieceColor { get; }
        IPlayer PlayerOwner { get; }
        PieceState State { get; set; }
        int StepIndex { get; set; } // Current step on the path, -1 for base/home
    }

    /// <summary>
    /// Represents a game piece.
    /// </summary>
    public class Piece : IPiece
    {
        public Color PieceColor { get; }
        public IPlayer PlayerOwner { get; }
        public PieceState State { get; set; }
        public int StepIndex { get; set; } // -1 indicates at base or home

        public Piece(Color pieceColor, IPlayer playerOwner)
        {
            PieceColor = pieceColor;
            PlayerOwner = playerOwner;
            State = PieceState.AtBase;
            StepIndex = -1; // Initially at base
        }
    }

    // --- IDice & Dice ---

    /// <summary>
    /// Interface for a dice roller.
    /// </summary>
    public interface IDice
    {
        int Roll();
    }

    /// <summary>
    /// Represents a dice for rolling numbers.
    /// </summary>
    public class Dice : IDice
    {
        private readonly Random _random;

        public Dice()
        {
            _random = new Random();
        }

        public int Roll()
        {
            return _random.Next(1, 7); // Rolls a number between 1 and 6
        }
    }

    // --- IBoard & Board ---

    /// <summary>
    /// Interface for the game board.
    /// </summary>
    public interface IBoard
    {
        Piece?[,] Grid { get; }
    }

    /// <summary>
    /// Represents the game board.
    /// </summary>
    public class Board : IBoard
    {
        public Piece?[,] Grid { get; }

        // A very simplified board representation.
        // A real Ludo board would have specific paths and zones.
        private const int BOARD_SIZE = 15; // Example size

        public Board()
        {
            Grid = new Piece?[BOARD_SIZE, BOARD_SIZE];
            // In a real Ludo game, you'd initialize specific positions for start, home, safe zones, etc.
        }

        // Example method to place a piece on the grid (for visualization, not game logic)
        public void PlacePiece(Piece piece, Position position)
        {
            if (position.X >= 0 && position.X < BOARD_SIZE &&
                position.Y >= 0 && position.Y < BOARD_SIZE)
            {
                Grid[position.X, position.Y] = piece;
            }
        }

        // Example method to remove a piece (e.g., when moving)
        public void RemovePiece(Position position)
        {
            if (position.X >= 0 && position.X < BOARD_SIZE &&
                position.Y >= 0 && position.Y < BOARD_SIZE)
            {
                Grid[position.X, position.Y] = null;
            }
        }
    }

    // --- GameController ---

    /// <summary>
    /// Manages the game flow and logic.
    /// </summary>
    public class GameController
    {
        private readonly Dictionary<IPlayer, List<IPiece>> _playerPieces;
        private readonly Dictionary<Color, List<Position>> _playerPaths;
        private readonly Dictionary<Position, ZoneType> _zoneMap;
        private readonly IDice _dice;
        private readonly List<IPlayer> _players;
        private readonly IBoard _board;
        private int _currentTurnIndex;

        public event Action OnGameStart;

        public GameController(IPlayer player1, IPlayer player2, IPlayer player3, IPlayer player4, IDice dice, IBoard board)
        {
            _players = new List<IPlayer> { player1, player2, player3, player4 };
            _dice = dice;
            _board = board;
            _playerPieces = new Dictionary<IPlayer, List<IPiece>>();
            _playerPaths = new Dictionary<Color, List<Position>>();
            _zoneMap = new Dictionary<Position, ZoneType>();

            InitializeGameComponents();
        }

        private void InitializeGameComponents()
        {
            // Initialize pieces for each player
            foreach (var player in _players)
            {
                _playerPieces[player] = new List<IPiece>();
                for (int i = 0; i < 4; i++) // Assuming 4 pieces per player
                {
                    _playerPieces[player].Add(new Piece(player.Color, player));
                }
            }

            // Initialize player paths (simplified for this example)
            // In a real Ludo game, these paths would be complex and specific to the board layout.
            // For demonstration, let's create a generic path.
            foreach (Color color in Enum.GetValues(typeof(Color)))
            {
                _playerPaths[color] = new List<Position>();
                // Example: A simple linear path for demonstration
                for (int i = 0; i < 52; i++) // 52 steps in a typical Ludo common path
                {
                    _playerPaths[color].Add(new Position(i, 0)); // Dummy positions
                }
                // Add home path positions (e.g., last 6 steps before home point)
                for (int i = 0; i < 6; i++)
                {
                    _playerPaths[color].Add(new Position(52 + i, 0)); // Dummy positions for home path
                }
                _playerPaths[color].Add(new Position(58, 0)); // Home point
            }

            // Initialize zone map (simplified)
            // Map specific positions to their zone types (e.g., start points, safe zones, home paths)
            // This would be highly dependent on your board design.
            // For example: _zoneMap[new Position(0,0)] = ZoneType.StartPoint;
            //             _zoneMap[new Position(10,0)] = ZoneType.SafeZone;
        }

        public void StartGame()
        {
            Console.WriteLine("Game Started!");
            _currentTurnIndex = 0;
            OnGameStart?.Invoke(); // Raise the game start event

            // Example game loop (very basic for demonstration)
            // In a real game, this would be part of a continuous game loop
            // with user input and complex turn management.
            for (int i = 0; i < 5; i++) // Simulate 5 turns
            {
                Console.WriteLine($"--- Turn {i + 1} ---");
                IPlayer currentPlayer = GetCurrentPlayer();
                Console.WriteLine($"It's {currentPlayer.Name}'s ({currentPlayer.Color}) turn.");

                int roll = RollDice();
                Console.WriteLine($"{currentPlayer.Name} rolled a {roll}.");

                // Basic piece movement simulation
                // In a real game, the player would choose which piece to move
                // and a comprehensive check for valid moves would occur.
                var movablePieces = _playerPieces[currentPlayer].Where(p => CanMove(p, roll)).ToList();

                if (movablePieces.Any())
                {
                    Piece pieceToMove = (Piece)movablePieces.First(); // Just move the first movable piece
                    Console.WriteLine($"Attempting to move {pieceToMove.PieceColor} piece. Current state: {pieceToMove.State}, Step: {pieceToMove.StepIndex}");

                    // Example: Try to move a piece from base if 6 is rolled
                    if (pieceToMove.State == PieceState.AtBase && roll == 6 && CanEnterFromBase(pieceToMove, roll))
                    {
                        Console.WriteLine($"Piece {pieceToMove.PieceColor} can enter from base.");
                        MovePiece(pieceToMove, -1, 0); // Move from base to start (StepIndex 0)
                        Console.WriteLine($"Piece {pieceToMove.PieceColor} moved to active state at step 0.");
                    }
                    else if (pieceToMove.State == PieceState.Active)
                    {
                        // Simulate moving an active piece
                        int currentStep = pieceToMove.StepIndex;
                        if (MovePiece(pieceToMove, currentStep, currentStep + roll))
                        {
                            Console.WriteLine($"Piece {pieceToMove.PieceColor} moved from step {currentStep} to {pieceToMove.StepIndex}.");
                            CheckWin(currentPlayer);
                            CaptureIfExists();
                        }
                        else
                        {
                            Console.WriteLine($"Piece {pieceToMove.PieceColor} cannot move {roll} steps.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{currentPlayer.Name} has no valid moves.");
                }

                NextTurn();
            }
            Console.WriteLine("Game simulation finished.");
        }

        public int RollDice()
        {
            return _dice.Roll();
        }

        public IPlayer GetCurrentPlayer()
        {
            return _players[_currentTurnIndex];
        }

        public void NextTurn()
        {
            _currentTurnIndex = (_currentTurnIndex + 1) % _players.Count;
        }

        public ZoneType GetZoneTypeForPiece(Piece piece)
        {
            // This is a placeholder. In a real game, you would determine the zone type
            // based on the piece's current position (StepIndex) on its path.
            if (piece.State == PieceState.AtBase) return ZoneType.Base;
            if (piece.State == PieceState.Home) return ZoneType.HomePoint;

            // Example: If piece is on the common path (first 52 steps in our simplified path)
            if (piece.StepIndex >= 0 && piece.StepIndex < 52)
            {
                // You'd need a more robust way to map StepIndex to a board Position and then to ZoneType.
                // For simplicity, let's assume specific step indices correspond to specific zones.
                // E.g., if step index 0 is a start point for a player.
                return ZoneType.CommonPath; // Or StartPoint, SafeZone based on actual step
            }
            // If piece is on its home path
            if (piece.StepIndex >= 52 && piece.StepIndex < _playerPaths[piece.PieceColor].Count - 1)
            {
                return ZoneType.HomePath;
            }
            // If piece is at the home point
            if (piece.StepIndex == _playerPaths[piece.PieceColor].Count - 1)
            {
                return ZoneType.HomePoint;
            }

            return ZoneType.Empty; // Default or unknown
        }

        public bool CanMove(IPiece piece, int steps)
        {
            // Simplified logic:
            // 1. If at base, needs a 6 to move out.
            // 2. If active, check if it can move 'steps' without overshooting home.
            if (piece.State == PieceState.AtBase)
            {
                return steps == 6; // Needs a 6 to move out of base
            }
            else if (piece.State == PieceState.Active)
            {
                List<Position> playerPath = _playerPaths[piece.PieceColor];
                return (piece.StepIndex + steps) < playerPath.Count; // Can't overshoot the end of the path
            }
            return false;
        }

        /// <summary>
        /// Moves a piece from an old step index to a new step index.
        /// This method encapsulates the state changes of the piece.
        /// </summary>
        /// <param name="piece">The piece to move.</param>
        /// <param name="oldStepIndex">The current step index of the piece (-1 if at base).</param>
        /// <param name="newStepIndex">The target step index for the piece.</param>
        /// <returns>True if the move was successful, false otherwise.</returns>
        public bool MovePiece(Piece piece, int oldStepIndex, int newStepIndex)
        {
            List<Position> playerPath = _playerPaths[piece.PieceColor];

            if (piece.State == PieceState.AtBase && newStepIndex == 0) // Moving from base to start
            {
                piece.State = PieceState.Active;
                piece.StepIndex = newStepIndex;
                // You would update the board's Grid here: _board.PlacePiece(piece, playerPath[newStepIndex]);
                return true;
            }
            else if (piece.State == PieceState.Active)
            {
                if (newStepIndex < playerPath.Count) // Ensure it doesn't go beyond the path
                {
                    piece.StepIndex = newStepIndex;
                    // Check if it reached home
                    if (piece.StepIndex == playerPath.Count - 1) // Assuming last step is home point
                    {
                        piece.State = PieceState.Home;
                    }
                    // Update board: _board.RemovePiece(playerPath[oldStepIndex]);
                    //              _board.PlacePiece(piece, playerPath[newStepIndex]);
                    return true;
                }
            }
            return false; // Invalid move
        }

        public void CheckWin(IPlayer player)
        {
            // A player wins if all their pieces are in the 'Home' state.
            bool allPiecesHome = _playerPieces[player].All(p => p.State == PieceState.Home);
            if (allPiecesHome)
            {
                Console.WriteLine($"{player.Name} has won the game!");
                // Here you would typically end the game or remove the player from active turns.
            }
        }

        public void CaptureIfExists()
        {
            // This is where the logic for capturing opponent pieces would go.
            // When a piece lands on a square occupied by an opponent's piece (and it's not a safe zone),
            // the opponent's piece is sent back to its base.
            // This would involve iterating through the current positions of active pieces and checking for collisions.
            Console.WriteLine("Checking for captures (logic not implemented in detail).");
        }

        public bool CanEnterFromBase(Piece piece, int roll)
        {
            // A piece can enter from base if a 6 is rolled.
            // Also needs to check if the start point is not blocked by own piece.
            if (piece.State == PieceState.AtBase && roll == 6)
            {
                // In a real game, you would check if the player's start position is free
                // _playerPaths[piece.PieceColor][0] would be the start position.
                // For now, assume it's always possible.
                return true;
            }
            return false;
        }

        public List<Position> GetPathForPlayer(Color color)
        {
            return _playerPaths.GetValueOrDefault(color);
        }

        public int GetPiecePathIndex(Piece piece)
        {
            // Returns the current step index of the piece on its path.
            // Returns -1 if at base or home, or if the piece is not found/active on a path.
            if (piece.State == PieceState.Active || piece.State == PieceState.Home)
            {
                return piece.StepIndex;
            }
            return -1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create players
            IPlayer player1 = new Player("Alice", Color.RED);
            IPlayer player2 = new Player("Bob", Color.BLUE);
            IPlayer player3 = new Player("Charlie", Color.GREEN);
            IPlayer player4 = new Player("Diana", Color.YELLOW);

            // Create dice and board
            IDice dice = new Dice();
            IBoard board = new Board();

            // Create game controller
            GameController gameController = new GameController(player1, player2, player3, player4, dice, board);

            // Subscribe to game start event
            gameController.OnGameStart += () => Console.WriteLine("Game is officially starting!");

            // Start the game simulation
            gameController.StartGame();

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}