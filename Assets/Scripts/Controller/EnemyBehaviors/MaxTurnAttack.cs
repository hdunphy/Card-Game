﻿using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Mingmings;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Controller.EnemyBehaviors
{
    public class MaxTurnAttack : IEnemyAttackBehavior
    {
        private Queue<CardPlay> CardPlays;

        public bool GetNextAttack()
        {
            bool canPlay = CardPlays.Any();

            if (canPlay)
            {
                var cardPlay = CardPlays.Dequeue();

                if (cardPlay.IsValidAction())
                {
                    cardPlay.Play();
                }
                else
                {
                    canPlay = false;
                }
            }

            return canPlay;
        }

        public void SetTurnStrategy(List<Card> hand, IEnumerable<Mingming> ownedParty, IEnumerable<Mingming> otherParty)
        {
            var canAttack = EnemyBehaviourHelper.CanAttack(ownedParty, otherParty, hand);
            CardPlays = new Queue<CardPlay>();

            if (canAttack)
            {
                UserMessage.Instance.CanSendMessage = false;

                var turnState = new TurnState(ownedParty, otherParty, hand);

                var maxScore = GetBestCardPlay(turnState, ref CardPlays, int.MinValue);

                UserMessage.Instance.CanSendMessage = true;
            }
        }

        public int GetBestCardPlay(TurnState turnState, ref Queue<CardPlay> cardPlays, int maxScore)
        {
            var bestCardPlays = new Queue<CardPlay>(cardPlays);

            foreach(var card in turnState.RemaingHand)
            {
                var sources = turnState.OwnedMingmings.Where(m => card.CanUseCard(m));

                foreach (var source in sources)
                {
                    var sourceMingming = turnState.GetMingming(source);
                    var allTargets = turnState.AllTargets.Where(t => card.IsValidTarget(sourceMingming, turnState.GetMingming(t)));
                
                    foreach (var target in allTargets)
                    {
                        var _turnState = turnState.Clone();
                        var cardplay = new CardPlay
                        {
                            Card = card,
                            Source = turnState.GetMingming(source),
                            Target = turnState.GetMingming(target)
                        };

                        var score = _turnState.ApplyCardPlay(cardplay);

                        var _cardPlays = new Queue<CardPlay>(cardPlays);
                        _cardPlays.Enqueue(cardplay);

                        //want to get the final score
                        if (_turnState.HasUsableCards())
                        {
                            score = GetBestCardPlay(_turnState, ref _cardPlays, score);
                        }

                        if(score > maxScore)
                        {
                            bestCardPlays = _cardPlays;
                            maxScore = score;
                        }
                    }
                }
            }

            cardPlays = bestCardPlays;
            return maxScore;
        }
    }

    public class CardPlay
    {
        public Card Card { get; set; }
        public Mingming Source { get; set; }
        public Mingming Target { get; set; }

        public bool IsValidAction() => Card.IsValidAction(Source, Target);

        public void Play()
        {
            UserMessage.Instance.CanSendMessage = true;

            EventManager.Instance.OnSelectMingmingTrigger(Source);
            EventManager.Instance.OnSelectTargetTrigger(Target, Card);

            EventManager.Instance.OnSelectMingmingTrigger(Source); //to deselect

            //disable user message so not to get bombarded by failed attempts
            UserMessage.Instance.CanSendMessage = false;
        }
    }

    public class TurnState
    {
        public Dictionary<int, Mingming> Mingmings { get; private set; }

        public List<MingmingBattleLogic> OwnedMingmings { get; private set; }

        public List<MingmingBattleLogic> OtherMingmings { get; private set; }

        public IEnumerable<MingmingBattleLogic> AllTargets => OwnedMingmings.Union(OtherMingmings);

        public List<Card> RemaingHand { get; private set; }

        public TurnState Clone() => new(this);

        public TurnState(IEnumerable<Mingming> owned, IEnumerable<Mingming> other, IEnumerable<Card> hand)
        {
            var _mingmings = owned.Union(other);
            foreach(var mingming in _mingmings)
            {
                mingming.Logic.Id = mingming.GetInstanceID();
            }
            Mingmings = _mingmings.ToDictionary(m => m.Logic.Id);
            OwnedMingmings = owned.Where(m => m.IsInPlay).Select(m => m.Logic).ToList();
            OtherMingmings = other.Where(m => m.IsInPlay).Select(m => m.Logic).ToList();

            RemaingHand = new List<Card>(hand);
        }

        public TurnState(TurnState turnState)
        {
            OwnedMingmings = turnState.OwnedMingmings
                .Select(m => new MingmingBattleLogic(m)).ToList();
            OtherMingmings = turnState.OtherMingmings
                .Select(m => new MingmingBattleLogic(m)).ToList();
            Mingmings = turnState.Mingmings;

            RemaingHand = new List<Card>(turnState.RemaingHand);
        }

        public bool HasUsableCards()
        {
            var OwnedParty = OwnedMingmings.Where(x => x.CurrentHealth > 0 && x.EnergyAvailable > 0).ToList();

            RemaingHand = RemaingHand.Where(card => OwnedParty.Any(own => card.CanUseCard(own))).ToList();

            return OwnedParty.Any() && RemaingHand.Any();
        }

        public int ApplyCardPlay(CardPlay cardplay)
        {
            var source = OwnedMingmings.Single(m => m.Id == cardplay.Source.GetInstanceID());
            var target = AllTargets.Single(m => m.Id == cardplay.Target.GetInstanceID());

            var actions = cardplay.Card.InvokeActions(source, target);

            while (actions.MoveNext())
            {
                var test = actions.Current;
            }

            var score = GetScore() + cardplay.Card.GetCardScore();

            RemaingHand.Remove(cardplay.Card);
            OwnedMingmings = OwnedMingmings.Where(m => m.CurrentHealth > 0).ToList();
            OtherMingmings = OtherMingmings.Where(m => m.CurrentHealth > 0).ToList();

            return score;
        }

        private int GetScore()
        {
            int score = 0;
            foreach(var owned in OwnedMingmings)
            {
                score += owned.GetCurrentStateScore();
            }

            foreach(var other in OtherMingmings)
            {
                score -= other.GetCurrentStateScore();
            }

            return score;
        }

        public Mingming GetMingming(MingmingBattleLogic logic)
        {
            return Mingmings[logic.Id];
        }
    }
}