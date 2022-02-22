using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Multiple Status Constraint", menuName = "Data/Constraints/Create Multiple Statuses")]
    public class MultipleStatusConstraints : BaseConstraint
    {
        [SerializeField] private List<StatusConstraint> StatusConstraints;
        public override bool CheckConstraint(Monster source, Card card)
        {
            bool isValid = base.CheckConstraint(source, card);

            foreach(var StatusConstraint in StatusConstraints)
            {
                isValid = isValid && (source.HasStatus(StatusConstraint.Status) == StatusConstraint.HasStatus);
            }

            return isValid;
        }
    }
}
