using NotTimeTravel.Core.Message;
using NotTimeTravel.Core.Speech;
using UnityEngine;

namespace Core.NPC
{
    public class NpcManager : MonoBehaviour
    {
        public string npcName;
        public Color backgroundColor;
        public Color foregroundColor;
        public bool needToBeSaved;

        private bool _isSaved;

        private void Awake()
        {
            DialogueManager dialogueManager = GetComponentInChildren<DialogueManager>();
            dialogueManager.characterName = npcName;
            dialogueManager.popupColor = backgroundColor;
            dialogueManager.textColor = foregroundColor;
            MessageManager messageManager = GetComponentInChildren<MessageManager>();
            messageManager.popupColor = backgroundColor;
            messageManager.textColor = foregroundColor;
            messageManager.ShowMessage(npcName, true);
        }

        public void Save()
        {
            _isSaved = true;
        }

        public bool IsSaved()
        {
            return _isSaved;
        }
    }
}