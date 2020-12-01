using System.Collections.Generic;
using UnityEngine;

namespace NotTimeTravel.Core.Model
{
    public class Recording
    {
        private float _startingTime;
        private readonly List<RecordingEvent> _events = new List<RecordingEvent>();

        public float GetEndRelativeTime()
        {
            return _events[_events.Count - 1].DeltaTime;
        }
        
        public Vector2? GetStartingLocation()
        {
            if (_events.Count == 0)
            {
                return null;
            }

            return _events[0].Position;
        }
        
        public void AddEvent(RecordingEvent newEvent)
        {
            float time = Time.fixedTime;

            if (_events.Count == 0)
            {
                _startingTime = time;
            }

            if (newEvent.DeltaTime == 0 && newEvent.EventType != RecordingEventType.Appear)
            {
                newEvent.DeltaTime = time - _startingTime;
            }

            _events.Add(newEvent);
        }

        public List<RecordingEvent> GetEventsAfter(float delta, int afterIndex = -1)
        {
            return _events.GetRange(afterIndex + 1, _events.Count - (afterIndex + 1))
                .FindAll(ev => delta >= ev.DeltaTime);
        }
    }
}