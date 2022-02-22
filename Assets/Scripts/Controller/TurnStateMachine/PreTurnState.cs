using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PreTurnState : ITurnStateMachine
{
    public UnityEvent NewStateAlert { get; set; }

    public TurnStateEnum GetNextState()
    {
        return TurnStateEnum.AttackTurn;
    }
}
