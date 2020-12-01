using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace NotTimeTravel.Core.Player
{
    public class CharacterManager : MonoBehaviour
    {
        public bool isMain;
        public Color startingColor;
        public Color color;

        private static readonly int CharacterTint = Shader.PropertyToID("CharacterTint");
        private static readonly int VisibilityPercentage = Shader.PropertyToID("VisibilityPercentage");

        private void Start()
        {
            GetComponentInChildren<Light2D>().color = color;

            if (isMain)
            {
                SetColor(startingColor);
                GetComponent<RecordingManager>().enabled = true;
                GetComponentInChildren<Renderer>().material.SetFloat(VisibilityPercentage, 1);
            }
            else
            {
                SetColor(color);
                GetComponent<PlaybackManager>().enabled = true;
            }
        }

        public void SetColor(Color setColor)
        {
            GetComponentInChildren<Renderer>().material.SetColor(CharacterTint, setColor);
        }
    }
}