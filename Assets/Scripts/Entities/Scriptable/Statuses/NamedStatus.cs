using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Status", menuName = "Data/Status/Create Named Status")]
    public class NamedStatus : BaseStatus
    {
        [SerializeField] private new string name;
        [SerializeField] private string description;
        [SerializeField] private int maxCount;

        private readonly Dictionary<Mingming, UnityAction> _mingmingActions = new Dictionary<Mingming, UnityAction>();

        public override void ApplyStatus(Mingming mingming, int count)
        {
            int currentCount = mingming.Simulation.GetStatusCount(this);
            int _count = Mathf.Clamp(count, count, maxCount - currentCount);
            
            base.ApplyStatus(mingming, _count);

            if (!_mingmingActions.ContainsKey(mingming))
            {
                UnityAction action = delegate { mingming.GetStatusEffect(this); };
                _mingmingActions.Add(mingming, action);

                FindObjectsOfType<PartyController>().First(m => m.HasMingming(mingming))
                    .AddListenerToTurnStateMachine(TurnStateEnum.PostTurn, action);
            }
        }

        public override void RemoveStatus(Mingming mingming)
        {
            base.RemoveStatus(mingming);

            FindObjectsOfType<PartyController>().First(m => m.HasMingming(mingming))
                .RemoveListenerToTurnStateMachine(TurnStateEnum.PostTurn, _mingmingActions[mingming]);

            _mingmingActions.Remove(mingming);
        }

        public override void DoEffect(Mingming mingming, int count)
        {
            if(count > 1)
            {
                UserMessage.Instance.SendMessageToUser($"{mingming.name} is {name} for {count} more turn(s)");
            }
            mingming.ApplyStatus(this, -1);
        }

        public override string GetTooltip(int count) => $"{description}. Lasts for {count} turns;";

        public override string GetTooltipHeader(int count) => name;
    }
}
