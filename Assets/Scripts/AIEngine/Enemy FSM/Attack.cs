using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    // Variables
    private EnemySM enemySM;
    private float distance;
    private Vector3 playerPosition;
    private GameObject target, colObject;

    // Scripts
    private Pathfinding pathfindingScript;
    private Enemy enemyScript;

    public Attack(EnemySM stateMachine) : base("Attack", stateMachine)
    {
        enemySM = stateMachine;
    }

    // When enter in the state, get the player position and distance, and the first target
    public override void Enter() {
        base.Enter();
        playerPosition = player.transform.position;
        distance = Vector3.Distance(playerPosition, enemySM.enemy.transform.position);
        pathfindingScript = manager.GetComponent<Pathfinding>();
        enemyScript = enemySM.enemy.GetComponent<Enemy>();
        target = pathfindingScript.calculateAStar((int)enemySM.enemy.transform.position.y, (int)enemySM.enemy.transform.position.x,
                                                           (int)player.transform.position.y, (int)player.transform.position.x);
    }

    // If the player is so far, go to return state
    // If not, move to the player
    public override void Logic() {
        base.Logic();
        if(distance > 4.0f)     // If the player is far, change to return state
        {
            stateMachine.ChangeState(enemySM.returnState);
        }
        else
        {
            playerPosition = player.transform.position;
            manager.GetComponent<Manager>().clearMap();
            
            // Only calculate another targget when it reaches the actual target
           if(target == null || enemySM.enemy.transform.position == target.transform.position)
                target = pathfindingScript.calculateAStar((int)enemySM.enemy.transform.position.y, (int)enemySM.enemy.transform.position.x,
                                                               (int)player.transform.position.y, (int)player.transform.position.x);

            enemySM.enemy.transform.position = Vector3.MoveTowards(enemySM.enemy.transform.position, target.transform.position, Time.deltaTime * enemySM.enemy.GetComponent<Enemy>().getSpeed());
        }

        distance = Vector3.Distance(playerPosition, enemySM.enemy.transform.position);

        // If the object only has 1 live, change to carnage state
        if (enemyScript.getLives() == 1)
        {
            stateMachine.ChangeState(enemySM.carnageState);
        }
    }

    
    public override void Exit()
    {
      
    }
}
