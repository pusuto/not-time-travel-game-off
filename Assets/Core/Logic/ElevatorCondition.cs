using NotTimeTravel.Core.Elevator;
using NotTimeTravel.Core.Door;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Logic
{
    public class ElevatorCondition : MonoBehaviour, ICondition
    {
        public bool isActive;
        public string message = "Elevator moving";
        public ElevatorManager elevator;

        public bool IsActive()
        {
            return isActive;
        }

        public bool IsCleared()
        {
            DoorStatus status = elevator.GetComponent<DoorManager>().GetStatus();
            
            return status != DoorStatus.Closing && status != DoorStatus.Opening;
        }

        public string GetMessage()
        {
            return message;
        }
    }
}