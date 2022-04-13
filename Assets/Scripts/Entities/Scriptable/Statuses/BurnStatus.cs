using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Burn Status", menuName = "Data/Status/Create Burn Status")]
    public class BurnStatus : BaseStatus
    {
        private Dictionary<Mingming, UnityAction> MingmingActions = new Dictionary<Mingming, UnityAction>();

        public override void ApplyStatus(MingmingBattleLogic mingming, int count)
        {
            base.ApplyStatus(mingming, count);

            if (!MingmingActions.ContainsKey(mingming))
            {
                UnityAction action = delegate { mingming.GetStatusEffect(this); };
                MingmingActions.Add(mingming, action);

                FindObjectsOfType<PartyController>().First(m => m.HasMingming(mingming))
                    .AddListenerToTurnStateMachine(TurnStateEnum.PreTurn, action);
            }
        }

        public UnityAction GetEffect(MingmingBattleLogic mingming)
        {
            return delegate { DoEffect(mingming, mingming.GetStatusCount(this)); };
        }

        public override void RemoveStatus(MingmingBattleLogic mingming)
        {
            base.RemoveStatus(mingming);

            FindObjectsOfType<PartyController>().First(m => m.HasMingming(mingming))
                .RemoveListenerFromTurnStateMachine(TurnStateEnum.PreTurn, MingmingActions[mingming]);
            
            MingmingActions.Remove(mingming);
        }

        public override void DoEffect(MingmingBattleLogic mingming, int count)
        {
            int dmg = GetDamage(mingming.TotalHealth, count);
            mingming.TakeDamage(dmg, null); //TODO: pass source some how
            UserMessage.Instance.SendMessageToUser($"{mingming.Name} took {dmg} {GetTooltipHeader(count)} damage");
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
            => $"Does {GetModifier(count) * 100}% of Mingming's health at start of each turn";

        public override string GetTooltipHeader(int count) => "Burn";
    }
}
