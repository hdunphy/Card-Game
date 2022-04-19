using System;
using UnityEngine.Events;

public enum TurnStateEnum { PreTurn, AttackTurn, PostTurn, End }
public interface ITurnStateMachine
{
    TurnStateEnum GetNextState();

    public void SetEvent(UnityEvent _event);

    public void AddListener(UnityAction action);

    public void RemoveListener(UnityAction action);

    public void Invoke();
}
