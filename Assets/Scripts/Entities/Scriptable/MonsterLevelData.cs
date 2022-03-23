using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [Serializable]
    public class MonsterLevelData
    {
        public MonsterData MonsterData;

        [Range(1, 99)]
        public int Level;
    }
}
