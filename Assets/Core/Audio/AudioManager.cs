using System.Collections;
using UnityEngine;

namespace NotTimeTravel.Core.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public float soundFadeDuration;
        public float ambienceVolumeLevel;
        public AudioSource caveAmbience;

        public void FadeInCaveAmbience()
        {
            if (!caveAmbience.isPlaying)
            {
                caveAmbience.Play();
            }

            IEnumerator sequence = Transition.Transition.TransitionFloat(soundFadeDuration, ambienceVolumeLevel, 0,
                ambienceVolumeLevel,
                newVolume => { caveAmbience.volume = newVolume; });
            StartCoroutine(sequence);
        }
    }
}