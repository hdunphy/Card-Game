using Assets.Scripts.Entities.Mingmings;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Status", menuName = "Data/Status/Create Named Status")]
    public class NamedStatus : EffectStatus
    {
        [SerializeField] private new string name;
        [SerializeField] private string description;
        [SerializeField] private int maxCount;
        [SerializeField] private int scoreFactor;

        public override TurnStateEnum TurnState => TurnStateEnum.PostTurn;

        public override void ApplyStatus(MingmingBattleLogic mingming, int count)
        {
            int currentCount = mingming.GetStatusCount(this);
            int _count = Mathf.Clamp(count, count, maxCount - currentCount);
            
            base.ApplyStatus(mingming, _count);
        }

        public override void DoEffect(MingmingBattleLogic mingming)
        {
            int count = mingming.GetStatusCount(this);

            if(count > 1)
            {
                UserMessage.Instance.SendMessageToUser($"{mingming.Name} is {name} for {count} more turn(s)");
            }
            mingming.ApplyStatus(this, -1);
        }

        public override int GetScore(int count) => scoreFactor * count;

        public override string GetTooltip(int count) => $"{description}. Lasts for {count} turns;";

        public override string GetTooltipHeader(int count) => name;
    }
}
