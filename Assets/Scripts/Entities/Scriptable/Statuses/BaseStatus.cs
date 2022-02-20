using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class BaseStatus : ScriptableObject
    {
        [SerializeField] protected Sprite sprite;

        public virtual Sprite GetSprite(int count) => sprite;

        public virtual void ApplyStatus(Monster monster, int count)
        {
            monster.ApplyStatus(this, count);
        }

        public virtual void RemoveStatus(Monster monster)
        {
        }

        public abstract void DoEffect(Monster monster, int count);

        public abstract string GetTooltip(int count);

        public abstract string GetTooltipHeader(int count);
    }
}
