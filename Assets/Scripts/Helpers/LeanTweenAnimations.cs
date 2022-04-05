using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class LeanTweenAnimations
    {
        public static LTDescr RotateBackAndForth(GameObject gameObject, float rotationAmountDegrees, float durationSeconds)
        {
            LTDescr rotate(float to) => LeanTween.rotateZ(gameObject, to, durationSeconds);

            return rotate(rotationAmountDegrees)
                .setOnComplete(() => rotate(0)
                    .setOnComplete(() => rotate(-rotationAmountDegrees)
                        .setOnComplete(() => rotate(0))));
        }
    }
}
