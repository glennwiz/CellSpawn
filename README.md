# CellSim

![image](https://user-images.githubusercontent.com/195927/209436858-5697229b-c9db-417f-a780-be0229fa5851.png)

TThis C# code is a simulation of cellular automata in which cells move randomly on the console and can mutate. The cells are represented by the `Cell` class, which has the following properties:

- `Id`: a unique identifier for each cell
- `CellForm`: a string representation of the cell's form, chosen from the alphabet array
- `X` and `Y`: the x and y coordinates of the cell's position on the console
- `Mutations`: a list of `Mutation` objects, which contain information about the cell's mutations
- `CellColor`: the color of the cell as it is displayed on the console
- `IsAlive`: a boolean value indicating whether the cell is still alive or not

## Features
- If a cell's age is greater than 10, it has a 10% chance of mutating.
- If a cell's age is greater than 100, it has a 1% chance of dying. 
- The program checks for collisions between cells and, if two cells collide, it creates 4 new cells and removes the colliding cells from the list. 
- The program displays the number of cells on the top left corner of the console and clears the console before printing the cells' new positions.

## To-Do List

- [ ] Trail mutation: Leaves a diffrent trail, like * - , ¨ \ ` and so on.
- [ ] Add Energy: cells dont have any enegy atm, so add energy and depletion on movment.
- [ ] Agro Mutation: Allow cells to hunt and consume other cells to gain energy, should burn more energy when hunting
- [ ] Runner: Avoid others when energy > 80%
- [ ] Cell reproduction: Allow cells to reproduce by splitting into two new cells when their age reaches a certain threshold.
- [ ] Cell fusion: Allow cells to reproduce by fusing with another cell when they collide.
- [ ] Movement speed mutation: Allow cells to mutate to become faster or slower.
- [ ] Movement pattern mutation: Allow cells to mutate to follow a more predictable or unpredictable movement pattern.
- [ ] Sensory range mutation: Allow cells to mutate to have a larger or smaller sensory range.
- [ ] Camouflage mutation: Allow cells to mutate to match their surroundings.
- [ ] Size mutation: Allow cells to mutate to become larger or smaller.
- [ ] Shape mutation: Allow cells to mutate to have a different shape, such as a star or a spiral.
- [ ] Color mutation: Allow cells to mutate to have a different color.
- [ ] Environmental factors: Add environmental factors to the simulation, such as food or obstacles, which cells must navigate or avoid.
- [ ] Cell energy: Allow cells to die off if they run out of energy or if they are exposed to harmful conditions for too long.
- [ ] Genetic inheritance: Allow cells to inherit traits from their parents, such as movement bias or mutations.
- [ ] Population dynamics: Keep track of the overall population size and allow it to fluctuate based on factors such as reproduction rate, death rate, and environmental conditions.
- [ ] Graphical display: Create a graphical display of the simulation using a library like `System.Drawing` or a third-party library like SFML or SDL.
This list uses the task list syntax of GitHub-flavored Markdown, which allows you to mark items as incomplete ([ ]) or complete ([x]). You can use this syntax to track your progress on implementing the various features and mutations for the CellSim program.
