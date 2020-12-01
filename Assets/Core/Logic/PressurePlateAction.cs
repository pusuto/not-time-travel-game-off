using UnityEngine;
using NotTimeTravel.Core.PressurePlate;

namespace NotTimeTravel.Core.Logic
{
    public class PressurePlateAction : MonoBehaviour, IGameAction
    {
        public bool isActive;
        public PressurePlateManager pressurePlate;

        public bool IsActive()
        {
            return isActive;
        }

        public void Invoke()
        {
            pressurePlate.SetOperational(true);
        }
    }
}