using UnityEngine;

namespace NotTimeTravel.Core.Model
{
    public class RecordingEvent
    {
        public Vector2 Position;
        public float HorizontalMovement;
        public Vector2 Velocity;
        public float DeltaTime;
        public RecordingEventType EventType;
    }
}