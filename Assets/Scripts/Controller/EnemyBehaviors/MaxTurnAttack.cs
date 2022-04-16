using Assets.Scripts.Entities;
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

        public void SetTurnStategy(List<Card> hand, IEnumerable<Mingming> ownedParty, IEnumerable<Mingming> otherParty)
        {
            var canAttack = EnemyBehaviourHelper.CanAttack(ownedParty, otherParty, hand);
            CardPlays = new Queue<CardPlay>();

            if (canAttack)
            {
                int maxScore = int.MinValue;
                var turnState = new TurnState(ownedParty, otherParty, hand);

                //scores not being added correctly check get best turn
                /*
                 * Should go something like:
                 * Foreach card in hand //pick best start
                 *  While still has energy
                 *      recursively score += GetBestTurn
                 *      
                 *  GetBestTurn:
                 */
                maxScore = GetBestCard(maxScore, ref CardPlays, turnState);
                
            }
        }

        private int GetBestCard(int score, ref Queue<CardPlay> cardPlays, TurnState turnState)
        {
            int maxScore = int.MinValue;
            foreach (var card in turnState.RemaingHand)
            {
                var _cardPlays = new Queue<CardPlay>();
                int _maxScore = GetBestSource(maxScore, card, _cardPlays, turnState.Clone());
                if (_maxScore > maxScore)
                {
                    maxScore = _maxScore;
                    cardPlays = _cardPlays;
                }
            }

            score += maxScore;
            return score;
        }

        private int GetBestSource(int maxScore, Card currentCard, Queue<CardPlay> cardPlayQueue, TurnState turnState)
        {
            if (!turnState.RemaingHand.Any())
            {
                return maxScore;
            }

            CardPlay cardPlay = new CardPlay();

            foreach (var _source in turnState.OwnedMingmings.Keys)
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

            turnState.RemaingHand.Remove(currentCard);
            cardPlayQueue.Enqueue(cardPlay);

            return GetBestCard(maxScore, ref cardPlayQueue, turnState);
        }

        private int GetBestCardPlay(out CardPlay cardPlay, MingmingBattleLogic _source, Card currentCard, TurnState turnState)
        {
            int maxScore = int.MinValue;
            cardPlay = new CardPlay();

            foreach (var _target in turnState.AllTargets)
            {
                if(!turnState.OtherMingmings.TryGetValue(_target, out Mingming targetMingming))
                {
                    targetMingming = turnState.OwnedMingmings[_target];
                }

                var _cardplay = new CardPlay
                {
                    Card = currentCard,
                    Source = turnState.OwnedMingmings[_source],
                    Target = targetMingming
                };

                if (currentCard.IsValidAction(_cardplay.Source, _cardplay.Target))
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
        public Dictionary<MingmingBattleLogic, Mingming> OwnedMingmings { get; private set; }

        public Dictionary<MingmingBattleLogic, Mingming> OtherMingmings { get; private set; }

        public List<MingmingBattleLogic> AllTargets { get; private set; }

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
            OwnedMingmings = turnState.OwnedMingmings.Values.ToDictionary(m => new MingmingBattleLogic(m.Logic));
            OtherMingmings = turnState.OtherMingmings.Values.ToDictionary(m => new MingmingBattleLogic(m.Logic));
            RemaingHand = new List<Card>(turnState.RemaingHand);

            GetAllTargets();
        }

        private void GetAllTargets()
        {
            AllTargets = OtherMingmings.Keys.Union(OwnedMingmings.Keys).ToList();
        }

        public int ApplyCardPlay(CardPlay cardplay)
        {
            var source = AllTargets.First(x => x.Equals(cardplay.Source.Logic));
            var target = AllTargets.First(x => x.Equals(cardplay.Target.Logic));

            var actions = cardplay.Card.InvokeActions(source, target);

            while (actions.MoveNext())
            {
                var test = actions.Current;
            }

            RemaingHand.Remove(cardplay.Card);

            return GetScore();
        }

        private int GetScore()
        {
            int score = 0;
            foreach(var owned in OwnedMingmings.Keys)
            {
                score += owned.GetCurrentStateScore();
            }

            foreach(var other in OtherMingmings.Keys)
            {
                score -= other.GetCurrentStateScore();
            }

            return score;
        }
    }
}