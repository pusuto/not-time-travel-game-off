using System;
using System.Collections;
using NotTimeTravel.Core.Message;
using NotTimeTravel.Core.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace NotTimeTravel.Core.Door
{
    public class DoorManager : MonoBehaviour
    {
        public Vector3 openPositionShift;
        public float lightChangeDuration = .4f;
        public float openingMovementSpeed = .5f;
        public float closingMovementSpeed = .5f;
        public Color lockedColor;
        public Color closedColor;
        public Color openingColor;
        public Color closingColor;
        public Color openColor;
        public DoorStatus initialStatus;
        public MessageManager messageManager;
        public bool showUnlockMessage;
        public string unlockMessage = "Woot woot";
        public UnityEvent hasOpened;
        public UnityEvent hasClosed;
        public AudioSource doorSound;
        public AudioSource doorThudSound;

        private Vector3 _closedPosition;
        private Vector3 _openPosition;
        private DoorStatus _status;
        private Light2D _light;
        private Rigidbody2D _rigidbody;
        private bool _hasPlayedStopSign = true;
        private bool _shouldPlaySound;

        public void Start()
        {
            _closedPosition = transform.position;
            _openPosition = _closedPosition + openPositionShift;
            _light = GetComponentInChildren<Light2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            SetStatus(initialStatus);
        }

        private void FixedUpdate()
        {
            Vector3 target = GetTargetPosition();

            if (Vector3.Distance(transform.position, target) < 0.03f)
            {
                if (!_hasPlayedStopSign)
                {
                    _hasPlayedStopSign = true;
                    _shouldPlaySound = false;
                    doorThudSound.PlayOneShot(doorThudSound.clip);
                    doorSound.Stop();
                }

                if (_status == DoorStatus.Opening)
                {
                    SetStatus(DoorStatus.Open);
                    hasOpened.Invoke();
                }

                if (_status == DoorStatus.Closing)
                {
                    SetStatus(DoorStatus.Closed);
                    hasClosed.Invoke();
                }

                _rigidbody.velocity = Vector2.zero;

                return;
            }

            _hasPlayedStopSign = false;

            if (_shouldPlaySound)
            {
                if (_rigidbody.bodyType == RigidbodyType2D.Dynamic)
                {
                    if (_rigidbody.velocity.magnitude <= 0.01)
                    {
                        doorSound.Pause();
                    }
                    else
                    {
                        doorSound.UnPause();
                    }
                }
            }
            else if (!doorSound.isPlaying)
            {
                doorSound.Play();
                _shouldPlaySound = true;
                _hasPlayedStopSign = false;
            }

            if (_rigidbody.bodyType == RigidbodyType2D.Dynamic)
            {
                _rigidbody.AddForce((target - transform.position) * (GetSpeed() * Time.fixedDeltaTime));
            }
            else
            {
                float step = GetSpeed() * Time.fixedDeltaTime;
                Vector3 newPosition = Vector3.MoveTowards(transform.position, target, step);
                _rigidbody.MovePosition(newPosition);
            }
        }

        public void SetStatus(DoorStatus status)
        {
            if (_status == DoorStatus.Open && status == DoorStatus.Opening ||
                _status == DoorStatus.Closed && status == DoorStatus.Closing)
            {
                return;
            }

            if (_status == DoorStatus.Locked && status == DoorStatus.Closed && messageManager != null &&
                showUnlockMessage)
            {
                messageManager.ShowMessage(unlockMessage);
            }

            _status = status;
            StopAllCoroutines();
            StartCoroutine(SetStatusSequence());
        }

        public DoorStatus GetStatus()
        {
            return _status;
        }

        private IEnumerator SetStatusSequence()
        {
            yield return Transition.Transition.TransitionColor(lightChangeDuration, _light.color, GetColor(),
                color => _light.color = color);
        }

        private float GetSpeed()
        {
            switch (GetDirection())
            {
                case DoorDirection.Closing:
                    return closingMovementSpeed;
                case DoorDirection.Opening:
                    return openingMovementSpeed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Color GetColor()
        {
            switch (_status)
            {
                case DoorStatus.Locked:
                    return lockedColor;
                case DoorStatus.Closed:
                    return closedColor;
                case DoorStatus.Opening:
                    return openingColor;
                case DoorStatus.Closing:
                    return closingColor;
                case DoorStatus.Open:
                    return openColor;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Vector3 GetTargetPosition()
        {
            switch (GetDirection())
            {
                case DoorDirection.Closing:
                    return _closedPosition;
                case DoorDirection.Opening:
                    return _openPosition;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private DoorDirection GetDirection()
        {
            switch (_status)
            {
                case DoorStatus.Locked:
                    return DoorDirection.Closing;
                case DoorStatus.Closed:
                    return DoorDirection.Closing;
                case DoorStatus.Opening:
                    return DoorDirection.Opening;
                case DoorStatus.Closing:
                    return DoorDirection.Closing;
                case DoorStatus.Open:
                    return DoorDirection.Opening;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ToggleStatus()
        {
            switch (_status)
            {
                case DoorStatus.Open:
                    StartClosing();
                    break;
                case DoorStatus.Closed:
                    StartOpening();
                    break;
                case DoorStatus.Locked:
                    break;
                case DoorStatus.Opening:
                    break;
                case DoorStatus.Closing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void StartOpening()
        {
            SetStatus(DoorStatus.Opening);
        }

        public void StartClosing()
        {
            SetStatus(DoorStatus.Closing);
        }

        public void Lock()
        {
            SetStatus(DoorStatus.Locked);
        }

        [ContextMenu("Set closed position")]
        public void SetClosedPosition()
        {
            _closedPosition = transform.position;
        }

        [ContextMenu("Set open position")]
        public void SetOpenPosition()
        {
            _openPosition = transform.position;
        }
    }
}