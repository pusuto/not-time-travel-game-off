using NotTimeTravel.Core.Speech;
using UnityEngine;

namespace NotTimeTravel.Core.Logic
{
    public class ItemCondition : MonoBehaviour, ICondition
    {
        public bool isActive;
        public string message;
        public string itemName;

        public bool IsActive()
        {
            return isActive;
        }

        public bool IsCleared()
        {
            return GlobalInstanceManager.GetInventoryManager().HasItem(itemName);
        }

        public string GetMessage()
        {
            return message;
        }
    }
}