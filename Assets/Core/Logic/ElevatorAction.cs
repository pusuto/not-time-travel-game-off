using NotTimeTravel.Core.Elevator;
using UnityEngine;
using NotTimeTravel.Core.Interaction;

namespace NotTimeTravel.Core.Logic
{
    public class ElevatorAction : MonoBehaviour, IGameAction
    {
        public bool isActive;
        public bool useInteraction;
        public ElevatorManager elevator;

        public bool IsActive()
        {
            return isActive;
        }

        public void Invoke()
        {
            elevator.Activate();

            if (useInteraction)
            {
                elevator.GetComponentInChildren<InteractionManager>().InteractNow();
            }
        }
    }
}