using System.Reflection;
using NotTimeTravel.Core.Speech;
using NotTimeTravel.Core.State;
using UnityEngine;

namespace NotTimeTravel.Core.Logic
{
    public class StateCondition : MonoBehaviour, ICondition
    {
        public bool isActive;
        public string stateName;
        public bool expectedValue;

        public bool IsActive()
        {
            return isActive;
        }

        public bool IsCleared()
        {
            StateManager stateManager = GlobalInstanceManager.GetStateManager();
            PropertyInfo propertyInfo = stateManager.GetType().GetProperty(stateName);
            return !(propertyInfo is null) && (bool) propertyInfo.GetValue(stateManager) == expectedValue;
        }

        public string GetMessage()
        {
            return $"{stateName} does not equal {expectedValue}";
        }
    }
}