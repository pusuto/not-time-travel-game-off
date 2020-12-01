using System;
using UnityEngine;

namespace NotTimeTravel.Core.Audio
{
    public class AudioFadeManager : MonoBehaviour
    {
        public AudioSource caveAudio;
        public AudioSource natureAudio;

        private Bounds _bounds;

        private void Start()
        {
            _bounds = GetComponent<BoxCollider2D>().bounds;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!natureAudio.isPlaying)
            {
                natureAudio.Play();
            }

            float length = _bounds.max.y - _bounds.min.y;
            float currentY = other.gameObject.transform.position.y;
            float progress = Mathf.Clamp(currentY - _bounds.min.y, 0, float.PositiveInfinity);
            float increasingVolume = progress * 0.3f / length;
            float decreasingVolume = 0.5f - increasingVolume;

            natureAudio.volume = increasingVolume;
            caveAudio.volume = decreasingVolume;
        }
    }
}