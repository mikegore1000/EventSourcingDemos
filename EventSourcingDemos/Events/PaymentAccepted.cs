using System;

namespace EventSourcingDemos.Events
{
    public class PaymentAccepted
    {
        public PaymentAccepted()
        {
            EventId = Guid.NewGuid();
        }

        public Guid EventId { get; set; }

        public Guid PaymentId { get; set; }
    }
}
