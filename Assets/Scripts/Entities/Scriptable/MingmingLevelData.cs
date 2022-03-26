using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [Serializable]
    public class MingmingLevelData
    {
        public MingmingData MingMingData;

        [Range(1, 99)]
        public int Level;
    }
}
