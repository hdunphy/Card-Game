using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI.Controller
{
    public class SceneTransitionController : MonoBehaviour
    {
        private const string FADE_OUT_TRIGGER = "Fade Out Trigger";

        [SerializeField] private Animator Animator;
        
        // Use this for initialization
        void Start()
        {
            Animator.enabled = false;
        }

        public IEnumerator StartSceneTransition(Func<IEnumerator> func)
        {
            Animator.enabled = true;

            yield return new WaitForSeconds(1);

            yield return func.Invoke();

            Animator.SetTrigger(FADE_OUT_TRIGGER);
        }
    }
}