using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class MonsterInstance
    {
        private readonly MonsterData BaseData;
        private readonly int AttackModifier;
        private readonly int DefenseModifier;
        private readonly int HealthModifier;

        public int Attack { get => CalculateStat(BaseData.Attack, AttackModifier); }
        public int Defense { get => CalculateStat(BaseData.Defense, DefenseModifier); }
        public int Health { get => CalculateStat(BaseData.Health, HealthModifier) + Level + 5; }
        public int Energy { get => BaseData.Energy; }
        public int Level { get; private set; }
        public int Experiance { get; private set; }
        public string Name { get; private set; }

        public UnityEngine.Sprite Sprite { get => BaseData.Sprite; }
        public MonsterAlignment MonsterAlignment { get => BaseData.MonsterAlignments; }
        public List<CardData> WildDeck { get; private set; }

        public MonsterInstance(MonsterData monsterData, int _level)
        {
            BaseData = monsterData;
            Level = _level;
            Name = monsterData.name;
            Experiance = Rules.Instance.GetExp(this);
            AttackModifier = Rules.GetRandomInt(0, 31);
            DefenseModifier = Rules.GetRandomInt(0, 31);
            HealthModifier = Rules.GetRandomInt(0, 31);

            WildDeck = BaseData.WildCards;
            if (BaseData.Level30Card != null && Level >= 30)
                WildDeck.Add(BaseData.Level30Card);
            if (BaseData.Level50Card != null && Level >= 50)
                WildDeck.Add(BaseData.Level50Card);
        }

        public int AddExperience(int _xp, int levelUps = 0)
        {
            int xpToNextLevel = Rules.Instance.GetExpNextLevel(this) - Rules.Instance.GetExp(this);
            
            if(_xp > xpToNextLevel)
            {
                Experiance += xpToNextLevel;
                Level++;
                levelUps = AddExperience(_xp - xpToNextLevel, ++levelUps);
            }
            else
                Experiance += _xp;

            return levelUps;
        }

        public float GetExperiencePercentage()
        {
            int currentExp = Experiance - Rules.Instance.GetExp(this);
            int nextLevel = Rules.Instance.GetExpNextLevel(this) - Rules.Instance.GetExp(this);
            return (float)currentExp / nextLevel;
        }

        private int CalculateStat(int baseStat, int modifier)
        {
            //floor(floor((2 * B + I + E) * L / 100 + 5) * N)
            return (int) Math.Floor((double)(((2 * baseStat) + modifier) * Level / 100) + 5);
        }
    }
}