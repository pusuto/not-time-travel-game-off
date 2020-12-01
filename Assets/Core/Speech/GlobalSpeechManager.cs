using System.Collections;
using System.Collections.Generic;

namespace NotTimeTravel.Core.Speech
{
    public static class GlobalSpeechManager
    {
        private static readonly Dictionary<string, SpeechBubbleManager> CachedSpeechBubbleManagers =
            new Dictionary<string, SpeechBubbleManager>();

        public static IEnumerator MainCharacterSay(string line, float duration, float waitBefore = 0, float waitAfter = 0)
        {
            return CharacterSay("Main", line, duration, waitBefore, waitAfter);
        }

        public static void MainCharacterSayNow(string line, float duration, float waitBefore = 0, float waitAfter = 0)
        {
            CharacterSayNow("Main", line, duration, waitBefore, waitAfter);
        }
        
        public static IEnumerator CharacterSay(string identifier, string line, float duration, float waitBefore = 0,
            float waitAfter = 0)
        {
            return GetSpeechBubbleManager(identifier).Say(line, duration, waitBefore, waitAfter);
        }

        public static void CharacterSayNow(string identifier, string line, float duration, float waitBefore = 0,
            float waitAfter = 0)
        {
            GetSpeechBubbleManager(identifier).SayNow(line, duration, waitBefore, waitAfter);
        }

        public static SpeechBubbleManager GetSpeechBubbleManager(string identifier)
        {
            if (!CachedSpeechBubbleManagers.ContainsKey(identifier))
            {
                CachedSpeechBubbleManagers.Add(identifier,
                    GlobalInstanceManager.GetCharacter(identifier).GetComponentInChildren<SpeechBubbleManager>());
            }

            return CachedSpeechBubbleManagers[identifier];
        }
    }
}