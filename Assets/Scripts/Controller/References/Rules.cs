using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Assets.Scripts.Entities.Mingmings;

namespace Assets.Scripts.References
{
    public class AlignmentCombination
    {
        public CardAlignment Source { get; set; }
        public CardAlignment Target { get; set; }

        public override bool Equals(object obj)
        {
            return obj is AlignmentCombination combination &&
                   Source == combination.Source &&
                   Target == combination.Target;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class Rules
    {
        public static readonly int HAND_MAX = 9;

        private static readonly float INEFFECTIVE = 0.5f;
        private static readonly float EFFECTIVE = 2f;
        private static readonly float SAME_TYPE_ADVANTAGE = 1.5f;
        private static readonly float SECONDARY_TYPE_ADVANTAGE = 0.75f;

        private static Dictionary<AlignmentCombination, float> AlignmentAdvantageLookup;

        private static readonly Rules instance = new Rules();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Rules()
        {
        }

        private Rules()
        {
            AlignmentAdvantageLookup = new Dictionary<AlignmentCombination, float>
            {
                { new AlignmentCombination { Source = CardAlignment.Fire, Target = CardAlignment.Water }, INEFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Fire, Target = CardAlignment.Earth }, INEFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Fire, Target = CardAlignment.Nature }, EFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Fire, Target = CardAlignment.Ice }, EFFECTIVE },

                { new AlignmentCombination { Source = CardAlignment.Water, Target = CardAlignment.Fire }, EFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Water, Target = CardAlignment.Earth }, EFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Water, Target = CardAlignment.Nature }, INEFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Water, Target = CardAlignment.Ice }, INEFFECTIVE },

                { new AlignmentCombination { Source = CardAlignment.Earth, Target = CardAlignment.Fire }, EFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Earth, Target = CardAlignment.Water }, INEFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Earth, Target = CardAlignment.Earth }, INEFFECTIVE },

                { new AlignmentCombination { Source = CardAlignment.Air, Target = CardAlignment.Fire }, INEFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Air, Target = CardAlignment.Earth }, INEFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Air, Target = CardAlignment.Ice }, EFFECTIVE },

                { new AlignmentCombination { Source = CardAlignment.Nature, Target = CardAlignment.Fire }, INEFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Nature, Target = CardAlignment.Water }, EFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Nature, Target = CardAlignment.Earth }, EFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Nature, Target = CardAlignment.Air }, EFFECTIVE },

                { new AlignmentCombination { Source = CardAlignment.Ice, Target = CardAlignment.Water }, EFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Ice, Target = CardAlignment.Earth }, EFFECTIVE },
                { new AlignmentCombination { Source = CardAlignment.Ice, Target = CardAlignment.Air }, INEFFECTIVE },

                { new AlignmentCombination { Source = CardAlignment.Light, Target = CardAlignment.Darkness }, EFFECTIVE },

                { new AlignmentCombination { Source = CardAlignment.Darkness, Target = CardAlignment.Light }, EFFECTIVE },
            };
        }

        public static Rules Instance
        {
            get
            {
                return instance;
            }
        }

        public int GetAttackDamage(MingmingBattleLogic source, MingmingBattleLogic target, CardAlignment cardAlignment, float power)
        {
            float _modifier = GetModifier(source.GetMingmingAlignment, target.GetMingmingAlignment, cardAlignment);

            float damage = GetLevelModifier(source.Level);
            damage *= (float)power * source.Attack / target.Defense;
            damage = (float)(damage / 40) + 2;

            //Debug.Log($"{source.name} {source.GetInstanceID()} attacks {target.name} {target.GetInstanceID()} for {damage} damage.\nModifier: {_modifier}");
            damage *= _modifier;

            return Mathf.FloorToInt(damage);
        }

        public static int GetHealAmount(MingmingBattleLogic source, MingmingBattleLogic target, float power)
        {
            float healAmount = GetLevelModifier(source.Level) * power * source.Attack / 2;
            return Mathf.FloorToInt(Mathf.Clamp(healAmount, 0, target.TotalHealth - target.CurrentHealth));
        }

        private static float GetLevelModifier(int level) => (float)((2 * level) / 5) + 2;

        public int GetAttackDamage(int level, int attack, int defense, float cardPower,
            MingmingAlignment attackerType, MingmingAlignment defenderTypes, CardAlignment cardType)
        {
            float _modifier = GetModifier(attackerType, defenderTypes, cardType);
            //float damage = ((((((2 * level) / 5) + 2) * power * attack / defense) / 50) + 2) * _modifier;
            float damage = (float)((2 * level) / 5) + 2;
            damage *= (float)cardPower * attack / defense;
            damage = (float)(damage / 50) + 2;
            damage *= _modifier;

            return Mathf.FloorToInt(damage);
        }

        private float GetModifier(MingmingAlignment attackerType, MingmingAlignment defenderTypes, CardAlignment cardType)
        {
            float modifier = attackerType.Contains(cardType) ? SAME_TYPE_ADVANTAGE : 1f; //Bonus for using same type
            modifier *= GetTypeAdvantage(defenderTypes, cardType);
            return modifier;
        }

        private float GetTypeAdvantage(MingmingAlignment defenderTypes, CardAlignment cardType)
        {
            float modifier = 1f;

            bool hasPrimaryAdvantage = AlignmentAdvantageLookup.TryGetValue(new AlignmentCombination { Source = cardType, Target = defenderTypes.Primary }, out float primaryValue);
            modifier *= hasPrimaryAdvantage ? primaryValue : 1;

            bool hasSecondaryAdvantage = AlignmentAdvantageLookup.TryGetValue(new AlignmentCombination { Source = cardType, Target = defenderTypes.Secondary }, out float secondaryValue);
            modifier *= hasSecondaryAdvantage ? secondaryValue * SECONDARY_TYPE_ADVANTAGE : 1;

            return modifier;
        }

        public static int GetExp(MingmingInstance mingmingInstance)
        {
            return GetExpForLevel(mingmingInstance.Level);
        }

        public static int GetExpNextLevel(MingmingInstance mingmingInstance)
        {
            return GetExpForLevel(mingmingInstance.Level + 1);
        }

        private static int GetExpForLevel(int level)
        {
            int exp = Mathf.RoundToInt(0.8f * Mathf.Pow(level, 3));
            return exp;
        }

        public static int CalculateStat(int baseStat, int modifier, int level)
        {
            //floor(floor((2 * B + I + E) * L / 100 + 5) * N)
            return (int)Math.Floor((double)(((2 * baseStat) + modifier) * level / 100) + 5);
        }

        public static int GetRandomInt(int? min = null, int? max = null)
        {
            int randomValue;
            if (max.HasValue)
            {
                if (min.HasValue)
                {
                    randomValue = Random.Range(min.Value, max.Value);
                }
                else
                {
                    randomValue = Random.Range(0, max.Value);
                }
            }
            else
            {
                randomValue = Random.Range(0, int.MaxValue);
            }

            return randomValue;
        }

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        /// <returns>float range[0,1]</returns>
        public static float GetRandomFloat()
            => Random.value;
    }
}