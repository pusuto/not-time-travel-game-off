using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace NotTimeTravel.Core.Camera
{
    public class CameraManager : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;
        private readonly Dictionary<string, GameObject> _cachedCharacters = new Dictionary<string, GameObject>();

        public void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public void FocusOnCharacter(string identifier = "Main")
        {
            if (!_cachedCharacters.ContainsKey(identifier))
            {
                _cachedCharacters.Add(identifier, GameObject.FindGameObjectWithTag($"{identifier}Character"));
            }

            _virtualCamera.Follow = _cachedCharacters[identifier].GetComponent<Transform>();
        }
    }
}