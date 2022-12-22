using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameScene.Entities
{
    public class TimerTrigger : MonoBehaviour, ITriggerable
    {
        [SerializeField] private float Timer;
        [SerializeField] private UnityEvent OnTimer;

        bool isOn;

        private float TimeLeft;

        private void Start()
        {
            isOn = false;
        }

        private void Update()
        {
            if (isOn)
            {
                TimeLeft -= Time.deltaTime;
                if (TimeLeft <= 0)
                {
                    OnTimer?.Invoke();
                    TimeLeft = Timer;
                }
            }
        }

        public void SetIsOn(bool _isOn, PlayerController player)
        {
            isOn = _isOn;
            if (isOn)
            {
                TimeLeft = Timer;
            }
        }
    }
}
