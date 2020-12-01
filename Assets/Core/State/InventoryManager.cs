using System.Collections.Generic;
using NotTimeTravel.Core.Model;
using UnityEngine;
using UnityEngine.Events;

namespace NotTimeTravel.Core.State
{
    public class InventoryManager : MonoBehaviour
    {
        public UnityEvent<InventoryItem> onItemGet;

        private readonly Dictionary<string, InventoryItem> _items = new Dictionary<string, InventoryItem>();

        public bool HasItem(string itemName)
        {
            return _items.ContainsKey(itemName);
        }

        public InventoryItem GetItem(string itemName)
        {
            return HasItem(itemName) ? _items[itemName] : null;
        }

        public void TakeItem(InventoryItem item)
        {
            onItemGet.Invoke(item);
            _items[item.Name] = item;
        }

        public void DropItem(string itemName)
        {
            _items.Remove(itemName);
        }
    }
}