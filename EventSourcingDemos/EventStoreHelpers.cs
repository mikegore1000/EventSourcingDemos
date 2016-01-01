using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EnsureThat;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace EventSourcingDemos
{
    public static class EventStoreHelpers
    {
        public static async Task<bool> CreatePersistentSubscriptionAsyncIfRequired(string stream, string groupName, PersistentSubscriptionSettings settings, UserCredentials credentials, Func<IEventStoreConnection> connectionFactory)
        {
            Ensure.That(stream, nameof(stream)).IsNotNullOrWhiteSpace();
            Ensure.That(groupName, nameof(groupName)).IsNotNullOrWhiteSpace();
            Ensure.That(settings, nameof(settings)).IsNotNull();
            Ensure.That(connectionFactory, nameof(connectionFactory)).IsNotNull();

            using (var connection = connectionFactory())
            {
                await connection.ConnectAsync();
                var createSubscription = await ShouldCreateSubscription(stream, groupName, credentials, new Uri("http://127.0.0.1:2113"));

                if (createSubscription)
                {
                    await connection.CreatePersistentSubscriptionAsync(stream, groupName, settings, credentials).ConfigureAwait(false);
                }

                return createSubscription;
            }
        }

        private static async Task<bool> ShouldCreateSubscription(string stream, string groupName, UserCredentials credentials, Uri eventStoreUri)
        {
            // Need to use the REST API as the .NET client does not provide a built in way to check the existence of a competing consumers subscription
            var builder = new UriBuilder(eventStoreUri) { Path = $"subscriptions/{stream}/{groupName}" };
            var basicCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{credentials.Username}:{credentials.Password}"));

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicCredentials);
            var response = await httpClient.GetAsync(builder.Uri);

            return response.StatusCode == HttpStatusCode.NotFound;
        }
    }
}