﻿using Assets.Scripts.Controller.References;
using Assets.Scripts.Entities.Drops;
using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.References;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities
{
    public class MingmingInstance
    {
        public readonly int AttackModifier;
        public readonly int DefenseModifier;
        public readonly int HealthModifier;

        public MingmingData BaseData { get; }
        public int Attack { get => Rules.CalculateStat(BaseData.Attack, AttackModifier, Level); }
        public int Defense { get => Rules.CalculateStat(BaseData.Defense, DefenseModifier, Level); }
        public int Health { get => Rules.CalculateStat(BaseData.Health, HealthModifier, Level) + Level + 5; }
        public int CurrentHealth { get; set; }
        public int Energy { get; private set; }
        public int Level { get; private set; }
        public int Experience { get; private set; }
        public string Name { get; set; }
        public int CardDraw { get; private set; }

        public string DataName => BaseData.name;

        public UnityEngine.Sprite Sprite { get => BaseData.Sprite; }
        public MingmingAlignment MingmingAlignment { get => BaseData.MingmingAlignment; }
        public List<CardData> WildDeck { get; private set; }

        public MingmingInstance(MingmingInstance mingmingInstance)
        {
            BaseData = mingmingInstance.BaseData;
            Level = mingmingInstance.Level;

            Experience = mingmingInstance.Experience;
            AttackModifier = mingmingInstance.AttackModifier;
            DefenseModifier = mingmingInstance.DefenseModifier;
            HealthModifier = mingmingInstance.HealthModifier;

            CurrentHealth = mingmingInstance.CurrentHealth;

            SetFieldsFromData();
        }

        public MingmingInstance(MingmingData mingmingData, int level)
        {
            BaseData = mingmingData;
            Level = level;

            Experience = Rules.GetExp(this);
            AttackModifier = Rules.GetRandomInt(0, 31);
            DefenseModifier = Rules.GetRandomInt(0, 31);
            HealthModifier = Rules.GetRandomInt(0, 31);
            
            CurrentHealth = Health;

            SetFieldsFromData();
        }

        public MingmingInstance(MingmingSaveModel mingmingSaveModel)
        {
            BaseData = ScriptableObjectReferenceSingleton.Singleton.GetScriptableObject<MingmingData>(mingmingSaveModel.MingmingDataName);
            Level = mingmingSaveModel.Level;
            Experience = mingmingSaveModel.Experience;
            AttackModifier = mingmingSaveModel.AttackModifier;
            CurrentHealth = mingmingSaveModel.CurrentHealth;
            DefenseModifier = mingmingSaveModel.DefenseModifier;

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
            int xpToNextLevel = Rules.GetExpNextLevel(this) - Experience;

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
            int currentExp = Experience - Rules.GetExp(this);
            int nextLevel = Rules.GetExpNextLevel(this) - Rules.GetExp(this);
            return (float)currentExp / nextLevel;
        }

        public int GetDeathExp() => Rules.GetExpNextLevel(this) / 5;

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