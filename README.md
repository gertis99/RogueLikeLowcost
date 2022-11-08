# RogueLikeLowcost
Student project made in the University of Derby, in the module Game Behavior. The game is a simple rogue like game, where the player is able to shoot to attack the enemies. There are three different types of enemies, with different behaviors. The player has 3 lives. The map is generated procedurally every time the player defeats all the enemies at the current level.
The game is developed in Unity, but we were not able to use the physics system in Unity, the pathfinding, or anything else, so everything is coded by hand.
- AABB, SAT, and GJK algorithm for the collisions
- A* for pathfinding
- Different final state machines (FSM) for each enemy
- Movement, force, and drag for the bullets
