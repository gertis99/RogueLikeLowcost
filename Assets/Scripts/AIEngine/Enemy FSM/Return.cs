using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : State
{
    // Variables
    private EnemySM enemySM;
    private float posX, posY;
    private GameObject target;

    // Scripts
    private Pathfinding pathfindingScript;
    private Enemy enemyScript;

    public Return(EnemySM stateMachine) : base("Return", stateMachine)
    {
        enemySM = stateMachine;
    }

    // When enter, get original pos
    public override void Enter() {
        base.Enter();
        pathfindingScript = manager.GetComponent<Pathfinding>();
        enemyScript = enemySM.enemy.GetComponent<Enemy>();
        posX = enemyScript.getPosX();
        posY = enemyScript.getPosY();
        
        target = pathfindingScript.calculateAStar((int)enemySM.enemy.transform.position.y, (int)enemySM.enemy.transform.position.x,
                                                           (int)posY, (int)posX);
    }

    // Go the the original pos. When reached, go to wait state
    public override void Logic() {
        base.Logic();

        // If reach the original position, change to wait state
        if(enemySM.enemy.transform.position.x == posX && enemySM.enemy.transform.position.y == posY)
        {
            stateMachine.ChangeState(enemySM.waitState);
        }
        else
        {
            manager.GetComponent<Manager>().clearMap();

            // Only calculate another targget when it reaches the actual target
            if (target == null || enemySM.enemy.transform.position == target.transform.position)
                target = pathfindingScript.calculateAStar((int)enemySM.enemy.transform.position.y, (int)enemySM.enemy.transform.position.x,
                                                               (int)player.transform.position.y, (int)player.transform.position.x);

            enemySM.enemy.transform.position = Vector3.MoveTowards(enemySM.enemy.transform.position, target.transform.position, Time.deltaTime * enemySM.enemy.GetComponent<Enemy>().getSpeed());
        }

        // If lives = 1, change to carnage state
        if (enemyScript.getLives() == 1)
        {
            stateMachine.ChangeState(enemySM.carnageState);
        }
    }
    public override void Exit() { }
}
