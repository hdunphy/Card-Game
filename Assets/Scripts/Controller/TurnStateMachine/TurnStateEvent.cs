using System;
using UnityEngine.Events;

[Serializable]
public class TurnStateEvent
{
    public TurnStateEnum StateEnum;
    public UnityEvent Event;
}
