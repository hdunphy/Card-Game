using UnityEngine.Events;

public class AttackTurnState : ITurnStateMachine
{
    private UnityEvent _newStateAlert { get; set; }

    public void AddListener(UnityAction action)
    {
        _newStateAlert.AddListener(action);
    }

    public void Invoke()
    {
        _newStateAlert.Invoke();
    }

    public void RemoveListener(UnityAction action)
    {
        _newStateAlert.RemoveListener(action);
    }

    public void SetEvent(UnityEvent _event)
    {
        _newStateAlert = _event;
    }

    public TurnStateEnum GetNextState()
    {
        return TurnStateEnum.PostTurn;
    }
}
