using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.Constraints
{
    [CreateAssetMenu(menuName = "Data/Constraints/Create Status Count")]
    public class HasStatusConstraintAndCount : BaseConstraint
    {
        [SerializeField] private StatusConstraint StatusConstraint;
        [SerializeField] private bool StatusCountIsPositive;
        public override bool CanUseCard(Mingming source, Card card)
        {
            bool MeetsStatusConstraint = source.Simulation.HasStatus(StatusConstraint.Status) == StatusConstraint.HasStatus;
            bool _isCountPositive = Mathf.Sign(source.Simulation.GetStatusCount(StatusConstraint.Status)) > 0 ;

            if (!MeetsStatusConstraint)
            {
                string canHave = StatusConstraint.HasStatus ? "MUST" : "CANNOT";
                UserMessage.Instance.SendMessageToUser($"{source.name} {canHave} have the status: {StatusConstraint.Status.name}");
            }
            else if(_isCountPositive != StatusCountIsPositive)
            {
                UserMessage.Instance.SendMessageToUser($"{source.name} does not have the correct status.");
            }
            return base.CanUseCard(source, card) && MeetsStatusConstraint && _isCountPositive == StatusCountIsPositive;
        }
    }
}