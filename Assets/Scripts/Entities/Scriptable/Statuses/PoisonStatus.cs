using Assets.Scripts.Entities.Mingmings;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.Statuses
{
    [CreateAssetMenu(fileName = "Poison Status", menuName = "Data/Status/Create Poison Status")]
    public class PoisonStatus : BurnStatus
    {
        public override void DoEffect(MingmingBattleLogic mingming)
        {
            base.DoEffect(mingming);
            mingming.ApplyStatus(this, -1);
        }

        protected override float GetModifier(int count) => count * 0.05f;

        public override string GetTooltip(int count) =>
            $"{base.GetTooltip(count)}. Decreases count every turn";

        public override string GetTooltipHeader(int count) => "Poison";
    }
}
