using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnage : State
{
    // Variables
    private EnemySM enemySM;
    private GameObject target;

    // Scripts
    private Pathfinding pathfindingScript;

    public Carnage(EnemySM stateMachine) : base("Carnage", stateMachine)
    {
        enemySM = stateMachine;
    }

    // When enter in the state, get the player position and increase enemy speed
    public override void Enter()
    {
        base.Enter();
        enemySM.enemy.GetComponent<Enemy>().setSpeed(2.5f);
        pathfindingScript = manager.GetComponent<Pathfinding>();
        target = pathfindingScript.calculateAStar((int)enemySM.enemy.transform.position.y, (int)enemySM.enemy.transform.position.x,
                                                           (int)player.transform.position.y, (int)player.transform.position.x);
    }

    // If the player is so far, go to return state
    // If not, move to the player
    public override void Logic()
    {
        base.Logic();
        
        manager.GetComponent<Manager>().clearMap();

        // Only calculate another targget when it reaches the actual target
        if (target == null || enemySM.enemy.transform.position == target.transform.position)
            target = pathfindingScript.calculateAStar((int)enemySM.enemy.transform.position.y, (int)enemySM.enemy.transform.position.x,
                                                           (int)player.transform.position.y, (int)player.transform.position.x);

        enemySM.enemy.transform.position = Vector3.MoveTowards(enemySM.enemy.transform.position, target.transform.position, Time.deltaTime * enemySM.enemy.GetComponent<Enemy>().getSpeed());
        
     
    }

    public override void Exit()
    {
        base.Exit();
    }
}
