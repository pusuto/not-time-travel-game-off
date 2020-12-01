using System.Collections;
using UnityEngine;

namespace NotTimeTravel.Core.Player
{
    public class ProgressManager : MonoBehaviour
    {
        public GameObject materialOwner;
        public float accentFadeInDuration = .4f;
        
        private static readonly int RecordingElapsedPercentage = Shader.PropertyToID("RecordingElapsedPercentage");
        private Renderer _spriteRenderer;
        private static readonly int AccentPresencePercentage = Shader.PropertyToID("AccentPresencePercentage");

        private void Start()
        {
            _spriteRenderer = materialOwner.GetComponent<Renderer>();
        }

        public void UpdatePercentage(float percentage)
        {
            _spriteRenderer.material.SetFloat(RecordingElapsedPercentage, percentage);
        }

        public void FadeInAccent()
        {
            StartCoroutine(RunFadeInAccent());
        }

        public void ResetAccent()
        {
            _spriteRenderer.material.SetFloat(AccentPresencePercentage, 0f);
        }

        private IEnumerator RunFadeInAccent()
        {
            float elapsedTime = 0f;

            float currentPercentage = _spriteRenderer.material.GetFloat(AccentPresencePercentage);

            while (elapsedTime <= accentFadeInDuration)
            {
                float newPercentage = Mathf.Lerp(currentPercentage, 1, (elapsedTime / accentFadeInDuration));
                _spriteRenderer.material.SetFloat(AccentPresencePercentage, newPercentage);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}