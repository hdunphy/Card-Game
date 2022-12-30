using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingBaseController : MonoBehaviour
    {
        [SerializeField] protected Image mingmingSprite;
        [SerializeField] protected TMP_Text nameText;
        [SerializeField] protected TMP_Text levelText;
        [SerializeField] protected AlignmentTooltipComponent primaryAlignment;
        [SerializeField] protected AlignmentTooltipComponent secondaryAlignment;

        public virtual void Setup(MingmingInstance mingming)
        {
            mingmingSprite.sprite = mingming.Sprite;
            nameText.text = mingming.Name;
            levelText.text = mingming.Level.ToString();

            primaryAlignment.SetData(mingming.BaseData);
            secondaryAlignment.SetData(mingming.BaseData);
        }
    }
}