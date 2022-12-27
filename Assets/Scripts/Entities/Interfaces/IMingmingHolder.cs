using Assets.Scripts.Entities.Mingmings;
using System.Collections.Generic;

namespace Assets.Scripts.Entities.Interfaces
{
    public interface IMingmingHolder
    {
        public List<MingmingInstance> AllMingmings { get; }
        public List<MingmingInstance> Party { get; }
    }
}