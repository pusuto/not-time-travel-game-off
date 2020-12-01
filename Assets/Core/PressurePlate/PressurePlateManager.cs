using System.Collections;
using NotTimeTravel.Core.Block;
using NotTimeTravel.Core.Message;
using NotTimeTravel.Core.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace NotTimeTravel.Core.PressurePlate
{
    public class PressurePlateManager : MonoBehaviour
    {
        public string outOfOrderMessage = "Out of order";
        public string repairMessage = "Repaired";
        public string alreadyTriggeredMessage = "Inactive";
        public bool isInitiallyOperational;
        public Color zeroColor;
        public Color fullColor;
        public DetectionType detectionType;
        public float forceNeeded = 50;
        public bool usePositionInstead;
        public Transform targetPosition;
        public float animationDuration = .2f;
        public UnityEvent onActivated;
        public UnityEvent onDeactivated;
        public Times times;
        public MessageManager invalidMessage;
        public MessageManager validMassage;
        public AudioSource bounceAudio;
        public bool useXAxis;
        [ColorUsage(true, true)] public Color targetZeroColor;
        [ColorUsage(true, true)] public Color targetFullColor;
        public Renderer progressMaterialRenderer;
        public bool hasProgress = true;
        public GameObject progressBackgroundObject;
        public GameObject normalBackgroundObject;

        private bool _isOperational;
        private bool _isActive;
        private bool _hasActivated;
        private bool _hasShownRepairMessage;
        private Light2D _light;
        private SpringJoint2D _springJoint;
        private float _initialLightIntensity;
        private bool _hasPlayedBounce;
        private Vector3 _originalPosition;
        private float _originalDistance;

        private static readonly int Progress = Shader.PropertyToID("Progress");

        private void Start()
        {
            progressBackgroundObject.SetActive(hasProgress);
            normalBackgroundObject.SetActive(!hasProgress);

            targetPosition.gameObject.SetActive(usePositionInstead);
            _isOperational = isInitiallyOperational;
            _light = GetComponentInChildren<Light2D>();
            _initialLightIntensity = _light.intensity;
            _springJoint = GetComponentInChildren<SpringJoint2D>();
            _originalPosition = transform.position;
            _originalDistance = Mathf.Abs(Vector2.Distance(_originalPosition, targetPosition.position));
            SetupSpring();
        }

        public void SetOperational(bool isOperational)
        {
            if (!_hasShownRepairMessage && isOperational)
            {
                _hasShownRepairMessage = true;
                validMassage.ShowMessage(repairMessage);
            }

            _isOperational = isOperational;
            SetupSpring();
        }

        private void SetupSpring()
        {
            _light.intensity = _isOperational ? _initialLightIntensity : 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isOperational || !other.gameObject.CompareTag("MainCharacter")) return;

            invalidMessage.ShowMessage(CanBeActivatedAgain() ? outOfOrderMessage : alreadyTriggeredMessage, true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!CanBeActivatedAgain())
            {
                SetOperational(false);
            }

            validMassage.ClearMessage();
        }

        private float GetAxisValue(Vector3 vector)
        {
            return useXAxis ? vector.x : vector.y;
        }

        private void FixedUpdate()
        {
            float reactionForceOnAxis = GetAxisValue(_springJoint.reactionForce);
            if (reactionForceOnAxis < -15 && !_hasPlayedBounce)
            {
                _hasPlayedBounce = true;
                bounceAudio.PlayOneShot(bounceAudio.clip);
            }

            if (Mathf.Abs(GetAxisValue(_originalPosition) -
                          GetAxisValue(_springJoint.gameObject.transform.position)) < .01f)
            {
                _hasPlayedBounce = false;
            }

            if (!_isOperational)
            {
                return;
            }

            float targetProgress;
            float currentProgress = progressMaterialRenderer.material.GetFloat(Progress);

            if (usePositionInstead)
            {
                float currentDistance = Mathf.Abs(Vector2.Distance(_springJoint.gameObject.transform.position,
                    targetPosition.position));
                targetProgress = Mathf.Clamp(1 - currentDistance / _originalDistance + .015f, 0, 1);
            }
            else
            {
                float detectedForce = reactionForceOnAxis > 0 ? 0 : Mathf.Abs(reactionForceOnAxis);
                targetProgress = detectedForce / forceNeeded;
            }

            if (targetProgress >= 1)
            {
                Activate();
            }
            else if (_isActive)
            {
                Deactivate();
            }

            if (!hasProgress)
            {
                return;
            }

            StartCoroutine(AnimateProgress(currentProgress, targetProgress));
            StartCoroutine(AnimateColor(currentProgress, targetProgress));
        }

        private IEnumerator AnimateProgress(float currentProgress, float targetProgress)
        {
            float range = Mathf.Abs(currentProgress - targetProgress);
            yield return Transition.Transition.TransitionFloat(animationDuration, range, currentProgress,
                targetProgress, newProgress => progressMaterialRenderer.material.SetFloat(Progress, newProgress), true);
        }

        private IEnumerator AnimateColor(float currentProgress, float targetProgress)
        {
            Color currentColor = Color.Lerp(zeroColor, fullColor, currentProgress);
            Color targetColor = Color.Lerp(zeroColor, fullColor, targetProgress);
            yield return Transition.Transition.TransitionColor(animationDuration, currentColor, targetColor,
                color => _light.color = color, true);
            Color currentTargetColor = Color.Lerp(targetZeroColor, targetFullColor, currentProgress);
            Color targetTargetColor = Color.Lerp(targetZeroColor, targetFullColor, targetProgress);
            StartCoroutine(Transition.Transition.TransitionColor(animationDuration, currentTargetColor,
                targetTargetColor,
                newColor => targetPosition.GetComponent<SpriteRenderer>().color = newColor));
        }

        private void Activate()
        {
            if (!CanBeActivatedAgain())
            {
                return;
            }

            _hasActivated = true;
            _isActive = true;
            onActivated.Invoke();
        }

        private bool CanBeActivatedAgain()
        {
            return !_hasActivated || times != Times.Once;
        }

        private void Deactivate()
        {
            _isActive = false;
            onDeactivated.Invoke();
        }

        private void OnValidate()
        {
            GetComponentInChildren<BlockManager>().skipBackground = hasProgress;
            GetComponentInChildren<BlockManager>().background =
                (hasProgress ? progressBackgroundObject : normalBackgroundObject).GetComponent<SpriteRenderer>();
        }
    }
}