using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI.Controller
{
    public class SceneTransitionController : MonoBehaviour
    {
        private const string FADE_OUT_TRIGGER = "Fade Out Trigger";
        private const string FADE_IN_TRIGGER = "Fade In Trigger";
        public const float ANIMATION_TIME = 1f;

        [SerializeField] private Animator Animator;

        private void Start()
        {
            Animator.enabled = true;
        }

        public void FadeToBlack()
        {
            Animator.SetTrigger(FADE_IN_TRIGGER);
        }

        public IEnumerator FadeToScene(float secondsPassed)
        {
            float waitDurationSeconds = Mathf.Clamp(ANIMATION_TIME - secondsPassed, 0.1f, ANIMATION_TIME);
            yield return new WaitForSeconds(waitDurationSeconds);

            Animator.SetTrigger(FADE_OUT_TRIGGER);
        }
    }
}