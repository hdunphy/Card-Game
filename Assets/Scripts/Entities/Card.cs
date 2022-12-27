using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.UI.Controller;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Card : MonoBehaviour
    {
        private ICardUI UIController;
        private CardData Data { get; set; }
        public bool PlayedThisTurn { get; private set; }
        public int EnergyCost => Data.EnergyCost;

        public event Action<bool> ToggleInteractions;
        public event Action<int> OnSiblingIndexChanged;

        public void SetSiblingIndex(int i) => OnSiblingIndexChanged?.Invoke(i);

        private void Awake()
        {
            UIController = GetComponentInChildren<ICardUI>();
        }

        public CardAlignment CardAlignment => Data.CardAlignment;

        public IEnumerator InvokeActionCoroutine(Mingming source, Mingming target)
        {
            PlayedThisTurn = true;

            yield return null;
            ToggleInteractions?.Invoke(false);

            yield return InvokeActions(source.Logic, target.Logic);

            EventManager.Instance.OnDiscardCardTrigger(this);
        }

        public IEnumerator InvokeActions(MingmingBattleLogic source, MingmingBattleLogic target) => Data.InvokeActionCoroutine(source, target, this);

        public bool CanUseCard(MingmingBattleLogic source) => !PlayedThisTurn && Data.CardConstraint.CanUseCard(source, this);

        public bool IsValidTarget(Mingming source, Mingming target) => Data.TargetType.IsValidTarget(source, target, this);

        public bool IsValidAction(Mingming source, Mingming target) => IsValidTarget(source, target) && CanUseCard(source.Logic);

        public void SetCardData(CardData _data)
        {
            Data = _data;
            name = Data.CardName;
            UIController.SetCardData(_data);
        }

        public void DiscardCard(Vector3 position, Vector3 scale, float CardMovementTiming, Action onComplete)
        {
            ToggleInteractions(false);

            LeanTween.cancel(gameObject);
            transform.SetAsLastSibling();
            LeanTween.move(gameObject, position, CardMovementTiming).setDelay(0.5f);
            LeanTween.scale(gameObject, scale, CardMovementTiming).setDelay(0.5f).setOnComplete(() => { onComplete.Invoke(); SetInactive(); });
        }

        private void SetInactive()
        {
            transform.SetParent(null);
            gameObject.SetActive(false);
        }

        public Card DrawCard(Transform parentLocation)
        {
            transform.SetParent(parentLocation);
            gameObject.SetActive(true);

            return this;
        }

        public void MoveToHandPosition(Vector2 position, Vector3 scale, float CardMovementTiming)
        {
            ToggleInteractions?.Invoke(false);
            RectTransform cardTransform = GetComponent<RectTransform>();
            LeanTween.move(cardTransform, position, CardMovementTiming);
            LeanTween.scale(cardTransform, scale, CardMovementTiming)
                .setOnComplete(SetCardInHand);
        }

        void SetCardInHand()
        {
            ToggleInteractions?.Invoke(true);
            PlayedThisTurn = false;
        }

        public int GetCardScore() => Data.GetCardScore();
    }
}