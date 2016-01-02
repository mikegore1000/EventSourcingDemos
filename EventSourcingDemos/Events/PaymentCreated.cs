using System;

namespace EventSourcingDemos.Events
{
    public class PaymentCreated
    {
        public PaymentCreated(Guid id, string paymentId)
        {
            Id = id;
            PaymentId = paymentId;
        }

        public Guid Id { get; private set; }

        public string PaymentId { get; private set; }
    }
}
