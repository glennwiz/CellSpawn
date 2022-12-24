# CellSim

![image](https://user-images.githubusercontent.com/195927/209434435-95e9031e-4901-435b-92cf-d7ef1fc998e3.png)

This is a console application that simulates the behavior of cells in a 2D space. The program creates a list of `Cell` objects and initializes them with random properties such as their form, position, and movement bias. The program then enters an infinite loop, in which it moves each cell randomly and increments their age. 

## Features
- If a cell's age is greater than 10, it has a 10% chance of mutating.
- If a cell's age is greater than 100, it has a 1% chance of dying. 
- The program checks for collisions between cells and, if two cells collide, it creates 4 new cells and removes the colliding cells from the list. 
- The program displays the number of cells on the top left corner of the console and clears the console before printing the cells' new positions.


## To-Do List

- [ ] Agro Mutation: Allow cells to hunt and consume other cells to gain energy
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
