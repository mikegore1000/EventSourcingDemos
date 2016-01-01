using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EventSourcingDemos.Demos.Subscriptions
{
    public class LiveOnlySubscriptionDemo : Demo
    {
        private const string StreamId = "$stats-127.0.0.1:2113";

        public override async Task Start()
        {
            Console.WriteLine("Press a key to start the subscription");
            Console.ReadKey();

            var connection = await CreateConnectionAndConnect();
            var subscription = connection.SubscribeToStreamAsync(StreamId, true, RecievedEvent, userCredentials: Credentials);
            
            Logger.Warn("Subscription started, waiting for messages");
            Console.WriteLine("Press a key to exit");
            Console.ReadKey();

            subscription.Dispose();
        }

        private static void RecievedEvent(EventStoreSubscription s, ResolvedEvent e)
        {
            Logger.Info("Received event {0}: {1}", e.OriginalEventNumber, e.Event.EventType);
        }
    }
}