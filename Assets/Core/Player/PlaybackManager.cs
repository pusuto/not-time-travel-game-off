using System;
using NotTimeTravel.Core.Model;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace NotTimeTravel.Core.Player
{
    public class PlaybackManager : MonoBehaviour
    {
        public RecordingPlayback RecordingPlayback;
        public int index;
        public int times;
        public RecordingManager recordingManager;

        private bool _hasAppeared;
        private static readonly int VisibilityPercentage = Shader.PropertyToID("VisibilityPercentage");

        public void FixedUpdate()
        {
            if (times == 0)
            {
                recordingManager.CloneWillBeDestroyed();
                Destroy(gameObject);
                RecordingPlayback = null;
            }

            if (RecordingPlayback == null)
            {
                return;
            }

            if (!RecordingPlayback.Started)
            {
                RecordingPlayback.Start();
            }

            float endTime = RecordingPlayback.Recording.GetEndRelativeTime();

            if (_hasAppeared)
            {
                float timeAlive = Time.fixedTime - RecordingPlayback.StartedAt;
                float timeUntilDeath = endTime - timeAlive;
                float visibilityPercentageAfterLife = Mathf.Clamp(timeAlive * 4, 0, 1);
                float visibilityPercentageBeforeDeath = Mathf.Clamp(timeUntilDeath * 4, 0, 1);
                float visibilityPercentage =
                    timeAlive < 0.25f ? visibilityPercentageAfterLife : visibilityPercentageBeforeDeath;

                GetComponentInChildren<Light2D>().intensity = visibilityPercentage;
                GetComponentInChildren<Renderer>().material.SetFloat(VisibilityPercentage, visibilityPercentage);
            }

            foreach (RecordingEvent recordingEvent in RecordingPlayback.GetEvents())
            {
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();

                switch (recordingEvent.EventType)
                {
                    case RecordingEventType.MoveEast:
                        playerMovement.MoveEast();
                        break;
                    case RecordingEventType.MoveWest:
                        playerMovement.MoveWest();
                        break;
                    case RecordingEventType.StopMoving:
                        playerMovement.StopMoving();
                        break;
                    case RecordingEventType.Jump:
                        playerMovement.JumpNow();
                        break;
                    case RecordingEventType.StopHoldingJump:
                        playerMovement.StopHoldingJump();
                        break;
                    case RecordingEventType.Appear:
                        GetComponentInChildren<Light2D>().intensity = 0;
                        GetComponentInChildren<Renderer>().material.SetFloat(VisibilityPercentage, 0);
                        GetComponent<Rigidbody2D>().velocity = recordingEvent.Velocity;
                        transform.position = recordingEvent.Position;
                        GetComponent<PlayerMovement>().SetHorizontalMovement(recordingEvent.HorizontalMovement);
                        _hasAppeared = true;
                        break;
                    case RecordingEventType.Disappear:
                        _hasAppeared = false;
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        times--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (recordingEvent.EventType == RecordingEventType.Disappear)
                {
                    RecordingPlayback.Started = false;
                }
            }
        }
    }
}