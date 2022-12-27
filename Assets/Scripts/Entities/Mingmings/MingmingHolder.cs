using Assets.Scripts.Entities.Interfaces;
using Assets.Scripts.Entities.Scriptable;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities.Mingmings
{
    public class MingmingHolder : IMingmingHolder
    {
        public List<MingmingInstance> AllMingmings { get; }

        public List<MingmingInstance> Party { get; }

        public MingmingHolder(List<MingmingLevelData> mingmings) {
            AllMingmings = mingmings.Select(d => new MingmingInstance(d.MingMingData, d.Level)).ToList();
            Party = new (AllMingmings);
        }
    }
}