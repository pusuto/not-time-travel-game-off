using NotTimeTravel.Core.Input;
using UnityEngine;
using NotTimeTravel.Core.Model;
using NotTimeTravel.Core.Speech;
using UnityEngine.InputSystem;

namespace NotTimeTravel.Core.Player
{
    public class RecordingManager : MonoBehaviour
    {
        public int slots;
        public int availableSeconds;
        public int repeatTimes = 2;
        public GameObject characterPrefab;
        public Color[] colors;

        private bool _isRecording;
        private Recording _currentRecording;
        private float _startedRecordingAt;
        private ProgressManager _progressManager;
        private bool _forceRecordingEnd;

        private void Start()
        {
            _progressManager = GetComponent<ProgressManager>();
            InputManager.OnMove(OnMove);
            InputManager.OnJump(OnJump);

            if (GetComponent<CharacterManager>().isMain)
            {
                InputManager.OnRecord(OnRecord);
            }
        }

        private void OnDestroy()
        {
            InputManager.OnMove(OnMove, true);
            InputManager.OnJump(OnJump, true);
            InputManager.OnRecord(OnRecord, true);
        }

        public void CloneWillBeDestroyed()
        {
            slots++;
        }

        public void FixedUpdate()
        {
            if (!_isRecording || _startedRecordingAt == 0f || _currentRecording == null)
            {
                return;
            }

            float elapsedTime = Time.fixedTime - _startedRecordingAt;
            float elapsedPercentage = elapsedTime / availableSeconds;

            _progressManager.UpdatePercentage(elapsedPercentage);

            if (elapsedTime < availableSeconds && !_forceRecordingEnd) return;

            _forceRecordingEnd = false;
            _isRecording = false;
            _progressManager.ResetAccent();

            _currentRecording.AddEvent(new RecordingEvent()
            {
                EventType = RecordingEventType.Disappear
            });

            GameObject playback = Instantiate(characterPrefab, new Vector2(-1000, -1000), Quaternion.identity);
            playback.tag = "Clone";
            playback.GetComponentInChildren<SpeechBubbleManager>().gameObject.SetActive(false);
            CharacterManager characterManager = playback.GetComponent<CharacterManager>();
            characterManager.isMain = false;
            characterManager.color = colors[slots];
            PlaybackManager playbackManager = playback.GetComponent<PlaybackManager>();
            playbackManager.times = repeatTimes;
            playbackManager.RecordingPlayback = new RecordingPlayback()
            {
                Recording = _currentRecording
            };
            playbackManager.recordingManager = this;
            playback.SetActive(true);

            _isRecording = false;
            _startedRecordingAt = 0f;
        }

        public void OnRecord(InputAction.CallbackContext context)
        {
            if (!GlobalInstanceManager.GetStateManager().CanMove)
            {
                return;
            }

            if (!GlobalInstanceManager.GetStateManager().CanUsePower)
            {
                return;
            }

            if (!context.performed)
            {
                return;
            }

            if (_isRecording)
            {
                _forceRecordingEnd = true;
                return;
            }

            if (slots <= 0)
            {
                return;
            }

            slots--;

            _isRecording = true;
            _startedRecordingAt = Time.fixedTime;
            _progressManager.FadeInAccent();

            Recording recording = new Recording();
            recording.AddEvent(new RecordingEvent()
            {
                Position = transform.position,
                Velocity = GetComponent<Rigidbody2D>().velocity,
                HorizontalMovement = GetComponent<PlayerMovement>().GetHorizontalMovement(),
                EventType = RecordingEventType.Appear
            });
            _currentRecording = recording;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();
            float movementX = movement.x;

            if (_isRecording && _currentRecording is Recording currentRecording)
            {
                currentRecording.AddEvent(new RecordingEvent()
                {
                    EventType = movementX == 0
                        ? RecordingEventType.StopMoving
                        : (movementX > 0 ? RecordingEventType.MoveEast : RecordingEventType.MoveWest)
                });
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed && !context.canceled)
            {
                return;
            }

            if (_isRecording && _currentRecording is Recording currentRecording)
            {
                currentRecording.AddEvent(new RecordingEvent()
                {
                    EventType = context.performed ? RecordingEventType.Jump : RecordingEventType.StopHoldingJump
                });
            }
        }

        public void IncreaseSeconds()
        {
            availableSeconds++;
        }

        public void IncreaseSlots()
        {
            slots++;
        }

        public void IncreaseRepeatTimes()
        {
            repeatTimes++;
        }
    }
}