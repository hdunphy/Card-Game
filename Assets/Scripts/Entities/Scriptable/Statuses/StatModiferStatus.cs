using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Stat Modifier Status", menuName = "Data/Status/Create Stat Modifier Status")]
    public class StatModiferStatus : BaseStatus
    {
        [SerializeField, Range(1, 2)] public float Modifier;
        [SerializeField] public Sprite PositiveSprite;
        [SerializeField] public Sprite NegativeSprite;
        [SerializeField] private string PropertyName;
        [SerializeField] private string PositiveName;
        [SerializeField] private string NegativeName;

        PropertyInfo ModifierProperty;

        private void OnEnable()
        {
            if(PropertyName == null)
            {
                Debug.LogWarning($"{name} has PropertyName set to null");
                return;
            }

            ModifierProperty = typeof(MingmingBattleLogic).GetProperty(PropertyName);

            if(ModifierProperty == null || ModifierProperty.PropertyType != typeof(float))
            {
                Debug.LogError($"Passed incorrect Property Name: {PropertyName}");
            }
        }

        public override void ApplyStatus(Mingming mingming, int count)
        {
            MingmingBattleLogic mingmingSimulation = mingming.Logic;
            var value = (float)ModifierProperty.GetValue(mingmingSimulation);
            var changeValue = Modifier * Mathf.Abs(count);

            base.ApplyStatus(mingming, count);
            if (count > 0)
            {
                ModifierProperty.SetValue(mingmingSimulation, value * changeValue);
            }
            else
            {
                ModifierProperty.SetValue(mingmingSimulation, value / changeValue);
            }
        }

        public override void RemoveStatus(Mingming mingming)
        {
            ModifierProperty.SetValue(mingming.Logic, 1);
        }

        public override void DoEffect(Mingming mingming, int count) {}

        public override Sprite GetSprite(int count)
        {
            sprite = count > 0 ? PositiveSprite : NegativeSprite;
            return sprite;
        }

        public override string GetTooltip(int count)
            => $"Modified {GetTooltipHeader(count)} by {Modifier * count}";

        public override string GetTooltipHeader(int count) => count > 0 ? PositiveName : NegativeName;
    }
}
