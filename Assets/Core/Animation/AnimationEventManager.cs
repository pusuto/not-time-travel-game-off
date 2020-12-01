using System.Linq;
using NotTimeTravel.Core.Audio;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Animation
{
    public class AnimationEventManager : MonoBehaviour
    {
        public Animator animator;
        public AnimationEventCollection[] soundCollectionManagers;

        public void OnEvent(AnimationEvent animationEvent)
        {
            SoundCollectionManager[] matchedSoundCollectionManagers = soundCollectionManagers
                .Where(collection => collection.MatchesAnimationEvent(animationEvent))
                .Select(collection => collection.soundCollectionManager).ToArray();

            foreach (SoundCollectionManager matchedSoundCollectionManager in matchedSoundCollectionManagers)
            {
                matchedSoundCollectionManager.PlayRandom();
            }
        }
    }
}