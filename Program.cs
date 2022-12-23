using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CellSim
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Clear();
            Console.CursorVisible = false;
            var _random = new Random();
            int SleepTime = 5;

            var cells = new List<Cell>();

            int cellCounter = 0;
            
            for (int i = 0; i < 10; i++)
            {
                var cell = new Cell();
                cell.CellForm = _random.Next(0, 2) == 0 ? "x" : "o";
                cell.X = _random.Next(0, 101);
                cell.Y = _random.Next(0, 101);
                cell.Mutations.Add(new Mutation { MovementBias = (MovementBias)_random.Next(1, 5) });
                cells.Add(cell);
                cellCounter++;
            }
            

            while (true)
            {
                // Check for dead cells and remove them from the list
                cells = cells.Where(c => c.IsAlive).ToList();
                //update the cell counter
                cellCounter = cells.Count;
                
                foreach (var cell in cells)
                {
                    //if the cell age is greater than 10, it has a 10% chance of mutating
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
                        //check if the cells StepsSinceLastCollision is > 9
                        if (cells[i].StepsSinceLastCollision > 9 && cells[j].StepsSinceLastCollision > 9 && cells[i].IsAlive && cells[j].IsAlive)
                        {
                            if (cells[i].X == cells[j].X && cells[i].Y == cells[j].Y)
                            {
                                
                                
                                //clear the cell from the screen
                                Console.SetCursorPosition(cells[i].X % Console.BufferWidth, cells[i].Y % Console.BufferHeight);
                                Console.Write(" ");
                                cells[i].IsAlive = false;
                                cells[j].IsAlive = false;
                                // Cells have collided, create 4 new cells
                                for (int k = 0; k < 10; k++)
                                {
                                    SleepTime = 500;
                                    // Console.WriteLine("--------------------------------------------");
                                    var newCell = new Cell();
                                    newCell.CellForm = _random.Next(0, 2) == 0 ? "z" : "y";
                                    // Add 3 to the X position of the first cell using cosine
                                    newCell.X = cells[i].X + (int)Math.Round(Math.Cos((k / 10.0) * 2 * Math.PI) * 3);
                                    // Add 3 to the Y position of the first cell using sine
                                    newCell.Y = cells[i].Y + (int)Math.Round(Math.Sin((k / 10.0) * 2 * Math.PI) * 3);
                                    newCell.Mutations.Add(new Mutation { MovementBias = (MovementBias)_random.Next(1, 5) });
                                    newCell.StepsSinceLastCollision = 0;
                                    cells.Add(newCell);
                                    cellCounter++;
                                }
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

        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }
        public List<Mutation> Mutations { get; set; }
        public string CellForm { get; set; }
        public List<(int X, int Y)> Trail { get; set; }
        public int StepsSinceLastCollision { get; set; }
        public int Age { get; set; }

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
            if (StepsSinceLastCollision < 10)
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
                switch (bias)
                {
                    case MovementBias.North:
                        yMovement = -1;
                        break;
                    case MovementBias.South:
                        yMovement = 1;
                        break;
                    case MovementBias.East:
                        xMovement = 1;
                        break;
                    case MovementBias.West:
                        xMovement = -1;
                        break;
                    default:
// No bias
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
            if (Trail.Count > 10)
            {
                Trail.RemoveAt(0);
            }

            // Draw the trail
            foreach (var (x, y) in Trail)
            {
                Console.SetCursorPosition(x, y % Console.BufferHeight);
                Console.ForegroundColor = (ConsoleColor)_random.Next(1, 16);
                Console.Write(".");
            }
            
            // Clear current cell position
            Console.SetCursorPosition(X, Y % Console.BufferHeight);
            Console.Write(" ");
            // Add current position to trail
            Trail.Add((X, Y));

            // Remove oldest position from trail if it has grown too long
            if (Trail.Count > 10)
            {
                var (x, y) = Trail[0];
                Console.SetCursorPosition(x, y % Console.BufferHeight);
                Console.Write(" ");
                Trail.RemoveAt(0);
            }

            // Draw new cell position
            Console.SetCursorPosition(X, Y % Console.BufferHeight);
            Console.ForegroundColor = (ConsoleColor)_random.Next(1, 16);
            
            Console.Write(CellForm);
            // Set a random color for the cell
            Console.ForegroundColor = (ConsoleColor)_random.Next(1, 16);

            // Draw new cell position
            Console.SetCursorPosition(X, Y % Console.BufferHeight);
            Console.Write(CellForm);
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