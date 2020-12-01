using System.Collections;
using Core.NPC;
using NotTimeTravel.Core.Message;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Speech.Dialogue
{
    public class ElbertDialogue : MonoBehaviour, IDialogue
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
                        yield return SavedDialogue(npc);
                    }
                    else
                    {
                        yield return SecondDialogue(npc);
                    }

                    break;
            }

            yield return null;
        }

        private IEnumerator FirstDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            void OnDone()
            {
                _speechState = 1;
                GlobalInstanceManager.GetStateManager().CanUsePower = true;
            }

            if (GlobalInstanceManager.GetDebugStateManager().elbertSkipFirstDialogue)
            {
                yield return npc.Say("Blah blah blah", 1f);

                OnDone();
                yield break;
            }

            yield return npc.Say("Oh, hi. Was it you that opened the door?", 5f);
            yield return main.Say("Yes", 3f);
            yield return npc.Say("Well, we're both trapped now", 3f);
            yield return main.Say("...", 1f, 1f);
            yield return npc.Say("Unless you happened to find a magical lantern on your way here?", 5f);
            yield return main.Say("I do have a lantern with me. Is it really magical?", 4f);
            yield return npc.Say("A message should have appeared telling you so", 4f);
            yield return main.Say("You mean, when I took it?", 2f);
            yield return npc.Say("Yup.", 2f);
            npc.SayNow("It did not appear, right?", 3f);
            npc.SayNow("...Great.", 4f, 2.5f, 0, VoiceTone.Angry);
            yield return main.Say("Nope", 4.5f, 1f);
            yield return new WaitForSeconds(2f);
            yield return npc.Say("*whistles*", 2f);
            yield return new WaitForSeconds(3f);
            GlobalInstanceManager.GetMainCharacter().GetComponent<MessageManager>()
                .ShowMessage(
                    "You found a <color=#FFF02D>magical lantern</color>!");
            yield return npc.Say("This game is already <b>falling apart</b>", 3f, 3f);
            yield return npc.Say("Make sure you read the instructions carefully.", 4f);
            yield return new WaitForSeconds(2f);
            npc.SayNow("Anytime now", 3f);
            yield return new WaitForSeconds(.8f);
            GlobalInstanceManager.GetMainCharacter().GetComponent<MessageManager>()
                .ShowMessage(
                    "Press <color=#FFF02D>q</color> to start recording");
            yield return npc.Say("...There you go", 1.5f, 1f);
            yield return npc.Say("Let me elaborate. Press \"q\" to start recording.", 6f, 4f);
            yield return npc.Say("The lantern will record everything you do, and then create a clone.", 6f);
            yield return npc.Say("The clone will move (as you did while recording) for a few times.", 5f);
            yield return npc.Say("You can probably think of a way to use this power to get out.", 5f);
            yield return npc.Say("Maybe you can find a way to save me?", 4f);
            yield return npc.Say("The platform is too high for me. I'm sure you'll find a way.", 5f);

            OnDone();
        }

        private IEnumerator SecondDialogue(SpeechBubbleManager npc)
        {
            yield return npc.Say("I know you can do it.", 3f);
        }

        private IEnumerator SavedDialogue(SpeechBubbleManager npc)
        {
            yield return npc.Say("Thank you very much for saving me.", 3f);
            yield return npc.Say("My name is <color=#FFF02D>Elbert</color>.", 3f);
            yield return npc.Say("I'll give you a hint.", 2f);
            yield return npc.Say("There are some things you or your clone can do alone.", 5f);
            yield return npc.Say("But sometimes, it is best to join forces.", 4f);
            yield return npc.Say("Good luck.", 2f);
        }
    }
}