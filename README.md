# CellSim

This is a console application that simulates the behavior of cells in a 2D space. The program creates a list of `Cell` objects and initializes them with random properties such as their form, position, and movement bias. The program then enters an infinite loop, in which it moves each cell randomly and increments their age. 

## Features
- If a cell's age is greater than 10, it has a 10% chance of mutating.
- If a cell's age is greater than 100, it has a 1% chance of dying. 
- The program checks for collisions between cells and, if two cells collide, it creates 4 new cells and removes the colliding cells from the list. 
- The program displays the number of cells on the top left corner of the console and clears the console before printing the cells' new positions.
