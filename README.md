# CellSim

![image](https://user-images.githubusercontent.com/195927/209434435-95e9031e-4901-435b-92cf-d7ef1fc998e3.png)

This is a console application that simulates the behavior of cells in a 2D space. The program creates a list of `Cell` objects and initializes them with random properties such as their form, position, and movement bias. The program then enters an infinite loop, in which it moves each cell randomly and increments their age. 

## Features
- If a cell's age is greater than 10, it has a 10% chance of mutating.
- If a cell's age is greater than 100, it has a 1% chance of dying. 
- The program checks for collisions between cells and, if two cells collide, it creates 4 new cells and removes the colliding cells from the list. 
- The program displays the number of cells on the top left corner of the console and clears the console before printing the cells' new positions.


## TODO
- Cell reproduction: You could allow cells to reproduce by splitting into two new cells when their age reaches a certain threshold. You could also allow cells to reproduce by fusing with another cell when they collide.

- Cell evolution: You could allow cells to evolve over time by introducing new mutations that change their behavior or appearance. For example, cells could mutate to become more or less likely to move in a particular direction, or they could mutate to change color or form.

- Environmental factors: You could add environmental factors to the simulation, such as food or obstacles, which cells must navigate or avoid. You could also allow cells to die off if they run out of energy or if they are exposed to harmful conditions for too long.

- Genetic inheritance: You could allow cells to inherit traits from their parents, such as movement bias or mutations. This would allow cells to evolve over multiple generations.

- Population dynamics: You could keep track of the overall population size and allow it to fluctuate based on factors such as reproduction rate, death rate, and environmental conditions.

- Graphical display: You could create a graphical display of the simulation using a library like System.Drawing or a third-party library like SFML or SDL. This would allow you to visualize the simulation in a more intuitive way and add additional visual elements like backgrounds or particle effects.
