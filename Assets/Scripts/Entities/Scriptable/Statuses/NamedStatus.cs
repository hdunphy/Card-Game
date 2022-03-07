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
        [SerializeField] private string Description;
        [SerializeField] private int Length;

        private Dictionary<Monster, UnityAction> MonsterActions = new Dictionary<Monster, UnityAction>();

        public override void ApplyStatus(Monster monster, int count)
        {
            base.ApplyStatus(monster, count);

            if (!MonsterActions.ContainsKey(monster))
            {
                UnityAction action = delegate { monster.GetStatusEffect(this); };
                MonsterActions.Add(monster, action);

                FindObjectsOfType<MonsterController>().First(m => m.HasMonster(monster))
                    .AddListenerToTurnStateMachine(TurnStateEnum.PreTurn, action);
            }
        }

        public override void RemoveStatus(Monster monster)
        {
            base.RemoveStatus(monster);

            FindObjectsOfType<MonsterController>().First(m => m.HasMonster(monster))
                .RemoveListenerToTurnStateMachine(TurnStateEnum.PreTurn, MonsterActions[monster]);

            MonsterActions.Remove(monster);
        }

        public override void DoEffect(Monster monster, int count)
        {
            if(count > 1)
            {
                UserMessage.Instance.SendMessageToUser($"{monster.name} is {name} for {count} more turn(s)");
            }
            monster.ApplyStatus(this, -1);
        }

        public override string GetTooltip(int count) => $"{Description}. Lasts for {count} turns;";

        public override string GetTooltipHeader(int count) => name;
    }
}
