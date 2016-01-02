using System;
using System.Collections.Generic;
using EventSourcingDemos.Events;

namespace EventSourcingDemos.Demos.Aggregates
{
    public class PaymentAggregate : Aggregate
    {
        // TODO: Make the aggregate more realistic!

        private bool created;
        private bool accepted;
        

        public PaymentAggregate(string paymentId) : base(Array.Empty<object>())
        {
            Id = paymentId;
            Apply(new PaymentCreated(Guid.NewGuid(), Id));
        }

        public PaymentAggregate(string paymentId, IEnumerable<object> events) : base(events)
        {
            Id = paymentId;
        }

        public string Id { get; private set; }

        public void AcceptPayment()
        {
            Apply(new PaymentAccepted(Guid.NewGuid(), Id));
        }

        public void UpdateState(PaymentCreated e)
        {
            created = true;
        }

        public void UpdateState(PaymentAccepted e)
        {
            accepted = true;
        }
    }
}