using System;

namespace EventSourcingDemos.Events
{
    public class ItemAddedToCart
    {
        public ItemAddedToCart(Guid id, string cartId, string productId, DateTime createdAt)
        {
            Id = id;
            CreatedAt = createdAt;
            CartId = cartId;
            ProductId = productId;
        }

        public string CartId { get; private set; }
        public string ProductId { get; private set; }
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
}
