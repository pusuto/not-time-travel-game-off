using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.NPC;
using NotTimeTravel.Core.Door;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Speech.Dialogue
{
    public class GattulinosDialogue : MonoBehaviour, IDialogue
    {
        public DoorManager lastDoor;

        private bool _hasPresented;

        public IEnumerator GetDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            if (!_hasPresented)
            {
                yield return GetFirstDialogue(npc, main);
            }

            yield return npc.Say("Welcome back to the outside, friend.", 4f);
        }

        private IEnumerator GetFirstDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            IEnumerable<NpcManager> managers = GlobalInstanceManager.GetGameManager()
                .GetComponentsInChildren<NpcManager>()
                .Where(manager => manager.needToBeSaved);
            IEnumerable<NpcManager> npcManagers = managers.ToList();
            int total = npcManagers.Count();
            int saved = npcManagers.Count(manager => manager.IsSaved());
            bool hasSavedEveryone = total == saved;

            yield return npc.Say("Congratulations, you have almost reached the exit.", 3f);
            yield return npc.Say("Unfortunately, it seems there is no way to open this door.", 5f);
            yield return main.Say("Are you the warden of this prison?", 3f);
            yield return npc.Say("Am I? Well. Yes.", 3f);
            yield return npc.Say("And so are you.", 3f);
            yield return npc.Say("And so is everyone here.", 3f);
            yield return npc.Say("Each prisoner, warden of a prison called <color=#FFF02D>self</color>.", 5f);
            yield return npc.Say("Curious, right?", 2f);
            yield return main.Say("I am not sure I understand.", 3f);
            yield return npc.Say("You might, in time.", 3f);
            yield return npc.Say("Still, in your shoes, I would be very proud of myself.", 5f);
            yield return npc.Say("You managed to exit your cell. Alone.", 4f);
            yield return main.Say("I had help. Someone throw a magical lantern (and a rock) right at me.", 5f);
            yield return npc.Say("I will repeat. You managed to exit your cell... alone.", 5f);
            yield return npc.Say("You managed to bring yourself here.", 4f);
            yield return npc.Say("You maybe didn't realize: that was already the most important step.", 5f);
            yield return npc.Say("While, this exit here? Just well-deserved joy.", 4f);
            yield return main.Say("What about the other prisoners? They were less lucky.", 5f);

            if (hasSavedEveryone)
            {
                yield return npc.Say("You <i>did</i> save everyone.", 3f);
                yield return npc.Say("Wouldn't you say, in this case, they were the luckiest ones?", 5f);
            }
            else
            {
                yield return npc.Say("You could have saved everyone. But you didn't.", 4f);
                yield return npc.Say("I expect you'll be thinking about that on your way out.", 5f);
            }

            yield return main.Say("What about this door, then?", 4f);
            yield return npc.Say("It won't open.", 2f);
            yield return main.Say("I... I don't accept that.", 3f);
            yield return npc.Say("You don't, uh?", 2f);
            yield return npc.Say("In the face of this insurmountable obstacle, you do not budge?", 5f);
            yield return npc.Say("When given all the wrong cards, do you not give up the game?", 5f);
            yield return npc.Say("Do you want to get out, even when everything screams at you, that you cannot?", 6f);

            yield return new WaitForSeconds(2f);

            yield return main.Say("You already know the answer to that.", 4f);
            yield return main.Say("I will chew my way out, if necessary.", 4f);
            yield return npc.Say("I see.", 1f);
            yield return npc.Say("It was not easy. Others had given up.", 4f);
            yield return npc.Say("But you had something in you that made the impossible, possible.", 5f);
            yield return npc.Say("If there is anything you should remember from this journey, let it be this:", 5f);
                
            yield return npc.Say("That power means nothing, if you only use it for yourself.", 4f);

            lastDoor.StartOpening();

            yield return npc.Say("Welcome back to the outside, friend.", 4f);
            yield return main.Say("What about this lantern?", 3f, 1f, 0, VoiceTone.Question);
            yield return npc.Say("I'll take it.", 2f);

            GlobalInstanceManager.GetStateManager().HasLantern = false;
            GlobalInstanceManager.GetStateManager().CanUsePower = false;

            yield return npc.Say("This lantern has more to offer.", 4f, 1f);
            yield return npc.Say("I know what to do with it. As for you...", 4f);
            yield return npc.Say("You should go now.", 3f);

            _hasPresented = true;
        }
    }
}