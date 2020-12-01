using System.Collections;
using NotTimeTravel.Core.Audio;
using NotTimeTravel.Core.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NotTimeTravel.Core.Speech
{
    public class SpeechBubbleManager : MonoBehaviour
    {
        public float animationDuration = .2f;
        public TextMeshProUGUI text;
        public CanvasGroup canvasGroup;
        public Canvas canvas;
        public Color popupColor;
        public Color textColor;
        public SpeechAlign speechAlign;

        private string _currentLine;
        private Image _popupInstance;

        private Image GetPopup()
        {
            if (_popupInstance == null)
            {
                _popupInstance = canvas.GetComponentInChildren<Image>();
            }

            return _popupInstance;
        }

        public IEnumerator Say(string line, float duration, float waitBefore = 0, float waitAfter = 0,
            VoiceTone voiceTone = default)
        {
            transform.parent.GetComponentInChildren<VoiceManager>()
                .PlayVoice(voiceTone == default ? VoiceTone.Normal : voiceTone);

            yield return AnimateSpeechBubble(line, duration, waitBefore, waitAfter);
        }

        public void SayNow(string line, float duration, float waitBefore = 0, float waitAfter = 0, VoiceTone voiceTone = default)
        {
            transform.parent.GetComponentInChildren<VoiceManager>()
                .PlayVoice(voiceTone == default ? VoiceTone.Normal : voiceTone);
            
            StartCoroutine(Say(line, duration, waitBefore, waitAfter));
        }

        private IEnumerator AnimateSpeechBubble(string line, float duration, float waitBefore = 0, float waitAfter = 0)
        {
            GetPopup().color = popupColor;
            text.color = textColor;
            _currentLine = line;
            yield return new WaitForSeconds(waitBefore);
            Vector2 values = text.GetPreferredValues(line);
            RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
            float newWidth = values.x + .3f;
            canvasRectTransform.sizeDelta = new Vector2(newWidth, canvasRectTransform.rect.height);
            Vector3 canvasPosition = canvasRectTransform.anchoredPosition;

            switch (speechAlign)
            {
                case SpeechAlign.Center:
                    canvasPosition.x = 0;
                    break;
                case SpeechAlign.Left:
                    canvasPosition.x = newWidth * 0.45f;
                    break;
                case SpeechAlign.Right:
                    canvasPosition.x = newWidth * -0.45f;
                    break;
            }

            canvasRectTransform.anchoredPosition = canvasPosition;

            text.SetText(line);
            yield return StartCoroutine(Animate(1));
            yield return new WaitForSeconds(duration);

            if (_currentLine != line)
            {
                yield break;
            }

            yield return StartCoroutine(Animate(0));
            yield return new WaitForSeconds(waitAfter);
        }

        private IEnumerator Animate(float target)
        {
            float currentAlpha = canvasGroup.alpha;
            float actualDuration = Mathf.Abs(target - currentAlpha) * animationDuration;
            float elapsedTime = 0;

            while (elapsedTime <= actualDuration)
            {
                float newAlpha = Mathf.Lerp(currentAlpha, target, elapsedTime / actualDuration);
                canvasGroup.alpha = newAlpha;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = target;
        }
    }
}