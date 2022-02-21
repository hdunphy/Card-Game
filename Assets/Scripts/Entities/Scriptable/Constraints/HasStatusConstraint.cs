using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Status Constraint", menuName = "Data/Constraints/Create Single Status")]
    public class HasStatusConstraint : BaseConstraint
    {
        [SerializeField] private StatusConstraint StatusConstraint;
        public override bool CheckConstraint(Monster source, Card card)
            => base.CheckConstraint(source, card) && (source.HasStatus(StatusConstraint.Status) ^ StatusConstraint.HasStatus);
    }
}
