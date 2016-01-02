using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsureThat;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EventSourcingDemos
{
    public class Repository
    {
        private const int ReadPageSize = 4096; // The max that EventStore allows
        private const string EventTypeHeader = "eventType";

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        private static readonly Encoding Encoding = Encoding.UTF8;

        private readonly IEventStoreConnection connection;
        private readonly UserCredentials credentials;

        public Repository(IEventStoreConnection connection, UserCredentials credentials)
        {
            Ensure.That(connection, nameof(connection)).IsNotNull();
            Ensure.That(credentials, nameof(credentials)).IsNotNull();

            this.connection = connection;
            this.credentials = credentials;
        }

        public async Task SaveAsync(string stream, IEnumerable<object> events, int expectedVersion)
        {
            await connection.AppendToStreamAsync(stream, expectedVersion, events.Select(ToEventData), credentials);
        }

        public async Task<IEnumerable<object>> LoadAsync(string stream)
        {
            StreamEventsSlice eventSlice;
            var eventSliceStart = StreamPosition.Start;
            var events = new List<object>();

            do
            {
                eventSlice = await connection.ReadStreamEventsForwardAsync(stream, eventSliceStart, ReadPageSize, true, credentials);

                // TODO: Handle deleted & not found statuses
                if (eventSlice.Status == SliceReadStatus.Success)
                {
                    events.AddRange(eventSlice.Events.Select(x => ToEventObject(x.OriginalEvent)));
                }

                eventSliceStart = eventSlice.NextEventNumber;

            } while (!eventSlice.IsEndOfStream);

            return events;
        }

        private EventData ToEventData(object @event)
        {
            var headers = new Dictionary<string, string>
            {
                { EventTypeHeader, @event.GetType().FullName }
            };

            byte[] eventData = Encoding.GetBytes(JsonConvert.SerializeObject(@event, JsonSerializerSettings));
            byte[] headerData = Encoding.GetBytes(JsonConvert.SerializeObject(headers, JsonSerializerSettings));

            return new EventData(Guid.NewGuid(), @event.GetType().Name, true, eventData, headerData);
        }

        private object ToEventObject(RecordedEvent eventData)
        {
            var headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.GetString(eventData.Metadata));
            var eventType = Type.GetType(headers[EventTypeHeader]);

            return JsonConvert.DeserializeObject(Encoding.GetString(eventData.Data), eventType);
        }
    }
}