using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Burn Status", menuName = "Data/Status/Create Burn Status")]
    public class BurnStatus : EffectStatus
    {
        private const int MAX_COUNT = 3;

        public override TurnStateEnum TurnState => TurnStateEnum.PreTurn;

        public override void ApplyStatus(MingmingBattleLogic mingming, int count)
        {
            int currentCount = mingming.GetStatusCount(this);
            int _count = Mathf.Clamp(count, count, MAX_COUNT - currentCount);

            base.ApplyStatus(mingming, _count);
        }

        public override void DoEffect(MingmingBattleLogic mingming)
        {
            int count = mingming.GetStatusCount(this);
            int dmg = GetDamage(mingming.TotalHealth, count);
            mingming.TakeDamage(dmg, null);
            UserMessage.Instance.SendMessageToUser($"{mingming.Name} took {dmg} {GetTooltipHeader(count)} damage");
        }

        private int GetDamage(int health, int count)
        {
            var damage = health * GetModifier(count);
            return Mathf.Clamp(Mathf.FloorToInt(damage), 1, health);
        }

        protected virtual float GetModifier(int count) => count switch
        {
            1 => 0.05f,
            2 => 0.12f,
            _ => 0.25f
        };

        public override string GetTooltip(int count)
            => $"Does {GetModifier(count) * 100}% of Mingming's health at start of each turn";

        public override string GetTooltipHeader(int count) => "Burn";
    }
}
