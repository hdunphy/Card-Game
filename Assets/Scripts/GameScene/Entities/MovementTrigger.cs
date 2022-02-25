using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameScene.Entities
{
    public class MovementTrigger : MonoBehaviour, ITriggerable
    {
        [SerializeField] private float Distance;
        [SerializeField] private UnityEvent OnTrigger;

        private bool isOn;
        private PlayerController player;
        private Vector2 position;

        private void Start()
        {
            isOn = false;
        }

        private void Update()
        {
            if (isOn && player != null)
            {
                float currentDistance = Mathf.Abs(Vector2.Distance(position, player.transform.position));
                if (currentDistance > Distance)
                {
                    position = player.transform.position;
                    OnTrigger?.Invoke();
                }
            }
        }

        public void SetIsOn(bool _isOn, PlayerController _player)
        {
            isOn = _isOn;
            player = _player;
            if (player != null)
            {
                position = player.transform.position;
            }
        }
    }
}
