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
            var paymentId = Guid.NewGuid().ToString();
            var connection = await CreateConnectionAndConnect();
            var repository = new Repository(connection, Credentials);

            var aggregate = new PaymentAggregate(paymentId);
            aggregate.AcceptPayment();

            await repository.SaveAsync(aggregate.Id, aggregate.UncommittedEvents, aggregate.Version);

            var loadedEventStream = await repository.LoadAsync(paymentId);

            Logger.Warn($"Read {loadedEventStream.Count()} events, for aggregate {paymentId}");
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
