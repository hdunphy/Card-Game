using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class BaseStatus : ScriptableObject
    {
        [SerializeField] protected Sprite sprite;

        public virtual Sprite GetSprite(int count) => sprite;

        public virtual void ApplyStatus(Mingming mingming, int count)
        {
            mingming.ApplyStatus(this, count);
        }

        public virtual void RemoveStatus(Mingming mingming)
        {
        }

        public abstract void DoEffect(Mingming mingming, int count);

        public abstract string GetTooltip(int count);

        public abstract string GetTooltipHeader(int count);
    }
}
