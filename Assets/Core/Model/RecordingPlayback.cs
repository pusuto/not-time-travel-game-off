using System.Collections.Generic;
using UnityEngine;

namespace NotTimeTravel.Core.Model
{
    public class RecordingPlayback
    {
        public float StartedAt;
        public Recording Recording;
        public bool Started;
        public int Done = -1;

        public void Start()
        {
            if (Started)
            {
                return;
            }
            
            StartedAt = Time.fixedTime;
            Done = -1;
            Started = true;
        }

        public List<RecordingEvent> GetEvents()
        {
            if (!Started)
            {
                return new List<RecordingEvent>();
            }
            
            List<RecordingEvent> events = Recording.GetEventsAfter(Time.fixedTime - StartedAt, Done);

            Done += events.Count;

            return events;
        }
    }
}