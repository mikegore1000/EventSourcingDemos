using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EventSourcingDemos.Demos.Subscriptions
{
    public class CatchupSubscriptionDemo : Demo
    {
        public override async Task Start()
        {
            Console.WriteLine("Press a key to start the subscription");
            Console.ReadKey();

            var connection = await CreateConnectionAndConnect();
            connection.SubscribeToAllFrom(lastCheckpoint: null, eventAppeared: RecievedEvent, userCredentials: Credentials, resolveLinkTos: false);

            Console.WriteLine("Press a key to exit");
            Console.ReadKey();
        }

        private static void RecievedEvent(EventStoreCatchUpSubscription s, ResolvedEvent e)
        {
            Logger.Info("Received event {0}: {1}", e.OriginalEventNumber, e.Event.EventType);
        }
    }
}