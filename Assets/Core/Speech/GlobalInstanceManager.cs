using System.Collections.Generic;
using NotTimeTravel.Core.Audio;
using NotTimeTravel.Core.Game;
using NotTimeTravel.Core.State;
using NotTimeTravel.Core.Camera;
using NotTimeTravel.Core.Resource;
using UnityEngine;

namespace NotTimeTravel.Core.Speech
{
    public static class GlobalInstanceManager
    {
        private static GameManager _gameManager;
        private static FlowManager _flowManager;
        private static StateManager _stateManager;
        private static InventoryManager _inventoryManager;
        private static CameraManager _cameraManager;
        private static AssetManager _assetManager;
        private static AudioManager _audioManager;
        private static DebugStateManager _debug;
        private static readonly Dictionary<string, GameObject> CachedCharacters = new Dictionary<string, GameObject>();

        public static void Reload()
        {
            _gameManager = null;
            _flowManager = null;
            _stateManager = null;
            _inventoryManager = null;
            _cameraManager = null;
            _assetManager = null;
            _audioManager = null;
            _debug = null;
            CachedCharacters.Clear();
        }

        public static GameManager GetGameManager()
        {
            if (_gameManager == null)
            {
                _gameManager = GameObject.FindGameObjectWithTag("Game").GetComponent<GameManager>();
            }

            return _gameManager;
        }

        public static FlowManager GetFlowManager()
        {
            if (_flowManager == null)
            {
                _flowManager = GameObject.FindGameObjectWithTag("Environment").GetComponent<FlowManager>();
            }

            return _flowManager;
        }

        public static StateManager GetStateManager()
        {
            if (_stateManager == null)
            {
                _stateManager = GameObject.FindGameObjectWithTag("Environment").GetComponent<StateManager>();
            }

            return _stateManager;
        }

        public static InventoryManager GetInventoryManager()
        {
            if (_inventoryManager == null)
            {
                _inventoryManager = GameObject.FindGameObjectWithTag("Environment").GetComponent<InventoryManager>();
            }

            return _inventoryManager;
        }

        public static AssetManager GetAssetManager()
        {
            if (_assetManager == null)
            {
                _assetManager = GameObject.FindGameObjectWithTag("World").GetComponent<AssetManager>();
            }

            return _assetManager;
        }

        public static GameObject GetMainCharacter()
        {
            return GetCharacter("Main");
        }

        public static GameObject GetCharacter(string identifier)
        {
            if (!CachedCharacters.ContainsKey(identifier))
            {
                CachedCharacters.Add(identifier, GameObject.FindGameObjectWithTag($"{identifier}Character"));
            }

            return CachedCharacters[identifier];
        }

        public static CameraManager GetCameraManager()
        {
            if (_cameraManager == null)
            {
                _cameraManager = GameObject.FindGameObjectWithTag("MainVirtualCamera")
                    .GetComponent<CameraManager>();
            }

            return _cameraManager;
        }

        public static AudioManager GetAudioManager()
        {
            if (_audioManager == null)
            {
                _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
            }

            return _audioManager;
        }

        public static DebugStateManager GetDebugStateManager()
        {
            if (_debug == null)
            {
                _debug = GameObject.FindGameObjectWithTag("Game").GetComponent<DebugStateManager>();
            }

            return _debug;
        }
    }
}