using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NotTimeTravel.Core.Input
{
    public class InputManager : MonoBehaviour
    {
        private NotTimeTravelInput _input;
        private static InputManager _instance;

        private void Awake()
        {
            _input = new NotTimeTravelInput();
            _input.Enable();
        }

        private static InputManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = GameObject.FindGameObjectWithTag("Environment").GetComponent<InputManager>();
            }

            return _instance;
        }

        private static void RegisterInputCallback(InputAction action, Action<InputAction.CallbackContext> handler,
            bool remove = false)
        {
            if (remove)
            {
                action.started -= handler;
                action.performed -= handler;
                action.canceled -= handler;
            }
            else
            {
                action.started += handler;
                action.performed += handler;
                action.canceled += handler;
            }
        }

        public static void OnMove(Action<InputAction.CallbackContext> handler, bool remove = false)
        {
            RegisterInputCallback(GetInstance()._input.Player.Move, handler, remove);
        }

        public static void OnJump(Action<InputAction.CallbackContext> handler, bool remove = false)
        {
            RegisterInputCallback(GetInstance()._input.Player.Jump, handler, remove);
        }

        public static void OnRecord(Action<InputAction.CallbackContext> handler, bool remove = false)
        {
            RegisterInputCallback(GetInstance()._input.Player.Record, handler, remove);
        }

        public static void OnInteract(Action<InputAction.CallbackContext> handler, bool remove = false)
        {
            RegisterInputCallback(GetInstance()._input.Player.Interact, handler, remove);
        }

        public static void OnPull(Action<InputAction.CallbackContext> handler, bool remove = false)
        {
            RegisterInputCallback(GetInstance()._input.Player.Pull, handler, remove);
        }
    }
}