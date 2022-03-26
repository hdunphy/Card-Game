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

        private readonly Dictionary<Monster, UnityAction> _monsterActions = new Dictionary<Monster, UnityAction>();

        public override void ApplyStatus(Monster monster, int count)
        {
            int currentCount = monster.GetStatusCount(this);
            int _count = Mathf.Clamp(count, count, maxCount - currentCount);
            
            base.ApplyStatus(monster, _count);

            if (!_monsterActions.ContainsKey(monster))
            {
                UnityAction action = delegate { monster.GetStatusEffect(this); };
                _monsterActions.Add(monster, action);

                FindObjectsOfType<MonsterController>().First(m => m.HasMonster(monster))
                    .AddListenerToTurnStateMachine(TurnStateEnum.PreTurn, action);
            }
        }

        public override void RemoveStatus(Monster monster)
        {
            base.RemoveStatus(monster);

            FindObjectsOfType<MonsterController>().First(m => m.HasMonster(monster))
                .RemoveListenerToTurnStateMachine(TurnStateEnum.PreTurn, _monsterActions[monster]);

            _monsterActions.Remove(monster);
        }

        public override void DoEffect(Monster monster, int count)
        {
            if(count > 1)
            {
                UserMessage.Instance.SendMessageToUser($"{monster.name} is {name} for {count} more turn(s)");
            }
            monster.ApplyStatus(this, -1);
        }

        public override string GetTooltip(int count) => $"{description}. Lasts for {count} turns;";

        public override string GetTooltipHeader(int count) => name;
    }
}
