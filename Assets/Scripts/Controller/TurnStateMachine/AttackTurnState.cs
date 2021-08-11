using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackTurnState : ITurnStateMachine
{
    public UnityEvent NewStateAlert { get; set; }

    public TurnStateEnum GetNextState()
    {
        return TurnStateEnum.PostTurn;
    }

    public void ProcessState()
    {
        throw new System.NotImplementedException();
    }
}
