using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Status", menuName = "Data/Status/Create Named Status")]
    public class NamedStatus : BaseStatus
    {
        [SerializeField] private new string name;
        [SerializeField] private string Description;
        [SerializeField] private int Length;

        public override void ApplyStatus(Monster monster, int count)
        {
            base.ApplyStatus(monster, count);
            FindObjectsOfType<MonsterController>().First(m => m.HasMonster(monster))
                .AddListenerToTurnStateMachine(TurnStateEnum.PreTurn, delegate { monster.GetStatusEffect(this); });
        }

        public override void RemoveStatus(Monster monster)
        {
            base.RemoveStatus(monster);
            FindObjectsOfType<MonsterController>().First(m => m.HasMonster(monster))
                .RemoveListenerToTurnStateMachine(TurnStateEnum.PreTurn, delegate { monster.GetStatusEffect(this); });
        }

        public override void DoEffect(Monster monster, int count)
        {
            monster.ApplyStatus(this, -1);
        }

        public override string GetTooltip(int count) => $"{Description}. Lasts for {count} turns;";

        public override string GetTooltipHeader(int count) => name;
    }
}
