# RogueLikeLowcost
Student project made in the University of Derby, in the module Game Behavior. The game is a simple rogue like game, where the player is able to shoot to attack the enemies. There are three different types of enemies, with different behaviors. The player has 3 lives. The map is generated procedurally every time the player defeats all the enemies at the current level.
The game is developed in Unity, but we were not able to use the physics system in Unity, the pathfinding, or anything else, so everything is coded by hand.
- AABB, SAT, and GJK algorithm for the collisions
- A* for pathfinding
- Different final state machines (FSM) for each enemy
- Movement, force, and drag for the bullets




The game has been developen in Unity.
Unity version: 2020.3.20f1

The languade used is C#. I have coded in Visual Studio 2019.

The game is inspired by the rogue-like games.
The player (green diamond) has to kill the enemies with its bullets.
If the player touches an enemy or an enemy bullet, it lose one live. If it losts the three lives,
the game start again with 0 points.
If the player touches an enemy, the enemies and the player are teleported to their original positions.
If the player kills all the enemies, the game recalculates a new map.

There are three types of enemies:
Enemy1 - Big red square. It attacks you if you are near or if its lives is 1. It gives you 1 point.
Enemy2 - Red diamond. It fires randomly, and the movement is random too. If you are near, its movement is
faster. If its live is 1, its fire in the four direction. It gives you 3 points.
Enemy3 - Small red square. It attacks when it is the last enemy. Until then, you can't kill it. It gives you 10 points.

The orange/brown squares are obstacles.

Controls:
WASD to move the player
Arrows to fire the bullet.
Space to recreate the map. Please use this only if it is necessary, it may cause problems.
The map and the positions are generated randomly, so sometimes it is possible that the player spawns
surrounded by obstacles or enemies. Use space in that case.
