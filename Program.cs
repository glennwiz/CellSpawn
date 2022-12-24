using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace CellSim
{
    internal class Program
    {
        public static string[] alphabet =
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
            "v", "w", "x", "y", "z"
        };

        private static void Main(string[] args)
        {
            Console.Clear();
            Console.CursorVisible = false;
            var _random = new Random();
            int SleepTime = 50;
            int lastAlphabetPick = 0;

            var cells = new List<Cell>();
            int cellCounter = 0;

            //gen 1 cells
            for (int i = 0; i < 10; i++)
            {
                var cell = new Cell();
                cell.Id = cellCounter;
                cell.CellForm = alphabet[lastAlphabetPick];

                cell.X = _random.Next(0, 101);
                cell.Y = _random.Next(0, 101);
                cell.Mutations.Add(new Mutation {MovementBias = (MovementBias) _random.Next(1, 5)});
                cell.CellColor = (ConsoleColor) _random.Next(1, 16);
                cells.Add(cell);
                cellCounter++;
            }

            lastAlphabetPick++;
            while (true)
            {
                // Check for dead cells and remove them from the list
                cells = cells.Where(c => c.IsAlive).ToList();
                //update the cell counter
                cellCounter = cells.Count;

                foreach (var cell in cells)
                {
                    //if the cell age is greater than 10, it has a 10% chance of mutating
                    if (cell.Age > 10 && _random.Next(0, 100) < 10)
                    {
                        cell.Mutations.Add(new Mutation {MovementBias = (MovementBias) _random.Next(1, 5)});
                    }

                    //cells age > 100 have a 1% chance of dying
                    // if (cell.Age > 100 && _random.Next(0, 1000) < 1)
                    // {
                    //     cell.IsAlive = false;
                    // }

                    cell.MoveRandomly();
                    cell.Age++;
                }

                // Display the cell counter in the top left corner
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"Number of cells: {cellCounter}");

                // Check for collisions between cells
                for (int i = 0; i < cells.Count; i++)
                {
                    for (int j = i + 1; j < cells.Count; j++)
                    {
                        //check if the cells StepsSinceLastCollision is > 100 
                        if (cells[i].StepsSinceLastCollision > 99 && cells[j].StepsSinceLastCollision > 99 &&
                            cells[i].IsAlive && cells[j].IsAlive)
                        {
                            if (cells[i].X == cells[j].X && cells[i].Y == cells[j].Y)
                            {
                                //clear the dead cell from the screen
                                Console.SetCursorPosition(cells[i].X % Console.BufferWidth,
                                    cells[i].Y % Console.BufferHeight);
                                Console.Write(" ");
                                cells[i].IsAlive = false;
                                cells[j].IsAlive = false;
                                // Cells have collided, create 4 new cells
                                for (int k = 0; k < 4; k++)
                                {
                                    SleepTime = SleepTime++;
                                    // Console.WriteLine("--------------------------------------------");
                                    var newCell = new Cell();
                                    newCell.CellForm = alphabet[lastAlphabetPick % alphabet.Length];
                                 
                                    // Add 3 to the X position of the first cell using cosine
                                    newCell.X = cells[i].X + (int) Math.Round(Math.Cos((k / 10.0) * 2 * Math.PI) * 3);
                                    // Add 3 to the Y position of the first cell using sine
                                    newCell.Y = cells[i].Y + (int) Math.Round(Math.Sin((k / 10.0) * 2 * Math.PI) * 3);
                                    newCell.Mutations.Add(new Mutation
                                        {MovementBias = (MovementBias) _random.Next(1, 5)});
                                    newCell.StepsSinceLastCollision = 0;
                                    newCell.CellColor = (ConsoleColor) _random.Next(1, 16);
                                    newCell.Age = 0;
                                    newCell.IsAlive = true;
                                    cells.Add(newCell);

                                    //draw the new cell
                                    if (cells[i].X % Console.BufferWidth >= 0 &&
                                        cells[i].X % Console.BufferWidth < Console.BufferWidth)
                                    {
                                        Console.SetCursorPosition(cells[i].X % Console.BufferWidth,
                                            cells[i].Y % Console.BufferHeight);
                                        Console.Write(newCell.CellForm);
                                    }

                                    cellCounter++;
                                }
                                lastAlphabetPick++;
                            }
                        }
                    }
                }

                Thread.Sleep(SleepTime);
            }
        }
    }

    public class Cell
    {
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
            if (StepsSinceLastCollision < collisonPeriode)
            {
                StepsSinceLastCollision++;
                return;
            }

            // Clear current cell position
            Console.SetCursorPosition(X % Console.BufferWidth, Y % Console.BufferHeight);
            Console.Write(" ");

            int xMovement = 0;
            int yMovement = 0;
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
                        Y = Math.Min(Console.BufferHeight - 11, Y + 1);
                        break;
                    case MovementBias.East:
                        X = Math.Max(0, X - 1);
                        break;
                    case MovementBias.West:
                        X = Math.Min(Console.BufferWidth - 1, X + 1);
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
            Trail.Add((X, Y));

            // Keep the trail at a maximum of 10 positions long
            if (Trail.Count > 40)
            {
                Trail.RemoveAt(0);
            }

            var bh = Console.BufferHeight - 10;

            if (bh < 0)
            {
                bh = 0;
            }

            // Draw the trail
            foreach (var (x, y) in Trail)
            {
                Console.SetCursorPosition(x, (y + bh) % bh);
                Console.ForegroundColor = CellColor;
                // console.ForegroundColor = (consoleColor)_random.Next(1, 16);
                Console.Write(".");
            }

            // Clear current cell position
            Console.SetCursorPosition(X, (Y + bh) % bh);
            Console.Write(" ");
            // Add current position to trail
            Trail.Add((X, Y));

            // Remove oldest position from trail if it has grown too long
            if (Trail.Count > 10)
            {
                var (x, y) = Trail[0];
                Console.SetCursorPosition(x, (y + bh) % bh);
                Console.Write(" ");
                Trail.RemoveAt(0);
            }

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
                int wrappedRow = (Console.BufferHeight - 10 + i) % Console.BufferHeight;
                Console.SetCursorPosition(0, wrappedRow);
                Console.Write(_logMessages[i]);
            }
        }
    }

    public class Mutation
    {
        public Mutation()
        {
            Description = "Unknown mutation";
        }

        public string Description { get; set; }
        public MovementBias MovementBias { get; set; }
    }

    public enum MovementBias
    {
        None,
        North,
        South,
        East,
        West
    }
}