using NotTimeTravel.Core.Door;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Logic
{
    public class DoorCondition : MonoBehaviour, ICondition
    {
        public bool isActive;
        public string message = "Unlocked with a device";
        public DoorManager door;

        public bool IsActive()
        {
            return isActive;
        }

        public bool IsCleared()
        {
            return door.GetStatus() != DoorStatus.Locked;
        }

        public string GetMessage()
        {
            return message;
        }
    }
}