using System;

namespace EventSourcingDemos.Events
{
    public class PaymentAccepted
    {
        public PaymentAccepted(Guid id, string paymentId)
        {
            Id = id;
            PaymentId = paymentId;
        }

        public Guid Id { get; private set; }

        public string PaymentId { get; private set; }
    }
}
