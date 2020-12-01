using UnityEngine;

namespace NotTimeTravel.Core.Logic
{
    public class SetActiveAction : MonoBehaviour, IGameAction
    {
        public bool isActive;
        public bool setActive;
        public GameObject target;

        public bool IsActive()
        {
            return isActive;
        }

        public void Invoke()
        {
            target.SetActive(setActive);
        }
    }
}