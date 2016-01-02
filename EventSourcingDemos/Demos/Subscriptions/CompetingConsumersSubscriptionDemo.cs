using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EventSourcingDemos.Demos.Subscriptions
{
    public class CompetingConsumersSubscriptionDemo : Demo
    {
        private const string StreamId = "$stats-127.0.0.1:2113";
        private const string GroupName = "TestPersistentSubscription";

        public override async Task Start()
        {
            await CreateCompetingConsumersSubscription();
            var connection = await CreateConnectionAndConnect();

            Console.WriteLine("Press a key to start the subscription");
            Console.ReadKey();

            connection.ConnectToPersistentSubscription(StreamId, GroupName, RecievedEvent,userCredentials: Credentials);

            Console.WriteLine("Press a key to delete the subscription");
            Console.ReadKey();

            await connection.DeletePersistentSubscriptionAsync(StreamId, GroupName, Credentials);
        }

        private async Task CreateCompetingConsumersSubscription()
        {
            bool created = await EventStoreHelpers.CreatePersistentSubscriptionAsyncIfRequired(
                StreamId,
                GroupName,
                PersistentSubscriptionSettings.Create()
                    .StartFromBeginning()
                    .PreferRoundRobin()
                    .Build(),
                Credentials,
                CreateConnection);

            if (created)
            {
                Logger.Warn("Created subscription for stream {0} using group name {1}", StreamId, GroupName);
            }
        }

        private static void RecievedEvent(EventStorePersistentSubscriptionBase s, ResolvedEvent e)
        {
            Logger.Info("Received event {0}: {1}", e.OriginalEventNumber, e.Event.EventType);
        }
    }
}
