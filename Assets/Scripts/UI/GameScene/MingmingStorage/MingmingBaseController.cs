using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.UI.Tooltips;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingBaseController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] protected Image mingmingSprite;
        [SerializeField] protected TMP_Text nameText;
        [SerializeField] protected TMP_Text levelText;
        [SerializeField] protected AlignmentTooltipComponent primaryAlignment;
        [SerializeField] protected AlignmentTooltipComponent secondaryAlignment;

        public MingmingInstance Mingming { get; private set; }
        protected Action<MingmingInstance> _onSelected;

        public virtual void Setup(MingmingInstance mingming, Action<MingmingInstance> onSelected)
        {
            Mingming = mingming;
            _onSelected= onSelected;

            mingmingSprite.sprite = mingming.Sprite;
            nameText.text = mingming.Name;
            levelText.text = mingming.Level.ToString();

            primaryAlignment.SetData(mingming.BaseData);
            secondaryAlignment.SetData(mingming.BaseData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _onSelected.Invoke(Mingming);
        }
    }
}