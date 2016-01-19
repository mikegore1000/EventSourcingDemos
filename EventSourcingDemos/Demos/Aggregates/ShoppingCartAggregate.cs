using System;
using System.Collections.Generic;
using EventSourcingDemos.Events;

namespace EventSourcingDemos.Demos.Aggregates
{
    public class ShoppingCartAggregate : Aggregate
    {
        private readonly Dictionary<string, CartItem> items = new Dictionary<string, CartItem>();

        public ShoppingCartAggregate(string cartId, IEnumerable<object> events) : base(cartId, events)
        {
        }

        public void AddItem(string productId)
        {
            var @event = new ItemAddedToCart(Guid.NewGuid(), StreamId, productId, DateTime.UtcNow);
            Apply(@event);
        }

        public void RemoveItem(string productId)
        {
            throw new NotImplementedException("Need to add removal functionality");
        }

        public void UpdateState(ItemAddedToCart @event)
        {
            if (items.ContainsKey(@event.ProductId))
            {
                items[@event.ProductId] = new CartItem(@event.ProductId, items[@event.ProductId].Quantity + 1);
            }
            else
            {
                items.Add(@event.ProductId, new CartItem(@event.ProductId, 1));
            }
        }
    }

    public class CartItem
    {

        public CartItem(string productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public string ProductId { get; }
        public int Quantity { get; }
    }
}