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

        public virtual void InvokeAction(Mingming source, Mingming target, Card card)
        {
            OnInvoked?.Invoke();

            PerformAnimation(source, target);
        }

        public virtual void PerformAnimation(Mingming source, Mingming target)
        {
            Vector3 currentPosition = source.transform.position;
            Vector3 destination = target.transform.position;

            LeanTween.move(source.gameObject, destination, durationSeconds).setEaseOutBack();
            LeanTween.delayedCall(durationSeconds, () => LeanTween.move(source.gameObject, currentPosition, durationSeconds / 2));
        }
    }
}