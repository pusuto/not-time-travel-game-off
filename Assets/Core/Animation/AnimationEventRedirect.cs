using UnityEngine;
using UnityEngine.Events;

namespace NotTimeTravel.Core.Animation
{
    public class AnimationEventRedirect : MonoBehaviour
    {
        public UnityEvent<AnimationEvent> onInvoke;

        public void Invoke(AnimationEvent animationEvent)
        {
            onInvoke.Invoke(animationEvent);
        }
    }
}