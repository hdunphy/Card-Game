using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Stat Modifier Status", menuName = "Data/Status/Create Stat Modifier Status")]
    public class StatModiferStatus : BaseStatus
    {
        [SerializeField, Range(1, 2)] private float Modifier;
        [SerializeField] private Sprite PositiveSprite;
        [SerializeField] private Sprite NegativeSprite;
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

        public override void ApplyStatus(MingmingBattleLogic mingming, int count)
        {
            var value = (float)ModifierProperty.GetValue(mingming);
            var changeValue = Modifier * Mathf.Abs(count);

            base.ApplyStatus(mingming, count);
            if (count > 0)
            {
                ModifierProperty.SetValue(mingming, value * changeValue);
            }
            else
            {
                ModifierProperty.SetValue(mingming, value / changeValue);
            }
        }

        public override void RemoveStatus(MingmingBattleLogic mingming)
        {
            ModifierProperty.SetValue(mingming, 1);
            base.RemoveStatus(mingming);
        }

        public override void DoEffect(MingmingBattleLogic mingming) {}

        public override Sprite GetSprite(int count)
        {
            sprite = count > 0 ? PositiveSprite : NegativeSprite;
            return sprite;
        }

        public override string GetTooltip(int count)
            => $"Modified {GetTooltipHeader(count)} by {Modifier * count}";

        public override string GetTooltipHeader(int count) => count > 0 ? PositiveName : NegativeName;

        public override bool Equals(object obj)
        {
            return obj is StatModiferStatus status &&
                   Modifier == status.Modifier &&
                   PropertyName == status.PropertyName;
        }

        public override int GetHashCode()
        {
            int hashCode = 76424289;
            hashCode = hashCode * -1521134295 + Modifier.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
            return hashCode;
        }

        public override int GetScore(int count) => 0;
    }
}
