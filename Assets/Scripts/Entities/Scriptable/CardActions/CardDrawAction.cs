using Assets.Scripts.References;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Card Draw", menuName = "Data/Card Action/Create Card Draw")]
    public class CardDrawAction : CardAction
    {
        [SerializeField] private int numberOfCards;

        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            FindObjectsOfType<MonsterController>().First(m => m.HasMonster(source)).DrawCards(numberOfCards);
        }
    }
}