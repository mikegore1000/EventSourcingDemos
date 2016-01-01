using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using NLog;
using ILogger = NLog.ILogger;

namespace EventSourcingDemos.Demos
{
    public abstract class Demo
    {
        protected static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        protected static UserCredentials Credentials { get; } = new UserCredentials("admin", "changeit");

        public abstract Task Start();

        protected async Task<IEventStoreConnection> CreateConnectionAndConnect()
        {
            var connection = CreateConnection();
            await connection.ConnectAsync();

            return connection;
        }

        protected IEventStoreConnection CreateConnection()
        {
            return EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
        }
    }
}