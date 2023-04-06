using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using CellSim;
namespace CellSim
{
    public class Cell
    {
         public static readonly string[] alphabet =
            {
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
                "v", "w", "x", "y", "z"
            };

        private readonly Random _random = new();

        public Cell()
        {
            X = 50;
            Y = 50;
            IsAlive = true;
            Mutations = new List<Mutation>();
            StepsSinceLastCollision = 0;
            Trail = new List<(int X, int Y)>();
        }

        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }
        public List<Mutation> Mutations { get; set; }
        public string CellForm { get; set; }
        public List<(int X, int Y)> Trail { get; set; }
        public int StepsSinceLastCollision { get; set; }
        public int Age { get; set; }
        int collisonPeriode = 100;
        public ConsoleColor CellColor { get; set; }

        public void Mutate()
        {
            Mutations.Add(new Mutation());
        }

        public void Die()
        {
            IsAlive = false;
        }

        public void MoveRandomly()
        {
            StepsSinceLastCollision++;

            // Clear current cell position
            Console.SetCursorPosition(X % Console.BufferWidth, Y % Console.BufferHeight);
            Console.Write(" ");

            int logAreaTop = Console.BufferHeight - 11;
            if (Y >= logAreaTop)
            {
                Y = logAreaTop - 1;
            }
            
            int xMovement = 0;
            int yMovement = 0;
            
            
            
            X = Math.Max(0, Math.Min(Console.BufferWidth - 1, X + xMovement));
            Y = Math.Max(0, Math.Min(logAreaTop - 1, Y + yMovement));
           
            
            MovementBias bias = MovementBias.None;
            foreach (var mutation in Mutations)
            {
                if (mutation.MovementBias != MovementBias.None)
                {
                    bias = mutation.MovementBias;
                    break;
                }
            }

            if (_random.Next(0, 100) < 51)
            {
                // Use bias 51% of the time
                switch (Mutations.First().MovementBias)
                {
                    case MovementBias.North:
                        Y = Math.Max(0, Y - 1);
                        break;
                    case MovementBias.South:
                        if (Y == Console.BufferHeight - 11)
                            Y = 0;
                        else
                            Y = Y + 1;
                        break;
                    case MovementBias.East:
                        X = Math.Min(Console.BufferWidth - 1, X + 1);
                        break;
                    case MovementBias.West:
                        X = Math.Max(0, X - 1);
                        break;
                }
            }
            else
            {
                // Move randomly 49% of the time
                xMovement = _random.Next(-1, 2);
                yMovement = _random.Next(-1, 2);
            }

            // Wrap around the grid when moving east or west
            X = (X + xMovement + 100) % 100;
            // Wrap around the grid when moving north or south
            Y = (Y + yMovement + 100) % 100;

            // Add the current position to the trail
            UpdateTrail();

            // Keep the trail at a maximum of 10 positions long
            if (Trail.Count > 40)
            {
                Trail.RemoveAt(0);
            }

            var bh = Console.BufferHeight - 11;

            if (bh < 0)
            {
                bh = 0;
            }

            DrawATrail(bh);

            ClearCurrentCellPosition(bh);
            
            // Add current position to trail
            UpdateTrail();

            // Remove oldest position from trail if it has grown too long
            CleanupTrail(bh);

            // Draw new cell position
            Console.SetCursorPosition(X, (Y + bh) % bh);
            Console.ForegroundColor = CellColor;

            Console.Write(CellForm);
            string msg = $"{DateTime.Now}: Cell {this.Id}" +
                         $": {this.CellForm}" +
                         $": {this.CellColor}" +
                         $"X{this.X}" +
                         $"Y{this.Y}" +
                         $" Age: {this.Age}" +
                         $" IsAlive: {this.IsAlive} - " +
                         $"MovementBias: {this.Mutations.Last().MovementBias};";
            //log the cell position withthe Log fucntion
            Log(msg, this);

        }

        private void CleanupTrail(int bh)
        {
            if (Trail.Count > 10)
            {
                var (x, y) = Trail[0];
                Console.SetCursorPosition(x, (y + bh) % bh);
                Console.Write(" ");
                Trail.RemoveAt(0);
            }
        }

        private void UpdateTrail()
        {
            Trail.Add((X, Y));
        }

        private void DrawATrail(int bh)
        {
            // Draw the trail
            foreach (var (x, y) in Trail)
            {
                Console.SetCursorPosition(x, (y + bh) % bh);
                Console.ForegroundColor = CellColor;
                // mutation console.ForegroundColor = (consoleColor)_random.Next(1, 16);
                Console.Write(".");
            }
        }

        private void ClearCurrentCellPosition(int bh)
        {
            // Clear current cell position
            Console.SetCursorPosition(X, (Y + bh) % bh);
            Console.Write(" ");
        }

        private string[] _logMessages = new string[10];

        private void Log(string message, Cell c)
        {
            // Set the cursor position based on the cell ID
            int row = 0;
            switch (this.Id)
            {
                case 1:
                    row = Console.BufferHeight - 10;
                    _logMessages[0] = message;
                    break;
                case 2:
                    row = Console.BufferHeight - 9;
                    _logMessages[1] = message;
                    break;
                case 3:
                    row = Console.BufferHeight - 8;
                    _logMessages[2] = message;
                    break;
                case 4:
                    row = Console.BufferHeight - 7;
                    _logMessages[3] = message;
                    break;
                case 5:
                    row = Console.BufferHeight - 6;
                    _logMessages[4] = message;
                    break;
                case 6:
                    row = Console.BufferHeight - 5;
                    _logMessages[5] = message;
                    break;
                case 7:
                    row = Console.BufferHeight - 4;
                    _logMessages[6] = message;
                    break;
                case 8:
                    row = Console.BufferHeight - 3;
                    _logMessages[7] = message;
                    break;
                case 9:
                    row = Console.BufferHeight - 2;
                    _logMessages[8] = message;
                    break;
                case 10:
                    row = Console.BufferHeight - 1;
                    _logMessages[9] = message;
                    break;
                default:
                    row = Console.BufferHeight - 0;
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            // Add the message to the list of log messages


            Console.SetCursorPosition(0, Console.BufferHeight - 10);
            //Console.Write(new string(' ', Console.BufferWidth * 10));
            // Write the log messages
            for (int i = 0; i < 10; i++)
            {
                // Wrap the row around the console's buffer size
                int wrappedRow = (Console.BufferHeight - 9 + i) % Console.BufferHeight;
                Console.SetCursorPosition(0, wrappedRow);
                Console.Write(_logMessages[i]);
            }
        }
    }
}