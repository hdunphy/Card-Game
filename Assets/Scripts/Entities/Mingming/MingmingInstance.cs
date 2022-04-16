using Assets.Scripts.Controller.References;
using Assets.Scripts.Entities.Drops;
using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.References;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities
{
    public class MingmingInstance : IEquatable<MingmingInstance>
    {
        public readonly int AttackModifier;
        public readonly int DefenseModifier;
        public readonly int HealthModifier;

        public MingmingData BaseData { get; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Health { get; private set; }
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

            SetFieldsFromData();

            CurrentHealth = mingmingInstance.CurrentHealth;
        }

        public MingmingInstance(MingmingData mingmingData, int level)
        {
            BaseData = mingmingData;
            Level = level;

            Experience = Rules.GetExp(this);
            AttackModifier = Rules.GetRandomInt(0, 31);
            DefenseModifier = Rules.GetRandomInt(0, 31);
            HealthModifier = Rules.GetRandomInt(0, 31);

            SetFieldsFromData();
            
            CurrentHealth = Health;
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

            Attack = Rules.CalculateStat(BaseData.Attack, AttackModifier, Level);
            Defense = Rules.CalculateStat(BaseData.Defense, DefenseModifier, Level);
            Health = Rules.CalculateStat(BaseData.Health, HealthModifier, Level) + Level + 5;

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

        #region Equals Overrides
        public override bool Equals(object obj)
        {
            return Equals(obj as MingmingInstance);
        }

        public bool Equals(MingmingInstance other)
        {
            return other != null &&
                   AttackModifier == other.AttackModifier &&
                   DefenseModifier == other.DefenseModifier &&
                   HealthModifier == other.HealthModifier &&
                   EqualityComparer<MingmingData>.Default.Equals(BaseData, other.BaseData) &&
                   CurrentHealth == other.CurrentHealth &&
                   Energy == other.Energy &&
                   Level == other.Level &&
                   Experience == other.Experience &&
                   Name == other.Name &&
                   CardDraw == other.CardDraw &&
                   DataName == other.DataName;
        }

        public override int GetHashCode()
        {
            int hashCode = -1499152676;
            hashCode = hashCode * -1521134295 + AttackModifier.GetHashCode();
            hashCode = hashCode * -1521134295 + DefenseModifier.GetHashCode();
            hashCode = hashCode * -1521134295 + HealthModifier.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<MingmingData>.Default.GetHashCode(BaseData);
            hashCode = hashCode * -1521134295 + CurrentHealth.GetHashCode();
            hashCode = hashCode * -1521134295 + Energy.GetHashCode();
            hashCode = hashCode * -1521134295 + Level.GetHashCode();
            hashCode = hashCode * -1521134295 + Experience.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + CardDraw.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DataName);
            return hashCode;
        }
        #endregion
    }
}