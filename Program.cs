using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using CellSim;

namespace CellSim
{
    internal class Program
    {


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
            cellCounter = Gen10StarterCells(_random, lastAlphabetPick, cells, cellCounter);

            lastAlphabetPick++;
            while (true)
            {
                // Check for dead cells and remove them from the list
                cells = cells.Where(c => c.IsAlive).ToList();
                //update the cell counter
                cellCounter = cells.Count;

                foreach (var cell in cells)
                {
                    MutateTheCell(_random, cell);

                    //cells age > 100 have a 1% chance of dying
                    // if (cell.Age > 100 && _random.Next(0, 1000) < 1)
                    // {
                    //     cell.IsAlive = false;
                    // }

                    MoveTheCell(cell);
                    AgeTheCell(cell);
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
                                ClearTheDeadCollidingCells(cells, i, j);
                                // Cells have collided, create 4 new cells
                                for (int k = 0; k < 4; k++)
                                {
                                    SleepTime = SleepTime++;
                                    // Console.WriteLine("--------------------------------------------");
                                    Cell newCell = CreateTheNewCell(_random, lastAlphabetPick, cells, i, k);

                                    //draw the new cell
                                    DrawNewCell(cells, i, newCell);

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

        private static void ClearTheDeadCollidingCells(List<Cell> cells, int i, int j)
        {
            Console.SetCursorPosition(cells[i].X % Console.BufferWidth, cells[i].Y % Console.BufferHeight);
            Console.Write("$");
            cells[i].IsAlive = false;
            cells[j].IsAlive = false;
        }

        private static void DrawNewCell(List<Cell> cells, int i, Cell newCell)
        {
            var bh = Console.BufferHeight - 11;
            if (cells[i].X % Console.BufferWidth >= 0 && cells[i].X % Console.BufferWidth < Console.BufferWidth)
            {
                Console.SetCursorPosition(cells[i].X % Console.BufferWidth,
                    cells[i].Y % bh);
                Console.Write(newCell.CellForm);
            }
        }

        private static Cell CreateTheNewCell(Random _random, int lastAlphabetPick, List<Cell> cells, int i, int k)
        {
            var newCell = new Cell();
            newCell.CellForm = Cell.alphabet[lastAlphabetPick % Cell.alphabet.Length];

            // Add 3 to the X position of the first cell using cosine
            newCell.X = cells[i].X + (int)Math.Round(Math.Cos((k / 10.0) * 2 * Math.PI) * 3);
            // Add 3 to the Y position of the first cell using sine
            newCell.Y = cells[i].Y + (int)Math.Round(Math.Sin((k / 10.0) * 2 * Math.PI) * 3);
            newCell.Mutations.Add(new Mutation
            { MovementBias = (MovementBias)_random.Next(1, 5) });
            newCell.StepsSinceLastCollision = 0;
            newCell.CellColor = (ConsoleColor)_random.Next(1, 16);
            newCell.Age = 0;
            newCell.IsAlive = true;
            cells.Add(newCell);
            return newCell;
        }

        private static void AgeTheCell(Cell cell)
        {
            cell.Age++;
        }

        private static void MoveTheCell(Cell cell)
        {
            cell.MoveRandomly();
        }

        private static void MutateTheCell(Random _random, Cell cell)
        {
            //if the cell age is greater than 10, it has a 10% chance of mutating
            if (cell.Age > 10 && _random.Next(0, 100) < 10)
            {
                cell.Mutations.Add(new Mutation { MovementBias = (MovementBias)_random.Next(1, 5) });
            }
        }

        private static int Gen10StarterCells(Random _random, int lastAlphabetPick, List<Cell> cells, int cellCounter)
        {
            for (int i = 0; i < 10; i++)
            {
                var cell = new Cell();
                cell.Id = cellCounter;
                cell.CellForm = Cell.alphabet[lastAlphabetPick];

                cell.X = _random.Next(0, 101);
                cell.Y = _random.Next(0, 101);
                cell.Mutations.Add(new Mutation { MovementBias = (MovementBias)_random.Next(1, 5) });
                cell.CellColor = (ConsoleColor)_random.Next(1, 16);
                cells.Add(cell);
                cellCounter++;
            }

            return cellCounter;
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