using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : State
{
    // Variables
    private EnemySM enemySM;
    private float distance;
    private Vector3 playerPosition;

    // Scripts
    private Enemy enemyScript;

    public Wait(EnemySM stateMachine) : base("Wait", stateMachine)
    {
        enemySM = stateMachine;
    }

    // When enter, get the player position and distance
    public override void Enter() {
        base.Enter();
        enemyScript = enemySM.enemy.GetComponent<Enemy>();
        playerPosition = player.transform.position;
        distance = Vector3.Distance(playerPosition, enemySM.enemy.transform.position);
    }

    // When the player is near, go to the attack state
    public override void Logic() {
        base.Logic();
        // If the player is near, change to attack state
        if(distance < 3.0f)
        {
            stateMachine.ChangeState(enemySM.attackState);
        }
        else
        {
            playerPosition = player.transform.position;
            distance = Vector3.Distance(playerPosition, enemySM.enemy.transform.position);
        }

        // If lives = 1, change to carnage state
        if (enemyScript.getLives() == 1)
        {
            stateMachine.ChangeState(enemySM.carnageState);
        }
    }

    public override void Exit() {
    }
}
