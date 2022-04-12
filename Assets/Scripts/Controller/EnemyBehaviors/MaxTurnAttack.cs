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
        private List<Card> Hand;
        private IEnumerable<Mingming> OwnedParty;
        private IEnumerable<Mingming> OtherParty;
        private Queue<CardPlay> CardPlays;

        public bool GetNextAttack()
        {
            throw new System.NotImplementedException();
        }

        public void SetTurnStategy(List<Card> hand, IEnumerable<Mingming> ownedParty, IEnumerable<Mingming> otherParty)
        {
            Hand = hand;
            OwnedParty = ownedParty;
            OtherParty = otherParty;

            EnemyBehaviourHelper.CanAttack(OwnedParty, OtherParty, Hand);

            CardPlays = new Queue<CardPlay>();
            int maxScore = 0;

            maxScore = GetBestCard(maxScore, ref CardPlays, new List<Card>(Hand));
        }

        private int GetBestCard(int maxScore, ref Queue<CardPlay> cardPlays, List<Card> remainingCards)
        {
            foreach (var card in Hand)
            {
                var _cardPlays = new Queue<CardPlay>();
                int _maxScore = GetBestTurn(maxScore, card, _cardPlays, remainingCards);
                if (_maxScore > maxScore)
                {
                    maxScore = _maxScore;
                    cardPlays = _cardPlays;
                }
            }

            return maxScore;
        }

        private int GetBestTurn(int score, Card currentCard, Queue<CardPlay> cardPlayQueue, List<Card> remainingCards)
        {
            if (!remainingCards.Any())
            {
                return score;
            }

            int maxScore = 0;
            CardPlay cardPlay = new CardPlay();

            foreach(var _source in OwnedParty)
            {
                int _maxScore = GetBestCardPlay(out CardPlay _cardPlay, _source, currentCard);
                if(_maxScore > maxScore)
                {
                    maxScore = _maxScore;
                    cardPlay = _cardPlay;
                }
            }

            score += maxScore;
            remainingCards.Remove(currentCard);
            cardPlayQueue.Enqueue(cardPlay);

            return GetBestCard(score, ref cardPlayQueue, remainingCards);
        }

        private int GetBestCardPlay(out CardPlay cardPlay, Mingming _source, Card currentCard)
        {
            int maxScore = 0;
            cardPlay = new CardPlay();

            foreach(var _target in OtherParty)
            {
                var isValid = currentCard.IsValidAction(_source, _target);
                if (isValid)
                {
                    int _score = 0; //TODO get value;
                    if(_score > maxScore)
                    {
                        maxScore = _score;
                        cardPlay = new CardPlay
                        {
                            Card = currentCard,
                            Source = _source,
                            Target = _target
                        };
                    }
                }
            }

            foreach (var _target in OwnedParty)
            {
                var isValid = currentCard.IsValidAction(_source, _target);
                if (isValid)
                {
                    int _score = 0; //TODO: Get value;
                    if (_score > maxScore)
                    {
                        maxScore = _score;
                        cardPlay = new CardPlay
                        {
                            Card = currentCard,
                            Source = _source,
                            Target = _target
                        };
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

    public static class EnemyBehaviourHelper
    {
        public static bool CanAttack(IEnumerable<Mingming> OwnedParty, IEnumerable<Mingming> OtherParty, IEnumerable<Card> Hand)
        {
            OwnedParty = OwnedParty.Where(x => x.IsInPlay && x.Simulation.EnergyAvailable > 0).ToList();
            OtherParty = OtherParty.Where(x => x.IsInPlay).ToList();

            var targets = new List<Mingming>(OwnedParty);
            targets.AddRange(new List<Mingming>(OtherParty));

            Hand = Hand.Where(card => OwnedParty.Any(own => targets.Any(target => card.IsValidAction(own, target)))).ToList();

            return OwnedParty.Any() && OtherParty.Any() && Hand.Any();
        }
    }
}