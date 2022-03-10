using UnityEngine.Audio;
using UnityEngine;

namespace Assets.Scripts.Entities.Sound
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        [SerializeField] private AudioClip clip;

        public virtual AudioClip Clip => clip;

        [Range(0f, 1f)]
        public float Volume;

        [Range(0.1f, 3f)]
        public float Pitch;

        public bool Loop;

        public bool PlayOnStart;

        public AudioMixerGroup AudioMixerGroup;

        [HideInInspector]
        public AudioSource AudioSource;
    }

}
