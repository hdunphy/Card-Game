using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Burn Status", menuName = "Data/Status/Create Burn Status")]
    public class BurnStatus : BaseStatus
    {
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
            int dmg = GetDamage(monster.TotalHealth, count);
            monster.TakeDamage(dmg, null); //TODO: pass source some how
        }

        protected virtual int GetDamage(int health, int count)
        {
            var damage = health * GetModifier(count);
            return Mathf.Clamp(Mathf.FloorToInt(damage), 1, health);
        }

        float GetModifier(int count) => count switch
        {
            1 => 0.025f,
            2 => 0.05f,
            _ => 0.08f
        };

        public override string GetTooltip(int count)
            => $"Does {GetModifier(count) * 100}% of Monster's health at start of each turn";

        public override string GetTooltipHeader(int count) => "Burn";
    }
}
