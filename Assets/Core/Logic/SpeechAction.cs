using NotTimeTravel.Core.Speech;
using UnityEngine;

namespace NotTimeTravel.Core.Logic
{
    public class SpeechAction : MonoBehaviour, IGameAction
    {
        public bool isActive;
        public string characterName;
        public string line;
        public float duration;

        public bool IsActive()
        {
            return isActive;
        }

        public void Invoke()
        {
            GlobalSpeechManager.CharacterSayNow(characterName, line, duration);
        }
    }
}