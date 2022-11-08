using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2SM : StateMachine
{
    public RandomState randomState;
    public Scared scaredState;
    public Survival survivalState;
    public GameObject enemy2;

    private void Awake()
    {
        randomState = new RandomState(this);
        scaredState = new Scared(this);
        survivalState = new Survival(this);
        enemy2 = this.gameObject;
    }

    protected override State GetInitialState()
    {
        return randomState;
    }
}
