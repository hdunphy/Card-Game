using Assets.Scripts.Entities.Mingmings;
using UnityEngine.Events;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class EffectStatus : BaseStatus
    {
        public abstract TurnStateEnum TurnState { get; }

        public virtual UnityAction GetEffect(MingmingBattleLogic mingming)
        {
            return delegate { DoEffect(mingming); };
        }
    }
}
