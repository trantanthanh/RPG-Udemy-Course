using System.Collections;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState {  get; private set; }

    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

    public bool IsState(PlayerState _state)
    {
        return currentState == _state;
    }
}
