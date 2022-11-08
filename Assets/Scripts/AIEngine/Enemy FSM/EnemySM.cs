using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySM : StateMachine
{

    public Attack attackState;
    public Return returnState;
    public Wait waitState;
    public Carnage carnageState;
    public GameObject enemy;
    
    private void Awake()
    {
        attackState = new Attack(this);
        returnState = new Return(this);
        waitState = new Wait(this);
        carnageState = new Carnage(this);
        enemy = this.gameObject;
    }

    protected override State GetInitialState()
    {
        return waitState;
    }
}
