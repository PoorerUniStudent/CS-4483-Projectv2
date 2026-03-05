using UnityEngine;

public class PlayerFiniteStateMachine
{
    public PlayerState currentState { get; private set; }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }
}

