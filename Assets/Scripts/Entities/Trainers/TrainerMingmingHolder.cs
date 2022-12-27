using Assets.Scripts.Entities.Interfaces;
using Assets.Scripts.Entities.Mingmings;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities.Trainers
{
    public class TrainerMingmingHolder : IMingmingHolder
    {
        public List<MingmingInstance> Party { get; }

        public TrainerMingmingHolder(IEnumerable<MingmingInstance> party)
        {
            Party = party.ToList();
        }
    }
}