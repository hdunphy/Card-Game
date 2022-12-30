using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.References;
using UnityEngine;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingStatsPanel : MonoBehaviour
    {
        [SerializeField] private StatContainerController statContainerPrefab;

        public void Setup(MingmingInstance mingming)
        {
            var attackStatContainer = Instantiate(statContainerPrefab, transform);
            attackStatContainer.Setup(new("ATK", mingming.Attack, Rules.CalculateStat(mingming.BaseData.Attack, MingmingInstance.MAX_MODIFIER, mingming.Level)));
            var defenseStatContainer = Instantiate(statContainerPrefab, transform);
            defenseStatContainer.Setup(new("DEF", mingming.Defense, Rules.CalculateStat(mingming.BaseData.Defense, MingmingInstance.MAX_MODIFIER, mingming.Level)));
            var healthStatContainer = Instantiate(statContainerPrefab, transform);
            healthStatContainer.Setup(new("HP", mingming.Health, Rules.CalculateStat(mingming.BaseData.Health, MingmingInstance.MAX_MODIFIER, mingming.Level, true)));
            var energyStatContainer = Instantiate(statContainerPrefab, transform);
            energyStatContainer.Setup(new("NRG", mingming.Energy, MingmingData.MAX_ENERGY));
            var cardDrawStatContainer = Instantiate(statContainerPrefab, transform);
            cardDrawStatContainer.Setup(new("DRW", mingming.CardDraw, MingmingData.MAX_CARD_DRAW));
        }
    }
}