using UnityEngine;

namespace NotTimeTravel.Core.Audio
{
    public class SoundCollectionManager : MonoBehaviour
    {
        public AudioSource[] sources;
        public float throttle;
        public float chance;

        private float _lastPlayed;

        public void PlayRandom()
        {
            if (_lastPlayed != 0 && Time.time - _lastPlayed <= throttle || Random.Range(0f, 1f) > chance)
            {
                return;
            }

            _lastPlayed = Time.time;
            AudioSource randomSource = sources[Random.Range(0, sources.Length)];

            randomSource.PlayOneShot(randomSource.clip);
        }
    }
}