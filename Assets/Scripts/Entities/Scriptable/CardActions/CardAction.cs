using Assets.Scripts.Entities.Mingmings;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class CardAction : ScriptableObject
    {
        [SerializeField] protected UnityEvent OnInvoked;

        [Header("Animation Parameters")]
        [SerializeField] protected float durationSeconds = 0.75f;

        public float DurationSeconds => durationSeconds;
        public virtual int ActionScore => 0;

        public virtual void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, CardAlignment cardAlignment)
        {
            OnInvoked?.Invoke();

            //PerformAnimation(source, target);
            source.OnTriggerAnimation(PerformAnimation, target);
        }

        public virtual Action<GameObject, GameObject> PerformAnimation
            => (source, target) =>
            {
                Vector3 currentPosition = source.transform.position;
                Vector3 destination = target.transform.position;
                LeanTween.move(source, destination, durationSeconds).setEaseOutBack();
                LeanTween.delayedCall(durationSeconds, () =>
                {
                    Debug.Log("Animation Moving Back");
                    LeanTween.move(source, currentPosition, durationSeconds / 2);
                });
            };
    }
}