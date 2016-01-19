using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventSourcingDemos.Demos.Aggregates;
using EventSourcingDemos.Events;
using NUnit.Framework;

namespace EventSourcingDemo.UnitTests
{
    [TestFixture]
    public class ShoppingCartAggregateTests
    {
        [Test]
        public void Given_An_Empty_Cart_When_An_Item_Is_Added()
        {
            var aggregate = new ShoppingCartAggregate("StreamId", null);
            aggregate.AddItem("JEANS");

            var @addedEvent = aggregate.UncommittedEvents.OfType<ItemAddedToCart>().FirstOrDefault();

            Assert.That(aggregate.UncommittedEvents.Count(), Is.EqualTo(1), "there should only be a single event");
            Assert.That(@addedEvent, Is.Not.Null, "there should be an ItemAddedToCart event in the stream");
            Assert.That(@addedEvent.CartId, Is.EqualTo("StreamId"));
            Assert.That(@addedEvent.ProductId, Is.EqualTo("JEANS"));
        }
    }
}
