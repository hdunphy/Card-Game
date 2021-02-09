using System;

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
        public UnityEngine.Sprite Sprite { get => BaseData.Sprite; }
        public MonsterAlignment MonsterAlignment { get => BaseData.MonsterAlignments; }

        public int Level { get; private set; }

        public int Experiance { get; private set; }

        public MonsterInstance(MonsterData monsterData, int _level)
        {
            BaseData = monsterData;
            Level = _level;
            Experiance = Rules.Instance.GetExp(this);
            AttackModifier = Rules.GetRandomInt(0, 31);
            DefenseModifier = Rules.GetRandomInt(0, 31);
            HealthModifier = Rules.GetRandomInt(0, 31);
        }

        public bool AddExperience(int _xp)
        {
            bool levelup = false;

            int nextLevelExp = Rules.Instance.GetExpNextLevel(this);
            Experiance += _xp;

            if(Experiance > nextLevelExp)
            {
                Level++;
                levelup = true;
            }

            return levelup;
        }

        private int CalculateStat(int baseStat, int modifier)
        {
            //floor(floor((2 * B + I + E) * L / 100 + 5) * N)
            return (int) Math.Floor((double)(((2 * baseStat) + modifier) * Level / 100) + 5);
        }
    }
}