using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TurnStateEvent
{
    public TurnStateEnum StateEnum;
    public UnityEvent Event;
}
