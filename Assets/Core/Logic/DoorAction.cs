using NotTimeTravel.Core.Door;
using NotTimeTravel.Core.Model;
using UnityEngine;
using NotTimeTravel.Core.Interaction;

namespace NotTimeTravel.Core.Logic
{
    public class DoorAction : MonoBehaviour, IGameAction
    {
        public bool isActive;
        public bool useInteraction;
        public DoorStatus status;
        public DoorManager door;

        public bool IsActive()
        {
            return isActive;
        }

        public void Invoke()
        {
            door.SetStatus(status);

            if (useInteraction)
            {
                door.GetComponentInChildren<InteractionManager>().InteractNow();
            }
        }
    }
}