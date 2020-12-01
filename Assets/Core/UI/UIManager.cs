using System;
using System.Collections;
using NotTimeTravel.Core.Speech;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NotTimeTravel.Core.State;
using TMPro;
using UnityEngine.InputSystem;

namespace NotTimeTravel.Core.UI
{
    public class UIManager : MonoBehaviour
    {
        public EventSystem eventSystem;
        public Canvas ui;
        public Image blanket;
        public CanvasGroup uiGroup;
        public CanvasGroup finishUiGroup;
        public float fadeDuration;
        public TextMeshProUGUI timeText;
        public AudioSource[] soundsToFadeOut;

        private DebugStateManager _debug;
        private bool _listeningForAnyKey;
        private bool _skip;

        private void Start()
        {
            _debug = GlobalInstanceManager.GetDebugStateManager();

            if (_debug.skipTitleScreen)
            {
                DeactivateUiAndStartPlaying();
            }
            else
            {
                StartCoroutine(StartUI());
            }
        }

        private void Update()
        {
            if (!_listeningForAnyKey || _skip) return;

            InputAction myAction = new InputAction(binding: "/*/<button>");
            myAction.performed += context => _skip = true;
            myAction.Enable();
        }

        public IEnumerator ShowGameTime(float time)
        {
            uiGroup.gameObject.SetActive(false);
            ReactivateUi();
            finishUiGroup.gameObject.SetActive(true);
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            timeText.SetText($"You finished the game in <color=#FFF02D>{timeSpan:g}</color>");
            _listeningForAnyKey = true;

            while (!_skip)
            {
                yield return null;
            }

            foreach (AudioSource audioSource in soundsToFadeOut)
            {
                StartCoroutine(Transition.Transition.TransitionFloat(fadeDuration, () => audioSource.volume, 0,
                    newVolume => { audioSource.volume = newVolume; }));
            }

            yield return FadeBlanket(1);
        }

        private IEnumerator StartUI()
        {
            yield return FadeBlanket(0);
        }

        public void StartGame()
        {
            StartCoroutine(StartGameSequence());
        }

        private IEnumerator StartGameSequence()
        {
            yield return FadeBlanket(1);
            uiGroup.alpha = 0;
            yield return FadeBlanket(0);
            DeactivateUiAndStartPlaying();
        }

        public IEnumerator FadeBlanket(float target)
        {
            yield return Transition.Transition.TransitionFloat(fadeDuration, 1, target == 1 ? 0 : 1, target, newAlpha =>
            {
                Color blanketColor = blanket.color;
                blanketColor.a = newAlpha;
                blanket.color = blanketColor;
            });
        }

        private void DeactivateUiAndStartPlaying()
        {
            GlobalInstanceManager.GetAudioManager().FadeInCaveAmbience();
            ui.gameObject.SetActive(false);
            eventSystem.gameObject.SetActive(false);
            GlobalInstanceManager.GetFlowManager().StartPlaying();
        }

        public void ReactivateUi()
        {
            ui.gameObject.SetActive(true);
            eventSystem.gameObject.SetActive(true);
        }
    }
}