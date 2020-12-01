using System.Collections;
using NotTimeTravel.Core.Message;
using NotTimeTravel.Core.Model;
using NotTimeTravel.Core.Speech;
using NotTimeTravel.Core.State;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace NotTimeTravel.Core.Logic
{
    public class ItemAction : MonoBehaviour, IGameAction
    {
        public bool isActive;
        public ItemActionType actionType;
        public string itemName;
        public string youFoundText;
        public GameObject item;
        public float disappearDuration;

        private InventoryManager _inventoryManager;

        private void Start()
        {
            _inventoryManager = GlobalInstanceManager.GetInventoryManager();
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void Invoke()
        {
            switch (actionType)
            {
                case ItemActionType.Take:
                    _inventoryManager.TakeItem(new InventoryItem()
                    {
                        Name = itemName
                    });
                    GlobalInstanceManager.GetMainCharacter().GetComponent<MessageManager>()
                        .ShowMessage(youFoundText);
                    StartCoroutine(FadeOutItem());
                    break;
                case ItemActionType.Drop:
                    _inventoryManager.DropItem(itemName);
                    break;
                default:
                    return;
            }
        }

        private IEnumerator FadeOutItem()
        {
            if (item == null)
            {
                yield break;
            }

            Light2D itemLight = item.GetComponentInChildren<Light2D>();

            if (itemLight != null)
            {
                yield return Transition.Transition.TransitionFloat(disappearDuration, 1, 1, 0,
                    newIntensity => itemLight.intensity = newIntensity);
            }

            Destroy(item);
        }
    }
}