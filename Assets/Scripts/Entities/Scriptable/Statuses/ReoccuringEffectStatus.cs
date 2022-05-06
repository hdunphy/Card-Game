using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.Statuses
{
    public class ReoccuringEffectStatus : EffectStatus
    {
        [Header("Reoccuring Effect Params")]
        [SerializeField] private TurnStateEnum turnState;
        [SerializeField] private BaseConstraint constraint;
        [SerializeField] private CardAction action;
        [SerializeField] private CardAlignment alignment;

        public override TurnStateEnum TurnState => turnState;

        public override void DoEffect(MingmingBattleLogic mingming)
        {
            if (constraint.MingmingMeetsConstraint(mingming))
            {
                action.InvokeAction(mingming, mingming, alignment);
            }
        }

        public override int GetScore(int count)
        {
            throw new NotImplementedException();
        }

        public override string GetTooltip(int count)
        {
            throw new NotImplementedException();
        }

        public override string GetTooltipHeader(int count)
        {
            throw new NotImplementedException();
        }
    }
}
