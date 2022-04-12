using Assets.Scripts.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Controller.EnemyBehaviors
{
    public class MaxTurnAttack : IEnemyAttackBehavior
    {
        private Queue<CardPlay> CardPlays;

        public bool GetNextAttack()
        {
            throw new System.NotImplementedException();
        }

        public void SetTurnStategy(List<Card> hand, IEnumerable<Mingming> ownedParty, IEnumerable<Mingming> otherParty)
        {
            var canAttack = EnemyBehaviourHelper.CanAttack(ownedParty, otherParty, hand);
            CardPlays = new Queue<CardPlay>();

            if (canAttack)
            {
                int maxScore = 0;
                var turnState = new TurnState(ownedParty, otherParty, hand);

                maxScore = GetBestCard(maxScore, ref CardPlays, turnState);
            }
        }

        private int GetBestCard(int maxScore, ref Queue<CardPlay> cardPlays, TurnState turnState)
        {
            foreach (var card in turnState.RemaingHand)
            {
                var _cardPlays = new Queue<CardPlay>();
                int _maxScore = GetBestTurn(maxScore, card, _cardPlays, turnState.Clone());
                if (_maxScore > maxScore)
                {
                    maxScore = _maxScore;
                    cardPlays = _cardPlays;
                }
            }

            return maxScore;
        }

        private int GetBestTurn(int score, Card currentCard, Queue<CardPlay> cardPlayQueue, TurnState turnState)
        {
            if (!turnState.RemaingHand.Any())
            {
                return score;
            }

            int maxScore = 0;
            CardPlay cardPlay = new CardPlay();

            foreach(var _source in turnState.OwnedMingmings.Keys)
            {
                var isValid = currentCard.CanUseCard(_source);
                if (isValid)
                {
                    var _turnState = turnState.Clone();
                    int _maxScore = GetBestCardPlay(out CardPlay _cardPlay, _source, currentCard, _turnState);
                    if (_maxScore > maxScore)
                    {
                        maxScore = _maxScore;
                        cardPlay = _cardPlay;
                        turnState = _turnState;
                    }
                }
            }

            score += maxScore;
            turnState.RemaingHand.Remove(currentCard);
            cardPlayQueue.Enqueue(cardPlay);

            return GetBestCard(score, ref cardPlayQueue, turnState);
        }

        private int GetBestCardPlay(out CardPlay cardPlay, MingmingBattleLogic _source, Card currentCard, TurnState turnState)
        {
            int maxScore = 0;
            cardPlay = new CardPlay();

            foreach(var _target in turnState.AllTargets)
            {
                var _cardplay = new CardPlay
                {
                    Card = currentCard,
                    Source = turnState.OwnedMingmings[_source],
                    Target = turnState.OtherMingmings[_target]
                };

                if(currentCard.IsValidAction(_cardplay.Source, _cardplay.Target))
                {
                    var _turnSate = turnState.Clone();
                    int _score = _turnSate.ApplyCardPlay(_cardplay);

                    if (_score > maxScore)
                    {
                        maxScore = _score;
                        cardPlay = _cardplay;
                    }
                }
            }

            return maxScore;
        }
    }

    public class CardPlay
    {
        public Card Card { get; set; }
        public Mingming Source { get; set; }
        public Mingming Target { get; set; }
    }

    public class TurnState
    {
        public Dictionary<MingmingBattleLogic, Mingming> OwnedMingmings { get; private set; }

        public Dictionary<MingmingBattleLogic, Mingming> OtherMingmings { get; private set; }

        public IEnumerable<MingmingBattleLogic> AllTargets { get; private set; }

        public List<Card> RemaingHand { get; private set; }

        public TurnState Clone() => new TurnState(this);

        public TurnState(IEnumerable<Mingming> owned, IEnumerable<Mingming> other, IEnumerable<Card> hand)
        {
            OwnedMingmings = owned.ToDictionary(m => new MingmingBattleLogic(m.Logic));
            OtherMingmings = other.ToDictionary(m => new MingmingBattleLogic(m.Logic));
            RemaingHand = new List<Card>(hand);

            GetAllTargets();
        }

        public TurnState(TurnState turnState)
        {
            OwnedMingmings = OwnedMingmings.Values.ToDictionary(m => new MingmingBattleLogic(m.Logic));
            OtherMingmings = OtherMingmings.Values.ToDictionary(m => new MingmingBattleLogic(m.Logic));
            RemaingHand = new List<Card>(turnState.RemaingHand);

            GetAllTargets();
        }

        private void GetAllTargets()
        {
            AllTargets = OtherMingmings.Keys;
            AllTargets.ToList().AddRange(OwnedMingmings.Keys);
        }

        public int ApplyCardPlay(CardPlay cardplay)
        {
            throw new NotImplementedException("Need to apply card effects to simulation");
        }
    }

    public static class EnemyBehaviourHelper
    {
        public static bool CanAttack(IEnumerable<Mingming> OwnedParty, IEnumerable<Mingming> OtherParty, IEnumerable<Card> Hand)
        {
            OwnedParty = OwnedParty.Where(x => x.IsInPlay && x.Logic.EnergyAvailable > 0).ToList();
            OtherParty = OtherParty.Where(x => x.IsInPlay).ToList();

            var targets = new List<Mingming>(OwnedParty);
            targets.AddRange(new List<Mingming>(OtherParty));

            Hand = Hand.Where(card => OwnedParty.Any(own => targets.Any(target => card.IsValidAction(own, target)))).ToList();

            return OwnedParty.Any() && OtherParty.Any() && Hand.Any();
        }
    }
}