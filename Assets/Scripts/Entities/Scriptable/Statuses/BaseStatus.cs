using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class BaseStatus : ScriptableObject
    {
        [SerializeField] protected Sprite sprite;

        public virtual Sprite GetSprite(int count) => sprite;

        public virtual void ApplyStatus(MingmingBattleLogic mingming, int count)
        {
            mingming.ApplyStatus(this, count);
        }

        public virtual void RemoveStatus(MingmingBattleLogic mingming)
        {
            mingming.RemoveStatus(this);
        }

        public abstract void DoEffect(MingmingBattleLogic mingming);

        public abstract string GetTooltip(int count);

        public abstract string GetTooltipHeader(int count);

        public abstract int GetScore(int count);
    }
}