using System;
using System.Collections.Generic;

namespace EventSourcingDemos
{
    public abstract class Aggregate
    {
        private readonly List<object> uncommitttedEvents = new List<object>();

        public IEnumerable<object> UncommittedEvents => uncommitttedEvents;

        public int Version { get; }

        public string StreamId { get; }

        protected void Apply(object @event)
        {
            UpdateState(@event);
            uncommitttedEvents.Add(@event);
        }

        protected Aggregate(string streamId, IEnumerable<object> events)
        {
            StreamId = streamId;
            Version = -1;
            events = events ?? Array.Empty<object>();

            foreach (var e in events)
            {
                Version++;
                UpdateState(e);
            }
        }

        private void UpdateState(object @event)
        {
            ((dynamic)this).UpdateState((dynamic)@event);
        }
    }
}