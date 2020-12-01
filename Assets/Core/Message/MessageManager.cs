using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NotTimeTravel.Core.Message
{
    public class MessageManager : MonoBehaviour
    {
        public Canvas canvas;
        public CanvasGroup panel;
        public Color popupColor;
        public Color textColor;
        public TextMeshProUGUI text;
        public float animationDuration = .2f;
        public float messageDuration = 4f;

        public void ShowMessage(string message, bool sticky = false)
        {
            canvas.GetComponentInChildren<Image>().color = popupColor;
            text.color = textColor;
            StopAllCoroutines();
            UpdateText(message);
            StartCoroutine(ShowMessageSequence(message, sticky));
        }

        public void ClearMessage()
        {
            StopAllCoroutines();
            StartCoroutine(ClearMessageSequence());
        }

        private void UpdateText(string message)
        {
            Vector2 values = text.GetPreferredValues(message);
            RectTransform rectTransform = canvas.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(values.x + 0.4f, rectTransform.rect.height);
            text.SetText(message);
        }

        private IEnumerator ShowMessageSequence(string message, bool sticky = false)
        {
            yield return Transition.Transition.TransitionFloat(animationDuration, 1, panel.alpha, 1,
                newAlpha => panel.alpha = newAlpha);
            yield return new WaitForSeconds(messageDuration);

            if (sticky || message != text.text)
            {
                yield break;
            }

            yield return ClearMessageSequence();
        }

        private IEnumerator ClearMessageSequence()
        {
            if (panel.alpha == 0)
            {
                yield break;
            }

            yield return Transition.Transition.TransitionFloat(animationDuration, 1, panel.alpha, 0,
                newAlpha => panel.alpha = newAlpha);
        }
    }
}