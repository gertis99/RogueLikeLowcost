using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public string name;
    protected StateMachine stateMachine;
    public GameObject player, manager;

    public State(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
        player = GameObject.Find("Player");
        manager = GameObject.Find("GameManager");
    }


    public virtual void Enter() { }
    public virtual void Logic() { }
    public virtual void Exit() { }
}
