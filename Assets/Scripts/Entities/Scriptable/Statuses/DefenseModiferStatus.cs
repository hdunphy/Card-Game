using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Defense Status", menuName = "Data/Status/Create Defense Modifier Status")]
    public class DefenseModiferStatus : BaseStatus
    {
        [SerializeField, Range(1, 2)] public float Modifier;
        [SerializeField] public Sprite PositiveSprite;
        [SerializeField] public Sprite NegativeSprite;

        public override void ApplyStatus(Monster monster, int count)
        {
            base.ApplyStatus(monster, count);
            if (count > 0)
            {
                monster.DefenseModifier *= Modifier * count;
            }
            else
            {
                monster.DefenseModifier /= Modifier * count;
            }
        }

        public override void RemoveStatus(Monster monster)
        {
            monster.DefenseModifier = 1;
        }

        public override void DoEffect(Monster monster, int count) {}

        public override Sprite GetSprite(int count)
        {
            sprite = count > 0 ? PositiveSprite : NegativeSprite;
            return sprite;
        }

        public override string GetTooltip(int count)
            => $"Modified Defense by {Modifier * count}";

        public override string GetTooltipHeader(int count) => count > 0 ? "Sharp" : "Dazed";
    }
}
