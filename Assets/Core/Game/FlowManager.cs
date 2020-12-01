using System;
using System.Collections;
using NotTimeTravel.Core.State;
using NotTimeTravel.Core.Message;
using NotTimeTravel.Core.Model;
using NotTimeTravel.Core.Player;
using NotTimeTravel.Core.Speech;
using NotTimeTravel.Core.UI;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace NotTimeTravel.Core.Game
{
    public class FlowManager : MonoBehaviour
    {
        private StateManager _stateManager;
        private GameObject _mainCharacter;
        private DebugStateManager _debug;

        #region Start of the game

        public float waitBeforeHint = 4f;

        private float _startingTime;

        private void Awake()
        {
            _stateManager = GlobalInstanceManager.GetStateManager();
            _mainCharacter = GlobalInstanceManager.GetMainCharacter();
            _debug = GlobalInstanceManager.GetDebugStateManager();
        }

        private float GetGameTime()
        {
            return Time.time - _startingTime;
        }

        public void StartPlaying()
        {
            _startingTime = Time.time;
            _stateManager.CanMove = true;
            StartCoroutine(StartOfTheGame());
            _stateManager.StateChanged += OnLanternGetAndLose;

            if (_debug.takeLanternImmediately)
            {
                _stateManager.HasLantern = true;
            }

            if (_debug.skipMonologue)
            {
                StartOfTheGameHasJumped();
            }

            if (_debug.startWithPower)
            {
                _stateManager.CanUsePower = true;
            }
        }

        private IEnumerator StartOfTheGame()
        {
            yield return new WaitForSeconds(waitBeforeHint);

            GlobalInstanceManager.GetMainCharacter().GetComponent<MessageManager>()
                .ShowMessage(
                    "Use the <color=#FFF02D>arrow keys</color> to move. <color=#FFF02D><space bar></color> to jump. <color=#FFF02D>W</color> to pull boxes");
        }

        #endregion

        #region Start Of The Game HasJumped

        public float waitBeforeFirstThoughts = 3f;
        public float waitBeforeFallingObjects = 3f;
        public GameObject fallingBox;
        public GameObject fallingLantern;

        public void StartOfTheGameHasJumped()
        {
            StartCoroutine(StartOfTheGameHasJumpedSequence());
        }

        private IEnumerator StartOfTheGameHasJumpedSequence()
        {
            if (!_debug.skipMonologue)
            {
                yield return GlobalSpeechManager.MainCharacterSay("So this looks like a prison.", 2f,
                    waitBeforeFirstThoughts);
                yield return GlobalSpeechManager.MainCharacterSay(
                    "Set up in a way that makes escape possible, but challenging.", 5f, 3f);
                yield return GlobalSpeechManager.MainCharacterSay("Still, I can't seem to get up there.", 3f, 3f);
                yield return GlobalSpeechManager.MainCharacterSay(
                    "I hope a certain developer didn't screw up the actual beginning of the game.", 6f, 3f);

                yield return new WaitForSeconds(waitBeforeFallingObjects);
            }

            fallingLantern.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            yield return GlobalSpeechManager.MainCharacterSay("Wat", 2f, 1f);
        }

        #endregion

        #region Lantern Interaction

        public float lanternGetDuration = .4f;

        public void OnInteractWithLantern()
        {
            _stateManager.HasLantern = true;
        }

        private void OnLanternGetAndLose(object caller, EventArgs change)
        {
            if (change is StateChangeArgs<bool> boolChange && boolChange.PropertyName == "HasLantern")
            {
                StartCoroutine(boolChange.NewValue ? OnInteractWithLanternSequence() : OnLoseLanternSequence());
            }
        }

        private IEnumerator OnInteractWithLanternSequence()
        {
            _stateManager.CanMove = false;
            Light2D fallingLight = fallingLantern.GetComponentInChildren<Light2D>();
            Light2D characterLight = _mainCharacter.GetComponentInChildren<Light2D>();
            CharacterManager characterManager = _mainCharacter.GetComponentInChildren<CharacterManager>();

            yield return GlobalSpeechManager.MainCharacterSay("What's this?", 2f);

            yield return Transition.Transition.TransitionFloat(lanternGetDuration, fallingLight.intensity,
                fallingLight.intensity,
                0,
                newIntensity => fallingLight.intensity = newIntensity);
            Destroy(fallingLantern);
            StartCoroutine(Transition.Transition.TransitionColor(lanternGetDuration, characterManager.startingColor,
                characterManager.color, newColor => characterManager.SetColor(newColor)));
            yield return Transition.Transition.TransitionFloat(lanternGetDuration, .8f,
                0,
                1f,
                newIntensity => characterLight.intensity = newIntensity);
            _stateManager.CanMove = true;
            yield return new WaitForSeconds(3f);
            fallingBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            yield return GlobalSpeechManager.MainCharacterSay("Can everyone please stop dropping stuff on me?", 5f, 1f);
        }

        private IEnumerator OnLoseLanternSequence()
        {
            Light2D characterLight = _mainCharacter.GetComponentInChildren<Light2D>();
            CharacterManager characterManager = _mainCharacter.GetComponentInChildren<CharacterManager>();

            StartCoroutine(Transition.Transition.TransitionColor(lanternGetDuration, characterManager.color,
                characterManager.startingColor, newColor => characterManager.SetColor(newColor)));
            yield return Transition.Transition.TransitionFloat(lanternGetDuration, 1f,
                1f,
                0f,
                newIntensity => characterLight.intensity = newIntensity);
        }

        #endregion

        public void EndGame()
        {
            StartCoroutine(EndGameSequence());
        }

        private IEnumerator EndGameSequence()
        {
            float gameTime = GetGameTime();
            UIManager uiManager = GlobalInstanceManager.GetGameManager().GetComponentInChildren<UIManager>();
            GlobalInstanceManager.GetStateManager().CanMove = false;
            GlobalInstanceManager.GetMainCharacter().GetComponent<CharacterController2D>().MakeSureFacing(true);
            yield return new WaitForSeconds(5);
            yield return uiManager.ShowGameTime(gameTime);
            GlobalInstanceManager.Reload();
            SceneManager.LoadScene(0);
        }
    }
}