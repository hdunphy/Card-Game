using Assets.Scripts.Entities.Mingmings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Multiple Status Constraint", menuName = "Data/Constraints/Create Multiple Statuses")]
    public class MultipleStatusConstraints : BaseConstraint
    {
        [SerializeField] private List<StatusConstraint> StatusConstraints;

        public override bool MingmingMeetsConstraint(MingmingBattleLogic source)
        {
            bool meetsStatusConstraint = StatusConstraints.All(sc => source.HasStatus(sc.Status) == sc.HasStatus);

            if (!meetsStatusConstraint)
            {
                var statusConstraint = StatusConstraints.FirstOrDefault(sc => source.HasStatus(sc.Status) != sc.HasStatus);
                string canHave = statusConstraint.HasStatus ? "MUST" : "CANNOT";
                UserMessage.Instance.SendMessageToUser($"{source.Name} {canHave} have the status: {statusConstraint.Status.name}");
            }

            return meetsStatusConstraint;
        }
    }
}
