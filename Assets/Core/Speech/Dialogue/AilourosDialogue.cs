using System.Collections;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Speech.Dialogue
{
    public class AilourosDialogue : MonoBehaviour, IDialogue
    {
        private int _speechStatus;

        public IEnumerator GetDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            switch (_speechStatus)
            {
                case 0:
                    yield return GetFirstDialogue(npc, main);
                    break;
                case 1:
                    yield return GetSecondDialogue(npc, main);
                    break;
            }
        }

        private IEnumerator GetSecondDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return npc.Say("What?", 2f, 0, 0, VoiceTone.Question);
            yield return main.Say("No more hints?", 2f, 0, 0, VoiceTone.Helpless);
            yield return npc.Say("Maybe later.", 2f);
        }

        private IEnumerator GetFirstDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return npc.Say("Well, well, well. You found a hidden passage, that you did.", 6f);
            yield return npc.Say("And in it, you found old <color=#FFF02D>Ailouros</color>.", 4f);
            yield return main.Say("Can we talk about this room appearing out of nowhere for a minute?", 6f);
            yield return npc.Say("No.", 2f);
            yield return npc.Say("However...", 1f);
            yield return main.Say("*cough*", 1.5f, 2f);
            yield return npc.Say("Oh-oh, sorry. Dozed off for a minute there.", 3f);
            yield return npc.Say("You found this little here hidden passage, yes, sir.", 5f);
            yield return npc.Say("But there are more, oh yes. I suggest looking for them in every corner.", 6f);
            yield return npc.Say("Sometimes you have to be quick to find them, yes you will.", 5f);
            yield return main.Say("Will do, thanks", 3f);

            _speechStatus = 1;
        }
    }
}