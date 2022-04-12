using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Multiple Status Constraint", menuName = "Data/Constraints/Create Multiple Statuses")]
    public class MultipleStatusConstraints : BaseConstraint
    {
        [SerializeField] private List<StatusConstraint> StatusConstraints;
        public override bool CanUseCard(MingmingBattleSimulation source, Card card)
        {
            bool isValid = base.CanUseCard(source, card);

            foreach(var StatusConstraint in StatusConstraints)
            {
                bool meetsStatusConstraint = source.HasStatus(StatusConstraint.Status) == StatusConstraint.HasStatus;

                if (!meetsStatusConstraint)
                {
                    string canHave = StatusConstraint.HasStatus ? "MUST" : "CANNOT";
                    UserMessage.Instance.SendMessageToUser($"{source.Name} {canHave} have the status: {StatusConstraint.Status.name}");
                }
                isValid = isValid && meetsStatusConstraint;
            }

            return isValid;
        }
    }
}
