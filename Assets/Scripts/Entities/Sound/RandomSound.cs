using Assets.Scripts.References;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Sound
{
    public class RandomSound : Sound
    {
        [SerializeField] private List<AudioClip> clips;

        public override AudioClip Clip => clips[Rules.GetRandomInt(clips.Count)];
    }
}
