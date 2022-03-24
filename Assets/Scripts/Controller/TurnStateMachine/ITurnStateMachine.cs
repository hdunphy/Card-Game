using UnityEngine.Events;

public enum TurnStateEnum { PreTurn, AttackTurn, PostTurn, End }
public interface ITurnStateMachine
{
    UnityEvent NewStateAlert { get; set; }

    TurnStateEnum GetNextState();
}
