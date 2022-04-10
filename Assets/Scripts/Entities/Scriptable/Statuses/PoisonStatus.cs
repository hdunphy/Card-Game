using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.Statuses
{
    [CreateAssetMenu(fileName = "Poison Status", menuName = "Data/Status/Create Poison Status")]
    public class PoisonStatus : BurnStatus
    {
        [SerializeField] private int damage; //damage per count

        public override void DoEffect(Mingming mingming, int count)
        {
            base.DoEffect(mingming, count);
            mingming.ApplyStatus(this, -1);
        }

        protected override int GetDamage(int health, int count) => count * damage;

        public override string GetTooltip(int count) =>
            $"Deals {count} * {damage} at the start of the turn. Decreases count every turn";

        public override string GetTooltipHeader(int count) => "Poison";
    }
}
