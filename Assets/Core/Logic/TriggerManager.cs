using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NotTimeTravel.Core.Logic
{
    public class TriggerManager : MonoBehaviour
    {
        public UnityEvent onTrigger;
        [ColorUsage(true, true)] public Color triggeredColor;
        [ColorUsage(true, true)] public Color notTriggeredColor;
        public float transitionDuration;
        public float waitBeforeTransition;

        private SpriteRenderer[] _spriteRenderers;

        private void Start()
        {
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            SetColor(notTriggeredColor);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            onTrigger.Invoke();

            StopAllCoroutines();
            SetColor(triggeredColor);
            StartCoroutine(TransitionColor());
        }

        private IEnumerator TransitionColor()
        {
            yield return new WaitForSeconds(waitBeforeTransition);
            yield return Transition.Transition.TransitionColor(transitionDuration, triggeredColor, notTriggeredColor,
                SetColor);
        }

        private void SetColor(Color color)
        {
            foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
            {
                spriteRenderer.color = color;
            }
        }
    }
}