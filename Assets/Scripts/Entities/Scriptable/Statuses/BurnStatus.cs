using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public class BurnStatus : BaseStatus
    {
        public override void ApplyStatus(Monster monster)
        {
            base.ApplyStatus(monster);
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
            int dmg = GetDamage(monster.TotalHealth, count);
            monster.TakeDamage(dmg, null);
        }

        int GetDamage(int health, int count)
        {
            var damage = count switch
            {
                1 => health * 0.025f,
                2 => health * 0.05f,
                _ => health * 0.08f
            };
            return Mathf.Clamp(Mathf.FloorToInt(damage), 1, health);
        }
    }
}
