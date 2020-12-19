using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    States<T> currentState;
    public FSM() { }
    public FSM(States<T> init)
    {
        if (init != null) SetInit(init);
    }

    public void SetInit(States<T> init)
    {
        currentState = init;
        currentState.Awake();
    }

    public void OnUpdate()
    {
        currentState.Execute();
    }

    public void Transition(T input)
    {
        States<T> newState = currentState.GetState(input);
        if (newState == null) return;

        currentState.Exit();
        newState.Awake();
        currentState = newState;
    }

}
