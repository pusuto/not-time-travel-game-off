using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.NPC;
using NotTimeTravel.Core.Grid;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Speech.Dialogue
{
    public class CecilyDialogue : MonoBehaviour, IDialogue
    {
        private bool _hasPresented;

        public IEnumerator GetDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            if (!_hasPresented)
            {
                _hasPresented = true;
                yield return GetFirstDialogue(npc, main);
            }

            IEnumerable<NpcManager> managers = GlobalInstanceManager.GetGameManager()
                .GetComponentsInChildren<NpcManager>()
                .Where(manager => manager.needToBeSaved);
            IEnumerable<NpcManager> npcManagers = managers.ToList();
            int total = npcManagers.Count();
            int saved = npcManagers.Count(manager => manager.IsSaved());

            yield return npc.Say(
                $"You have saved <color=#FFF02D>{saved}</color> out of <color=#FFF02D>{total}</color> prisoners.", 5f);

            if (saved == 0)
            {
                yield return npc.Say("Better get started on that, what do you think?", 4f);
            }

            if (saved == total)
            {
                yield return npc.Say("Good job on saving everyone!", 3f, 0, 0, VoiceTone.Angry);
            }

            IEnumerable<HiddenPassageManager> passages = GlobalInstanceManager.GetGameManager()
                .GetComponentsInChildren<HiddenPassageManager>();
            const int totalPassages = 7;
            int foundPassages = totalPassages - passages.Count();

            yield return npc.Say(
                $"You have found <color=#FFF02D>{foundPassages}</color> out of <color=#FFF02D>{totalPassages}</color> hidden passages.",
                5f);

            if (totalPassages == foundPassages)
            {
                yield return npc.Say("Good job on finding every hidden passage!", 3f);
            }

            yield return npc.Say("See you next time!", 2f);
        }

        private IEnumerator GetFirstDialogue(SpeechBubbleManager npc, SpeechBubbleManager main)
        {
            yield return npc.Say("Welcome!", 1f);
            yield return npc.Say("Escaping from this dungeon is not easy.", 4f);
            yield return npc.Say("It's already too much dealing with the fact of being kept here.", 5f);
            yield return npc.Say("And on top of that, you need to keep track of a bunch of stuff, too!", 5f);
            yield return npc.Say("But fear not. I am here to help.", 3f);
            yield return npc.Say("Bringing information about your escape, directly to your cell doorstep!", 5f);
            yield return main.Say("Hm.", 1f);
            yield return main.Say("Ok, I guess?", 1f, 0, 0, VoiceTone.Question);
        }
    }
}