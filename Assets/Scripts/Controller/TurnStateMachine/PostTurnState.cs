using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PostTurnState : ITurnStateMachine
{
    public UnityEvent NewStateAlert { get; set; }

    public TurnStateEnum GetNextState()
    {
        return TurnStateEnum.PreTurn;
    }

    public void ProcessState()
    {
        throw new System.NotImplementedException();
    }
}
