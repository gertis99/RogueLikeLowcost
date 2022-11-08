using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scared : State
{
    // Variables
    private Enemy2SM enemy2SM;
    private float distance;
    private Vector3 playerPosition;
    private GameObject target;
    private int random;
    private Vector3[] positions;

    // Scripts
    private Pathfinding pathfindingScript;
    private Enemy2 enemy2Script;
    private Manager managerScript;

    public Scared(Enemy2SM stateMachine) : base("Scared", stateMachine)
    {
        enemy2SM = stateMachine;
    }

    // When enter in the state, get the player position and distance
    // Change the speed and the fire rate
    public override void Enter()
    {
        base.Enter();
        pathfindingScript = manager.GetComponent<Pathfinding>();
        enemy2Script = enemy2SM.enemy2.GetComponent<Enemy2>();
        managerScript = manager.GetComponent<Manager>();

        playerPosition = player.transform.position;
        distance = Vector3.Distance(playerPosition, enemy2SM.enemy2.transform.position);
        enemy2SM.enemy2.GetComponent<Enemy2>().setSpeed(4);
        enemy2SM.enemy2.GetComponent<Enemy2>().setFireRate(0.5f);
        target = pathfindingScript.calculateAStar((int)enemy2SM.enemy2.transform.position.y, (int)enemy2SM.enemy2.transform.position.x,
                                                           (int)enemy2SM.enemy2.transform.position.y, (int)enemy2SM.enemy2.transform.position.x);
    }

    // If the player is so far, go to return state
    // If not, move to the player
    public override void Logic()
    {
        base.Logic();
        // If enemy live = 1 change to survival state
        if (enemy2Script.getLives() == 1)
            stateMachine.ChangeState(enemy2SM.survivalState);
        else
        {
            // If player is far, change to random state
            if (distance > 5.0f)
            {
                stateMachine.ChangeState(enemy2SM.randomState);
            }
            else
            {
                // Calculate the random position to move and to shoot
                random = Random.Range(1, 5);
                if (target == null || (enemy2SM.enemy2.transform.position.x == target.transform.position.x && enemy2SM.enemy2.transform.position.y == target.transform.position.y))
                {
                    managerScript.clearMap();
                    if (random == 1 && managerScript.isAccesible((int)enemy2SM.enemy2.transform.position.y + 1, (int)enemy2SM.enemy2.transform.position.x))
                    {
                        target = pathfindingScript.calculateAStar((int)enemy2SM.enemy2.transform.position.y, (int)enemy2SM.enemy2.transform.position.x,
                                                                   (int)enemy2SM.enemy2.transform.position.y + 1, (int)enemy2SM.enemy2.transform.position.x);
                    }
                    else if (random == 2 && managerScript.isAccesible((int)enemy2SM.enemy2.transform.position.y - 1, (int)enemy2SM.enemy2.transform.position.x))
                    {
                        target = pathfindingScript.calculateAStar((int)enemy2SM.enemy2.transform.position.y, (int)enemy2SM.enemy2.transform.position.x,
                                                                   (int)enemy2SM.enemy2.transform.position.y - 1, (int)enemy2SM.enemy2.transform.position.x);
                    }
                    else if (random == 3 && managerScript.isAccesible((int)enemy2SM.enemy2.transform.position.y, (int)enemy2SM.enemy2.transform.position.x + 1))
                    {
                        target = pathfindingScript.calculateAStar((int)enemy2SM.enemy2.transform.position.y, (int)enemy2SM.enemy2.transform.position.x,
                                                                   (int)enemy2SM.enemy2.transform.position.y, (int)enemy2SM.enemy2.transform.position.x + 1);
                    }
                    else if (random == 4 && managerScript.isAccesible((int)enemy2SM.enemy2.transform.position.y, (int)enemy2SM.enemy2.transform.position.x - 1))
                    {
                        target = pathfindingScript.calculateAStar((int)enemy2SM.enemy2.transform.position.y, (int)enemy2SM.enemy2.transform.position.x,
                                                                   (int)enemy2SM.enemy2.transform.position.y, (int)enemy2SM.enemy2.transform.position.x - 1);
                    }

                    if (Time.time > enemy2Script.getFireRate())     // Check if it is possible to shoot
                    {
                        GameObject tear = enemy2Script.fireTear();
                        positions = new Vector3[3];
                        if (random == 1)
                        {
                            positions[0] = new Vector3(-0.25f, 0, 0);
                            positions[1] = new Vector3(0, 0.5f, 0);
                            positions[2] = new Vector3(0.25f, 0, 0);
                            tear.GetComponent<LineRenderer>().SetPositions(positions);
                            tear.GetComponent<EnemyTear>().setVelocity((new Vector3(0, 1, 0)) * enemy2SM.enemy2.GetComponent<Enemy2>().getFireForce());
                        }
                        else if (random == 2)
                        {
                            tear.GetComponent<EnemyTear>().setVelocity((new Vector3(1, 0, 0)) * enemy2SM.enemy2.GetComponent<Enemy2>().getFireForce());
                        }
                        else if (random == 3)
                        {
                            positions[0] = new Vector3(-0.25f, 0, 0);
                            positions[1] = new Vector3(0, -0.5f, 0);
                            positions[2] = new Vector3(0.25f, 0, 0);
                            tear.GetComponent<LineRenderer>().SetPositions(positions);
                            tear.GetComponent<EnemyTear>().setVelocity((new Vector3(0, -1, 0)) * enemy2SM.enemy2.GetComponent<Enemy2>().getFireForce());
                        }
                        else if (random == 4)
                        {
                            positions[0] = new Vector3(0, 0.25f, 0);
                            positions[1] = new Vector3(-0.5f, 0, 0);
                            positions[2] = new Vector3(0, -0.25f, 0);
                            tear.GetComponent<LineRenderer>().SetPositions(positions);
                            tear.GetComponent<EnemyTear>().setVelocity((new Vector3(-1, 0, 0)) * enemy2SM.enemy2.GetComponent<Enemy2>().getFireForce());
                        }
                    }
                }


                if (target != null)
                    enemy2SM.enemy2.transform.position = Vector3.MoveTowards(enemy2SM.enemy2.transform.position, target.transform.position, Time.deltaTime * enemy2SM.enemy2.GetComponent<Enemy2>().getSpeed());
                playerPosition = player.transform.position;
                distance = Vector3.Distance(playerPosition, enemy2SM.enemy2.transform.position);
            }
        }
        
    }

    // When exit, change the speed and the fire rate
    public override void Exit()
    {
        base.Exit();
        enemy2SM.enemy2.GetComponent<Enemy2>().setSpeed(1.5f);
        enemy2SM.enemy2.GetComponent<Enemy2>().setFireRate(1);
    }
}
