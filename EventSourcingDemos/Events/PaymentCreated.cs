using System;

namespace EventSourcingDemos.Events
{
    public class PaymentCreated
    {
        public PaymentCreated()
        {
            EventId = Guid.NewGuid();
        }

        public Guid EventId { get; set; }

        public Guid PaymentId { get; set; }
    }
}
