using System.Collections;
using Core.NPC;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Speech.Dialogue
{
    public class ValentineDialogue : MonoBehaviour, IDialogue
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
                yield return GetSavedDialogue(npc, main);
                yield break;
            }

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

        private IEnumerator GetFinalDialogue(SpeechBubbleManager npc)
        {
            yield return npc.Say("Thank you so much for saving me.", 3f);
            yield return npc.Say("I'll see you on the outside!", 3f);
        }

        private IEnumerator GetNotTimeTravelDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return main.Say("What do you think about this magical lantern?", 4f);
            yield return npc.Say("I imagine it could be very useful. Although...", 4f);
            yield return npc.Say("It's unfortunate it is not a time travel magical lantern.", 5f);
            yield return npc.Say("I heard those are good.", 3f);
            yield return main.Say("But this thing has time travel written all over it!", 4f);
            yield return npc.Say("You seem to be very convinced about this.", 4f);
            yield return npc.Say("I trust you will give your best to find out the truth.", 5f);
            yield return npc.Say("Do some research, make some experiments.", 4f);
            yield return npc.Say("In the end you will definitely reach the truth.", 4f);
            yield return new WaitForSeconds(3f);
            yield return npc.Say("(which tells us this is not time travel)", 4f);

            _speechStatus = 3;
        }

        private IEnumerator GetTip(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return npc.Say("Here is a little tip for you.", 3f);
            yield return npc.Say("Up above, near the exit, there is a door that is very hard to open.", 5f);
            yield return npc.Say("...Very hard for whoever does not have your lantern.", 4f);
            yield return npc.Say("Sometimes it seems the hard way is the way to solve a riddle.", 5f);
            yield return npc.Say("Always remember that your power can give you a much easier alternative.", 5f);
            yield return npc.Say("You just need to be creative!", 3f);
            yield return main.Say("Thank you for the tip, I'll keep it in mind.", 4f);
        }

        private IEnumerator GetSavedDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return npc.Say("Oh, thank you! You have saved me.", 4f);
            yield return npc.Say("I told you there would be a way. And you found it!", 4f);
            yield return GetTip(npc, main);

            _speechStatus = 2;
        }

        private IEnumerator GetSecondDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            if (GetComponentInParent<NpcManager>().IsSaved())
            {
                yield return GetSavedDialogue(npc, main);
                yield break;
            }

            yield return npc.Say("Remember that sometimes the right way is not in the place you expect.", 5f);
        }

        private IEnumerator GetFirstDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return npc.Say("Oh, hello!", 2f);
            yield return npc.Say("My name is <color=#FFF02D>Valentine</color>.", 4f);
            yield return npc.Say("I see you have a nice power that allows you to go as you please.", 5f);
            yield return npc.Say("Unfortunately, I am stuck here.", 4f);
            yield return main.Say("I see. That doesn't seem to bother you much, though?", 5f);
            yield return npc.Say("Oh, you're right.", 3f);
            yield return npc.Say("I know everyone else here thinks I cannot be saved.", 4f);
            yield return npc.Say("After all, there seems to be no way to bring a box here.", 5f);

            if (GetComponentInParent<NpcManager>().IsSaved())
            {
                yield return npc.Say("But you still found a way!", 3f);
                yield return npc.Say("Thank you so much.", 2f);
                yield return GetTip(npc, main);

                _speechStatus = 2;
            }
            else
            {
                yield return npc.Say("However, I know there must be a way.", 3f);
                yield return npc.Say("It was just not found yet. But...", 3f);
                yield return npc.Say("I trust it will be.", 3f);
                yield return npc.Say("I wouldn't be surprised if it will be you who saves me.", 4f);
                yield return npc.Say("Remember that sometimes the right way is not in the place you expect.", 5f);

                _speechStatus = 1;
            }
        }
    }
}