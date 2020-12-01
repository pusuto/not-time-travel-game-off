using System;
using System.Collections;
using UnityEngine;

namespace NotTimeTravel.Core.Transition
{
    public static class Transition
    {
        public static IEnumerator TransitionFloat(float duration, float range, float from, float target,
            Action<float> onSet, bool fixedTime = false)
        {
            float actualChange = Mathf.Abs(from - target);
            float actualDuration = actualChange * duration / range;
            float elapsedTime = 0;

            while (elapsedTime <= actualDuration)
            {
                float newValue = Mathf.Lerp(from, target, elapsedTime / actualDuration);
                onSet(newValue);
                elapsedTime += fixedTime ? Time.fixedDeltaTime : Time.deltaTime;
                yield return null;
            }

            onSet(target);
        }

        public static IEnumerator TransitionFloat(float duration, Func<float> getter, float target, Action<float> setter)
        {
            float from = getter();
            yield return TransitionFloat(duration, Math.Abs(from - target), from, target, setter);
        }

        public static IEnumerator TransitionColor(float duration, Color from, Color target, Action<Color> onSet,
            bool fixedTime = false)
        {
            float elapsedTime = 0;

            while (elapsedTime <= duration)
            {
                Color newColor = Color.Lerp(from, target, elapsedTime / duration);
                onSet(newColor);
                elapsedTime += fixedTime ? Time.fixedDeltaTime : Time.deltaTime;
                yield return null;
            }
        }
    }
}