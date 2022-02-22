using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndTurnState : ITurnStateMachine
{
    public UnityEvent NewStateAlert { get; set; }

    public TurnStateEnum GetNextState()
    {
        return TurnStateEnum.End;
    }
}
