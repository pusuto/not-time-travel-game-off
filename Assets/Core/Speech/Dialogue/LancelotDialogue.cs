using System.Collections;
using Core.NPC;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Speech.Dialogue
{
    public class LancelotDialogue : MonoBehaviour, IDialogue
    {
        private int _speechState;

        public IEnumerator GetDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            switch (_speechState)
            {
                case 0:
                    yield return FirstDialogue(npc, main);
                    break;
                case 1:
                    if (GetComponentInParent<NpcManager>().IsSaved())
                    {
                        yield return npc.Say("Thanks to you, I am now free! Can't wait to play the harp again.", 5f);
                        yield return SavedDialogue(npc, main);
                    }
                    else
                    {
                        yield return npc.Say("Hello again. It has been hard without my harp.", 4f, 0, 0, VoiceTone.Helpless);
                        yield return npc.Say("If you would save me, it would allow me to play again.", 6f);
                    }

                    break;
                case 2:
                    yield return FinalDialogue(npc, main);
                    yield break;
                case 3:
                    yield return GoodbyeDialogue(npc);
                    yield break;
            }
        }

        private IEnumerator FirstDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return npc.Say("Hello! You have found <color=#FFF02D>Lancelot</color>, the musician.", 5f);
            yield return main.Say("What instrument do you play?", 3f);
            yield return npc.Say("The harp.", 2f);
            yield return npc.Say("Unfortunately, I can't play while imprisoned.", 4f);

            _speechState = 1;
        }

        private IEnumerator SavedDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return npc.Say("Listen. Getting out of here won't be easy.", 4f);
            yield return npc.Say("Make the best of your magical lantern.", 3f);
            yield return main.Say("About that. Is it me or this is totally time travel?", 3f);
            yield return npc.Say("What?", 1f);
            yield return main.Say("You know, this lantern thing. Time travelling stuff.", 4f);
            yield return npc.Say("Pretty sure it has nothing to do with that.", 3f);
            yield return main.Say("Hm.", 1f);

            _speechState = 2;
        }

        private IEnumerator FinalDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return main.Say("Where were we on the \"Is it time travel?\" conversation?", 4f);
            yield return npc.Say("You said it was time travel. I answered that it isn't.", 6f);
            yield return main.Say("Hm.", 1f);
            yield return npc.Say("I wouldn't worry about it. I am thankful you saved me.", 5f);
            yield return npc.Say("Does it really matter if it was time travel or not?", 5f);
            yield return main.Say("Maybe you're right.", 3f);

            _speechState = 3;
        }

        private IEnumerator GoodbyeDialogue(SpeechBubbleManager npc)
        {
            yield return npc.Say("Good luck on your way out!", 3f);
        }
    }
}