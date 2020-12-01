using System;
using NotTimeTravel.Core.Model;
using UnityEngine;

namespace NotTimeTravel.Core.Audio
{
    public class VoiceManager : MonoBehaviour
    {
        public SoundCollectionManager normal;
        public SoundCollectionManager angry;
        public SoundCollectionManager gentle;
        public SoundCollectionManager helpless;
        public SoundCollectionManager question;
        public SoundCollectionManager surprised;


        public void PlayVoice(VoiceTone voiceTone = VoiceTone.Normal)
        {
            switch (voiceTone)
            {
                case VoiceTone.Normal:
                    normal.PlayRandom();
                    break;
                case VoiceTone.Angry:
                    angry.PlayRandom();
                    break;
                case VoiceTone.Gentle:
                    gentle.PlayRandom();
                    break;
                case VoiceTone.Helpless:
                    helpless.PlayRandom();
                    break;
                case VoiceTone.Question:
                    question.PlayRandom();
                    break;
                case VoiceTone.Surprised:
                    surprised.PlayRandom();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(voiceTone), voiceTone, null);
            }
        }
    }
}