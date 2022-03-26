using Assets.Scripts.Controller.References;
using Assets.Scripts.Entities.Drops;
using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.References;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities
{
    public class MonsterInstance
    {
        public readonly int AttackModifier;
        public readonly int DefenseModifier;
        public readonly int HealthModifier;

        public MonsterData BaseData { get; }
        public int Attack { get => CalculateStat(BaseData.Attack, AttackModifier); }
        public int Defense { get => CalculateStat(BaseData.Defense, DefenseModifier); }
        public int Health { get => CalculateStat(BaseData.Health, HealthModifier) + Level + 5; }
        public int CurrentHealth { get; set; }
        public int Energy { get; private set; }
        public int Level { get; private set; }
        public int Experience { get; private set; }
        public string Name { get; set; }
        public int CardDraw { get; private set; }

        public string DataName => BaseData.name;

        public UnityEngine.Sprite Sprite { get => BaseData.Sprite; }
        public MonsterAlignment MonsterAlignment { get => BaseData.MonsterAlignments; }
        public List<CardData> WildDeck { get; private set; }

        public MonsterInstance(MonsterData monsterData, int _level)
        {
            BaseData = monsterData;
            Level = _level;

            Experience = Rules.Instance.GetExp(this);
            AttackModifier = Rules.GetRandomInt(0, 31);
            DefenseModifier = Rules.GetRandomInt(0, 31);
            HealthModifier = Rules.GetRandomInt(0, 31);
            
            CurrentHealth = Health;

            SetFieldsFromData();
        }

        public MonsterInstance(MonsterSaveModel monsterSaveModel)
        {
            BaseData = ScriptableObjectReferenceSingleton.Singleton.GetScriptableObject<MonsterData>(monsterSaveModel.MonsterDataName);
            Level = monsterSaveModel.Level;
            Experience = monsterSaveModel.Experience;
            AttackModifier = monsterSaveModel.AttackModifier;
            CurrentHealth = monsterSaveModel.CurrentHealth;
            DefenseModifier = monsterSaveModel.DefenseModifier;

            SetFieldsFromData();
        }

        private void SetFieldsFromData()
        {
            Name = BaseData.name;
            Energy = BaseData.Energy;
            CardDraw = BaseData.CardDraw;
            WildDeck = BaseData.WildCards;
            if (BaseData.Level30Card != null && Level >= 30)
                WildDeck.Add(BaseData.Level30Card);
            if (BaseData.Level50Card != null && Level >= 50)
                WildDeck.Add(BaseData.Level50Card);
        }

        public int AddExperience(int _xp, int levelUps = 0)
        {
            int xpToNextLevel = Rules.Instance.GetExpNextLevel(this) - Experience;

            if (_xp > xpToNextLevel)
            {
                Experience += xpToNextLevel;
                Level++;
                levelUps = AddExperience(_xp - xpToNextLevel, ++levelUps);
            }
            else
                Experience += _xp;

            return levelUps;
        }

        public float GetExperiencePercentage()
        {
            int currentExp = Experience - Rules.Instance.GetExp(this);
            int nextLevel = Rules.Instance.GetExpNextLevel(this) - Rules.Instance.GetExp(this);
            return (float)currentExp / nextLevel;
        }

        private int CalculateStat(int baseStat, int modifier)
        {
            //floor(floor((2 * B + I + E) * L / 100 + 5) * N)
            return (int)Math.Floor((double)(((2 * baseStat) + modifier) * Level / 100) + 5);
        }

        public int GetDeathExp() => Rules.Instance.GetExpNextLevel(this) / 5;

        public CardData GetCardDrop()
        {
            List<DropChance> wildCardDrops = new List<DropChance>();
            int total = Enumerable.Range(0, WildDeck.Count).Sum();
            for (int i = 0; i < WildDeck.Count; i++)
            {
                wildCardDrops.Add(new DropChance
                {
                    DropObject = WildDeck[i],
                    IsEmpty = false,
                    RollChance = (float)i / total
                });
            }

            return (CardData)new DropTable(wildCardDrops).GetDrop();
        }
    }
}