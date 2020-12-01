using System.Collections;
using NotTimeTravel.Core.Speech;

namespace NotTimeTravel.Core.Model
{
    public interface IDialogue
    {
        IEnumerator GetDialogue(SpeechBubbleManager npc, SpeechBubbleManager main);
    }
}