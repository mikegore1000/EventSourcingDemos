using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EventSourcingDemos.Demos.Aggregates
{
    public class AggregateDemo : Demo
    {
        public override async Task Start()
        {
            var cartId = "ShoppingCart:" + Guid.NewGuid();
            var connection = await CreateConnectionAndConnect();
            var repository = new Repository(connection, Credentials);

            // Call a method to apply events on the aggregate
            var aggregate = new ShoppingCartAggregate(cartId, null);
            aggregate.AddItem("T-SHIRT");

            await repository.SaveAsync(aggregate.StreamId, aggregate.UncommittedEvents, aggregate.Version);

            // Read the aggregate out of the event store
            var loadedEventStream = await repository.LoadAsync(cartId);

            Logger.Warn($"Read {loadedEventStream.Count()} events, for aggregate {cartId}");
            foreach (var e in loadedEventStream)
            {
                Logger.Info($"Event type: {e}");
                Logger.Trace(JsonConvert.SerializeObject(e, Formatting.Indented));
            }

            Console.WriteLine("Press a key to exit");
            Console.ReadKey();
        }
    }
}
