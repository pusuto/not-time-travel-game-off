using System.Collections;
using Core.NPC;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Speech.Dialogue
{
    public class PhilippaDialogue : MonoBehaviour, IDialogue
    {
        private int _speechStatus;

        public IEnumerator GetDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            if (_speechStatus == 2)
            {
                yield return GetNotTimeTravelDialogue(npc, main);
            }

            if (_speechStatus == 3)
            {
                yield return GetFinalDialogue(npc);
            }

            if (_speechStatus == 1 && GetComponentInParent<NpcManager>().IsSaved())
            {
                yield return GetSavedDialogue(npc);
                yield break;
            }

            switch (_speechStatus)
            {
                case 0:
                    yield return GetFirstDialogue(npc, main);
                    break;
                case 1:
                    yield return GetSecondDialogue(npc);
                    break;
            }
        }

        private IEnumerator GetFinalDialogue(SpeechBubbleManager npc)
        {
            yield return npc.Say("Still, thank you for saving me.", 3f);
            yield return npc.Say("The one thing that's ok with you.", 3f);
        }

        private IEnumerator GetNotTimeTravelDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return npc.Say("You look like someone who thinks that's a time traveler lantern.", 5f);
            yield return main.Say("I stand by it!", 2f);
            yield return npc.Say("It obviously has nothing to do with time travel!", 4f);
            yield return npc.Say("Really, I am tempted to just un-save myself.", 4f);
            yield return main.Say("I can just go back in time and stop me from saving you.", 4f);
            yield return main.Say("...You know, using this magical time travel lantern.", 4f);
            yield return npc.Say("*scoff*", 1f, 0, 0, VoiceTone.Angry);

            _speechStatus = 3;
        }

        private IEnumerator GetSavedDialogue(SpeechBubbleManager npc)
        {
            yield return npc.Say("Color me surprised!", 2f);
            yield return npc.Say("You actually did save me.", 3f);
            yield return npc.Say("You should be proud of yourself.", 3f);
            yield return npc.Say("...But not too much.", 3f);

            _speechStatus = 2;
        }

        private IEnumerator GetSecondDialogue(SpeechBubbleManager npc)
        {
            yield return npc.Say("Don't waste my time.", 2f, 0, 0, VoiceTone.Angry);
        }

        private IEnumerator GetFirstDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return npc.Say("Well, well, well.", 3f);
            yield return main.Say("What?", 2f);
            yield return npc.Say("What what?", 2f);
            yield return npc.Say("You did enter my cell, so you must want something.", 4f);

            if (GetComponentInParent<NpcManager>().IsSaved())
            {
                yield return main.Say("I just saved you.", 2f);
                yield return GetSavedDialogue(npc);

                _speechStatus = 2;
            }
            else
            {
                yield return main.Say("Hm. Do you not want me to save you?", 4f);
                yield return npc.Say("Sure.", 2f);
                yield return npc.Say("The question is... can you?", 4f);
                yield return main.Say("I'll se what I can do.", 3f);
                
                _speechStatus = 1;
            }
        }
    }
}