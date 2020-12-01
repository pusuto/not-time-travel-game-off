using System.Collections;
using NotTimeTravel.Core.Interaction;
using NotTimeTravel.Core.Model;
using NotTimeTravel.Core.Player;
using UnityEngine;

namespace NotTimeTravel.Core.Speech
{
    public class DialogueManager : MonoBehaviour
    {
        public string characterName;
        public Color popupColor;
        public Color textColor;
        public InteractionManager interactionManager;
        public SpeechAlign npcSpeechAlign;
        public SpeechAlign mainSpeechAlign;

        private SpeechBubbleManager _speechBubbleManager;

        public void Start()
        {
            _speechBubbleManager = GetComponentInChildren<SpeechBubbleManager>();
            _speechBubbleManager.popupColor = popupColor;
            _speechBubbleManager.textColor = textColor;
        }

        public void StartDialogue()
        {
            GlobalInstanceManager.GetMainCharacter().GetComponent<PlayerMovement>().StopAllMovement();
            StartCoroutine(DialogueSequence());
        }

        private IEnumerator DialogueSequence()
        {
            GlobalInstanceManager.GetStateManager().CanMove = false;
            SpeechBubbleManager main = GlobalSpeechManager.GetSpeechBubbleManager("Main");
            SpeechAlign oldNpcSpeechAlign = _speechBubbleManager.speechAlign;
            SpeechAlign oldMainSpeechAlign = main.speechAlign;
            _speechBubbleManager.speechAlign = npcSpeechAlign;
            main.speechAlign = mainSpeechAlign;

            GlobalInstanceManager.GetCameraManager().FocusOnCharacter(characterName);
            yield return GetComponent<IDialogue>()
                .GetDialogue(_speechBubbleManager, GlobalSpeechManager.GetSpeechBubbleManager("Main"));
            GlobalInstanceManager.GetCameraManager().FocusOnCharacter();

            GlobalInstanceManager.GetStateManager().CanMove = true;
            interactionManager.Refresh();
            _speechBubbleManager.speechAlign = oldNpcSpeechAlign;
            main.speechAlign = oldMainSpeechAlign;
            gameObject.transform.parent.GetComponentInChildren<InteractionManager>().SetCanvasActiveStatus(true);
        }
    }
}