using UnityEngine;
using UnityEngine.Events;

namespace NotTimeTravel.Core.Logic
{
    public class InvokeAction : MonoBehaviour, IGameAction
    {
        public bool isActive;
        public UnityEvent onInvoke;

        public bool IsActive()
        {
            return isActive;
        }

        public void Invoke()
        {
            onInvoke.Invoke();
        }
    }
}