using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Status Constraint", menuName = "Data/Constraints/Create Single Status")]
    public class HasStatusConstraint : BaseConstraint
    {
        [SerializeField] private StatusConstraint StatusConstraint;

        public override bool MingmingMeetsConstraint(MingmingBattleLogic source)
        {
            bool MeetsStatusConstraint = source.HasStatus(StatusConstraint.Status) == StatusConstraint.HasStatus;

            if (!MeetsStatusConstraint)
            {
                string canHave = StatusConstraint.HasStatus ? "MUST" : "CANNOT";
                UserMessage.Instance.SendMessageToUser($"{source.Name} {canHave} have the status: {StatusConstraint.Status.name}");
            }

            return MeetsStatusConstraint;
        }
    }
}
