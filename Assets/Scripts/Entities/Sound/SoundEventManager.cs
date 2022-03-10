using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Sound
{
    public class SoundEventManager : ScriptableObject
    {
        [SerializeField] private List<Sound> Sounds;
        bool isAttached;

        private void OnEnable()
        {
            isAttached = false;
        }

        private void OnDisable()
        {
            RemoveFromGameObject();
        }

        public void AddToGameObject(GameObject gameObject)
        {
            Sounds.ForEach((s) =>
            {
                s.AudioSource = gameObject.AddComponent<AudioSource>();
                s.AudioSource.volume = s.Volume;
                s.AudioSource.pitch = s.Pitch;
                s.AudioSource.loop = s.Loop;
                s.AudioSource.clip = s.Clip;
                s.AudioSource.outputAudioMixerGroup = s.AudioMixerGroup; //Not sure if this is accurate
            });
            isAttached = true;
        }

        public void RemoveFromGameObject()
        {
            Sounds.ForEach((s) => s.AudioSource = null);
            isAttached = false;
        }

        public void PlayStart()
        {
            if (!isAttached)
                return;

            Sounds.ForEach((s) =>
            {
                if (s.PlayOnStart)
                    s.AudioSource.Play();
            });
        }

        public void PlaySound(string name)
        {
            if (!isAttached)
                return;

            Sound _sound = Sounds.FirstOrDefault(s => s.name == name);

            if (_sound != null)
            {
                _sound.AudioSource.Play();
            }
        }
    }
}
