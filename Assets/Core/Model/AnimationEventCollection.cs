using System;
using NotTimeTravel.Core.Audio;
using UnityEngine;

namespace NotTimeTravel.Core.Model
{
    [Serializable]
    public class AnimationEventCollection
    {
        public string name;
        public SoundCollectionManager soundCollectionManager;

        public bool MatchesAnimationEvent(AnimationEvent animationEvent)
        {
            return name == animationEvent.stringParameter;
        }
    }
}