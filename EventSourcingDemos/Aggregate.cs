using System.Collections.Generic;

namespace EventSourcingDemos
{
    public abstract class Aggregate
    {
        private readonly List<object> uncommitttedEvents = new List<object>();

        public IEnumerable<object> UncommittedEvents => uncommitttedEvents;

        public int Version { get; }

        protected void Apply(object @event)
        {
            UpdateState(@event);
            uncommitttedEvents.Add(@event);
        }

        protected Aggregate(IEnumerable<object> events)
        {
            Version = -1;

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