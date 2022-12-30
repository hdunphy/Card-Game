using Assets.Scripts.Entities.Interfaces;
using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.Entities.Scriptable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities.Player
{
    public class PlayerMingmingHolder : IMingmingHolder
    {
        public List<MingmingInstance> AllMingmings { get; }

        public List<MingmingInstance> Party { get; }

        public PlayerMingmingHolder(List<MingmingLevelData> mingmings)
        {
            AllMingmings = mingmings.Select(d => new MingmingInstance(d.MingMingData, d.Level)).ToList();
            Party = new(AllMingmings.Take(3));
        }

        public PlayerMingmingHolder(MingmingHolderSaveModel saveModel)   
        {
            AllMingmings = saveModel.AllMingmings.Select(m => new MingmingInstance(m)).ToList();
            Party = saveModel.PartyIndecies.Select(i => AllMingmings[i]).ToList();
        }
    }

    [Serializable]
    public class MingmingHolderSaveModel
    {
        public List<MingmingSaveModel> AllMingmings { get; }
        public List<int> PartyIndecies { get; }

        public MingmingHolderSaveModel() { AllMingmings = new(); PartyIndecies = new(); }

        public MingmingHolderSaveModel(PlayerMingmingHolder holder)
        {
            AllMingmings = holder.AllMingmings.Select(m => new MingmingSaveModel(m)).ToList();
            PartyIndecies = holder.Party.Select(m => holder.AllMingmings.IndexOf(m)).ToList();
        }
    }
}